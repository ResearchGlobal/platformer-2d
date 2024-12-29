using System;
using System.Collections.Generic;
using Godot;

public partial class TrafficLightFSM : Node
{
	[Export]
	public NodePath initialState;

	private Dictionary<string, StateTL> _stateNodes;
	private StateTL _currentStateNode;

	public override void _Ready()
	{
		_stateNodes = new Dictionary<string, StateTL>();
		foreach (Node node in GetChildren())
		{
			if (node is StateTL s)
			{
				_stateNodes[node.Name] = s;
				s.fsm = this;
				s.Ready();
				s.Exit(); //reset all states
			}
		}
		_currentStateNode = GetNode<StateTL>(initialState);
		_currentStateNode.Enter();
	}

	public override void _Process(double delta)
	{
		_currentStateNode.Update((float)delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		_currentStateNode.PhysicsUpdate((float)delta);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		_currentStateNode.HandleInput(@event);
	}

	//Consider typing key to key of nodes in tree
	// see https://plbonneville.com/blog/string-literal-types-in-csharp/ for C# literal types
	public void TransitionTo(string key)
	{
		if (!_stateNodes.ContainsKey(key) || _currentStateNode == _stateNodes[key])
			return;
		_currentStateNode.Exit();
		_currentStateNode = _stateNodes[key];
		_currentStateNode.Enter();
	}
}
