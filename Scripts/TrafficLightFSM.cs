using System;
using System.Collections.Generic;
using Godot;

public partial class TrafficLightFSM : Node
{
	[Export]
	public NodePath initialState;

	private Dictionary<string, StateTL> _states;
	private StateTL _currentState;

	public override void _Ready()
	{
		_states = new Dictionary<string, StateTL>();
		foreach (Node node in GetChildren())
		{
			if (node is StateTL s)
			{
				_states[node.Name] = s;
				s.fsm = this;
				s.Ready();
				s.Exit(); //reset all states
			}
		}
		_currentState = GetNode<StateTL>(initialState);
		_currentState.Enter();
	}

	public override void _Process(double delta)
	{
		_currentState.Update((float)delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		_currentState.PhysicsUpdate((float)delta);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		_currentState.HandleInput(@event);
	}

	public void TransitionTo(string key)
	{
		if (!_states.ContainsKey(key) || _currentState == _states[key])
			return;
		_currentState.Exit();
		_currentState = _states[key];
		_currentState.Enter();
	}
}
