using System;
using System.Threading.Tasks;
using Godot;

public partial class PlayerController : CharacterBody2D
{
	[Export]
	private bool _isGravityEnabled;

	private float _moveSpeed = 150.0f;
	private float _jumpVelocity = 400.0f;
	private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private Sprite2D _idleSprite;
	private AnimatedSprite2D _walkSprite;
	private InputController inputController = new InputController();

	private static WSServer _server = new WSServer();

	static async Task serverStart()
	{
		GD.Print("Starting server");
		await _server.StartAsync();
	}

	public override async void _Ready()
	{
		_idleSprite = GetNode<Sprite2D>("IdleSprite");
		_walkSprite = GetNode<AnimatedSprite2D>("WalkSprite");
		await serverStart();
	}

	public override async void _Process(double delta)
	{
		await _server.HandleInbounds(inputController.FireInputFromMessage);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		inputController.ReadInput(ref velocity, _moveSpeed);

		UpdateSpriteRendered(velocity.X);

		// apply gravity after reading input
		if (_isGravityEnabled && !IsOnFloor())
		{
			velocity.Y += _gravity * (float)delta;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void UpdateSpriteRendered(float velX)
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
