using System;
using Godot;

public partial class PlayerController : CharacterBody2D
{
	public float moveSpeed = 150.0f;
	public float jumpVelocity = 400.0f;
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private Sprite2D _idleSprite;
	private AnimatedSprite2D _walkSprite;

	public override void _Ready()
	{
		_idleSprite = GetNode<Sprite2D>("IdleSprite");
		_walkSprite = GetNode<AnimatedSprite2D>("WalkSprite");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		_ReadInput(ref velocity);

		_UpdateSpriteRendered(velocity.X);

		// apply gravity after reading input
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void _ReadInput(ref Vector2 velocity)
	{
		Vector2 moveInput = Input.GetVector("left", "right", "up", "down");
		velocity.X = moveInput.X * moveSpeed;
		if (moveInput.Y < 0f)
		{
			velocity.Y = moveInput.Y * moveSpeed;
		}
	}

	private void _UpdateSpriteRendered(float velX)
	{
		bool isWalking = velX != 0;
		_idleSprite.Visible = !isWalking;
		_walkSprite.Visible = isWalking;

		if (isWalking)
		{
			_walkSprite.Play();
			_walkSprite.FlipH = velX < 0;
		}
	}
}
