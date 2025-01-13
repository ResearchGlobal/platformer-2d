using System;
using System.Collections.Generic;
using Godot;

/// <summary>
/// Need to start thinking about how to properly structure different enemy types.
/// Refer back to component pattern from Nystrom's Game Programming Patterns book.
/// </summary>
public partial class ShapeShifter : Node
{
	[Export]
	public NodePath initialState;

	private Dictionary<string, StateTL<ShapeShifter>> _stateNodes;
	private StateTL<ShapeShifter> _currentStateNode;

	public override void _Ready()
	{
		_stateNodes = new Dictionary<string, StateTL<ShapeShifter>>();
		foreach (Node node in GetChildren())
		{
			if (node is StateTL<ShapeShifter> s)
			{
				_stateNodes[node.Name] = s;
				s.stateMachine = this;
				s.Ready();
				s.Exit(); //reset all states
			}
		}
		_currentStateNode = GetNode<StateTL<ShapeShifter>>(initialState);
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

	public void OnDamage()
	{
		if (_currentStateNode.Name == "Green")
		{
			TransitionTo("Yellow");
		}
		else if (_currentStateNode.Name == "Yellow")
		{
			TransitionTo("Red");
		}
		else if (_currentStateNode.Name == "Red")
		{
			TransitionTo("Green");
		}
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
