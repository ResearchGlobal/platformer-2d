using System;
using Godot;

public partial class RedState : StateTL<ShapeShifter>
{
	// https://www.youtube.com/watch?v=Kcg1SEgDqyk 12:55

	public override void Enter()
	{
		GetNode<Sprite2D>("RedDude").Visible = true;
	}

	public override void Exit()
	{
		GetNode<Sprite2D>("RedDude").Visible = false;
	}
}
