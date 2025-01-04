using System;
using Godot;

public partial class Item : Node2D
{
	[Signal]
	public delegate void OnUseItemEventHandler(Node2D node);

	private bool _isFlipped = false;
	private AnimatedSprite2D _use;
	private AnimationPlayer _shoot;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_use = GetNode<AnimatedSprite2D>("Use");
		_shoot = GetNode<AnimationPlayer>("Shoot");
	}

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
			_shoot.Play("shoot");
			_use.Play();
		}
	}

	private void _holdItemUpRight(ref Vector2 mp)
	{
		// Vector2 rootPosition = GetNode<Node2D>("./Item").Position;
		// Position.
		if (mp.X < Position.X && !_isFlipped)
		{
			_isFlipped = true;
			_use.FlipV = true;
		}

		if (mp.X > Position.X && _isFlipped)
		{
			_isFlipped = false;
			_use.FlipV = false;
		}
	}
}
