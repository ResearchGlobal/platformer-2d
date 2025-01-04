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
		HoldItemUpRight(ref mousePosition);
		ReadInput();
	}

	private void ReadInput()
	{
		if (Input.IsActionJustPressed("shoot"))
		{
			string shootOrShootFlipped = _isFlipped ? "shoot_flipped" : "shoot";
			_shoot.Play(shootOrShootFlipped);
			_use.Play();
		}
	}

	private void HoldItemUpRight(ref Vector2 mp)
	{
		if (ShouldFlip() && !_isFlipped)
		{
			_isFlipped = true;
			_use.FlipV = true;
		}
		if (!ShouldFlip() && _isFlipped)
		{
			_isFlipped = false;
			_use.FlipV = false;
		}
	}

	private bool ShouldFlip()
	{
		float angle = Math.Abs(Rotation / (float)Math.PI * 180f % 360f);
		if (90f <= angle && angle <= 270f)
		{
			return true;
		}
		else
			return false;
	}
}
