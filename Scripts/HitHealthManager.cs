using System;
using Godot;

public partial class HitHealthManager : Control
{
	[Export]
	private PackedScene _scoreLabelPrefab;

	[Export]
	private ProgressBar _healthBar;

	private AnimationPlayer _buffAnim;
	private TextureRect _buffIcon;
	private bool _buffIsAlive = true;

	public override void _Ready()
	{
		//set up active node references
		_buffIcon = GetNode<TextureRect>("Buffalo");
		_buffAnim = GetNode<AnimationPlayer>("Buffalo/BuffGetsHit");
		_healthBar.Value = _healthBar.MaxValue;
	}

	private void _OnBuffaloGuiInpuut(InputEvent @event)
	{
		//disable input by shortcircuiting if _buffIsAlive is false
		if (!_buffIsAlive)
			return;

		// find a better way to evaluate this condition, this is very imperative for reading a mouse button clicked
		//this should be wired up to controller input eventually or to collision callbacks
		if (@event is InputEventMouseButton e && e.Pressed && e.ButtonIndex == MouseButton.Left)
		{
			_buffAnim.Play("take-hit");
			int dmg = GD.RandRange(0, 100);
			_SpawnScoreLabel(dmg, e.GlobalPosition);
			_healthBar.Value -= dmg;
			if (_healthBar.Value <= 0)
			{
				_buffIsAlive = false;
				//changes the co
				_buffIcon.Texture = GD.Load<Texture2D>("res://Sprites/buffalo/buffalo_dead.png");
			}
		}
	}

	private async void _SpawnScoreLabel(float value, Vector2 pos)
	{
		Label l = _scoreLabelPrefab.Instantiate<Label>();
		l.Text = $"{value}";
		AddChild(l);
		l.GlobalPosition = pos;

		//animate & destroy with tween
		Tween tween = GetTree().CreateTween();
		tween.SetParallel(true);
		tween
			.TweenProperty(l, "position:y", pos.Y + 300, 0.5f)
			.SetEase(Tween.EaseType.In)
			.SetTrans(Tween.TransitionType.Back);
		tween.TweenProperty(l, "position:x", pos.X + GD.RandRange(-50, 50), 0.5f);
		tween.TweenProperty(l, "modulate:a", 0, 0.45f);

		await ToSignal(tween, Tween.SignalName.Finished);
		l.QueueFree();
	}
}
