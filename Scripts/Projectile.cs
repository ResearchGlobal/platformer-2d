using System;
using Godot;

public partial class Projectile : RigidBody2D
{
	[Export]
	private float damage = 1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Timer timer = GetNode<Timer>("Timer");
		timer.Timeout += () => QueueFree();
	}

	public void OnBodyEntered(Node node)
	{
		GD.Print("Projectile body entered called " + node.Name);
		if (node.IsInGroup("enemy_npc"))
		{
			GD.Print("projectile collided with " + node.Name);
		}
	}
}
