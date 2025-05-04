using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Godot;

public class WSServer
{
	private readonly HttpListener _listener;
	private readonly string _url;

	public WSServer(string url)
	{
		_url = url;
		_listener = new HttpListener();
		_listener.Prefixes.Add(url);
	}

	public async Task StartAsync()
	{
		_listener.Start();

		GD.Print($"WebSocket server started at {_url}");

		// while (true)
		// {
		// 	HttpListenerContext context = await _listener.GetContextAsync();
		// 	if (context.Request.IsWebSocketRequest)
		// 	{
		// 		ProcessWebSocketRequest(context);
		// 	}
		// 	else
		// 	{
		// 		context.Response.StatusCode = 400;
		// 		context.Response.Close();
		// 	}
		// }
	}

	public async Task HandleInbounds()
	{
		while (true)
		{
			HttpListenerContext context = await _listener.GetContextAsync();
			if (context.Request.IsWebSocketRequest)
			{
				ProcessWebSocketRequest(context);
			}
			else
			{
				context.Response.StatusCode = 400;
				context.Response.Close();
			}
		}
	}

	private async void ProcessWebSocketRequest(HttpListenerContext context)
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
						GD.Print("Received JSON: " + jsonString);

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
