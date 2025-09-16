using System;
using System.Threading.Tasks;
using Godot;

public partial class MainRoot : Node2D
{
	public static WSServer server = new WSServer();

	static async Task serverStart()
	{
		GD.Print("Starting server in root");
		await server.StartAsync();
	}

	public override async void _Ready()
	{
		await serverStart();
	}
}
