using System;
using Godot;

public partial class TileMapLayer : Godot.TileMapLayer
{
	public void OnTileMapEntered(Node node)
	{
		GD.Print("TILE Entered " + node.Name);
	}
}
