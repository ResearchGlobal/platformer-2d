using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Godot;

public delegate void OnInboundCallback(string message);

public class WSServer
{
	private readonly HttpListener _listener;
	private readonly string _portSuffix = ":8080/";
	private string _url = "http://192.168.2.125:8080/";

	public WSServer()
	{
		_listener = new HttpListener();
	}

	private IPAddress? GetWifiIpAddress()
	{
		IPAddress[] finalList = { };
		// Get all network interfaces
		NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

		foreach (NetworkInterface ni in interfaces)
		{
			// Check for operational interfaces (exclude loopback and tunnel)
			if (
				ni.OperationalStatus == OperationalStatus.Up
				&& ni.NetworkInterfaceType != NetworkInterfaceType.Loopback
				&& ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel
			)
			{
				// Get IP properties
				var local = ni.GetIPProperties()
					.UnicastAddresses.FirstOrDefault(addr =>
						addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
					);

				if (local != null)
				{
					if (ni.Name == "Wi-Fi")
					{
						GD.Print($"Wi-Fi ip found at: {local.Address}");
						return local.Address;
					}
				}
			}
		}
		return null;
	}

	public async Task StartAsync()
	{
		IPAddress? wifiIp = GetWifiIpAddress();
		if (wifiIp != null)
		{
			_url = $"http://{wifiIp}{_portSuffix}";
		}
		GD.Print($"WebSocket server starting at {_url}");
		_listener.Prefixes.Add(_url);
		_listener.Start();
	}

	public async Task HandleInbounds(OnInboundCallback callback)
	{
		while (true)
		{
			HttpListenerContext context = await _listener.GetContextAsync();
			if (context.Request.IsWebSocketRequest)
			{
				ProcessWebSocketRequest(context, callback);
			}
			else
			{
				context.Response.StatusCode = 400;
				context.Response.Close();
			}
		}
	}

	private async void ProcessWebSocketRequest(
		HttpListenerContext context,
		OnInboundCallback callback
	)
	{
		HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
		WebSocket webSocket = webSocketContext.WebSocket;

		try
		{
			byte[] buffer = new byte[1024 * 4];

			while (webSocket.State == WebSocketState.Open)
			{
				WebSocketReceiveResult result = await webSocket.ReceiveAsync(
					new ArraySegment<byte>(buffer),
					CancellationToken.None
				);

				if (result.MessageType == WebSocketMessageType.Text)
				{
					string jsonString = Encoding.UTF8.GetString(buffer, 0, result.Count);
					try
					{
						// Deserialize JSON data
						JsonDocument jsonDoc = JsonDocument.Parse(jsonString);
						// GD.Print("Received JSON: " + jsonString);
						callback(jsonString);

						// Example response
						var response = new
						{
							status = "received",
							receivedData = jsonDoc.RootElement.ToString(),
							timestamp = DateTime.UtcNow,
						};

						string responseJson = JsonSerializer.Serialize(response);
						byte[] responseBytes = Encoding.UTF8.GetBytes(responseJson);

						await webSocket.SendAsync(
							new ArraySegment<byte>(responseBytes),
							WebSocketMessageType.Text,
							true,
							CancellationToken.None
						);
					}
					catch (JsonException ex)
					{
						GD.Print($"Invalid JSON received: {ex.Message}");
						string errorResponse = JsonSerializer.Serialize(
							new { error = "Invalid JSON format" }
						);
						byte[] errorBytes = Encoding.UTF8.GetBytes(errorResponse);
						await webSocket.SendAsync(
							new ArraySegment<byte>(errorBytes),
							WebSocketMessageType.Text,
							true,
							CancellationToken.None
						);
					}
				}
				else if (result.MessageType == WebSocketMessageType.Close)
				{
					await webSocket.CloseAsync(
						WebSocketCloseStatus.NormalClosure,
						"Closing",
						CancellationToken.None
					);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			GD.Print($"WebSocket error: {ex.Message}");
		}
		finally
		{
			webSocket.Dispose();
		}
	}
}
