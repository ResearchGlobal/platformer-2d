using System;
using Godot;

public partial class StateTL<FiniteStateMachine> : Node
{
	public FiniteStateMachine stateMachine;

	public virtual void Enter() { }

	public virtual void Exit() { }

	public virtual void Ready() { }

	public virtual void Update(float delta) { }

	public virtual void PhysicsUpdate(float delta) { }

	public virtual void HandleInput(InputEvent @event) { }
}
