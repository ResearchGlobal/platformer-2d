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

	public void OnBodyEntered(Node2D node)
	{
		if (node.IsInGroup("enemy_npc"))
		{
			//cast to Shapeshifter - should really be some enemy prototype that abstracts the InDmaage call
			var localNode = (ShapeShifter)node;
			localNode.OnDamage();
			QueueFree();
		}
	}
}
