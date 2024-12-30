using System;
using Godot;

public partial class GreenState : StateTL<ShapeShifter>
{
	// https://www.youtube.com/watch?v=Kcg1SEgDqyk 12:55

	public override void Enter()
	{
		GetNode<Sprite2D>("GreenDude").Visible = true;
		GetNode<Timer>("Timer2s").Start();
	}

	public override void Exit()
	{
		GetNode<Sprite2D>("GreenDude").Visible = false;
		GetNode<Timer>("Timer2s").Stop();
	}

	private void OnTimerTmeout()
	{
		stateMachine.TransitionTo("Yellow");
	}
}
