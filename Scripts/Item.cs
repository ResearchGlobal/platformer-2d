using System;
using Godot;

public partial class Item : Node2D
{
	[Signal]
	public delegate void OnUseItemEventHandler(Node2D node);

	private bool _isFlipped = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() { }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		LookAt(mousePosition);
		_holdItemUpRight(ref mousePosition);
		_ReadInput();
	}

	private void _ReadInput()
	{
		if (Input.IsActionJustPressed("shoot"))
		{
			GetNode<AnimationPlayer>("Shoot").Play("shoot");
			GetNode<AnimatedSprite2D>("Use").Play();
		}
	}

	private void _holdItemUpRight(ref Vector2 mp)
	{
		Vector2 rootPosition = GetNode<Node2D>("Item").Position;
		if (mp.X < rootPosition.X && !_isFlipped)
		{
			AnimatedSprite2D use = GetNode<AnimatedSprite2D>("Item/Use");
			_isFlipped = true;
			use.FlipV = true;
		}

		if (mp.X > rootPosition.X && _isFlipped)
		{
			AnimatedSprite2D use = GetNode<AnimatedSprite2D>("Item/Use");
			_isFlipped = false;
			use.FlipV = false;
		}
	}
}
