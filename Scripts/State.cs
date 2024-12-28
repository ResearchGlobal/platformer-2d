using System;
using Godot;

public partial class StateTL : Node
{
	public TrafficLightFSM fsm;

	public virtual void Enter() { }

	public virtual void Exit() { }

	public virtual void Ready() { }

	public virtual void Update(float delta) { }

	public virtual void PhysicsUpdate(float delta) { }

	public virtual void HandleInput(InputEvent @event) { }
}
