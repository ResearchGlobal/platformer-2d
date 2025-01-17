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

	private ProgressBar _healthBar;
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
		_healthBar = GetNode<ProgressBar>("Health/HealthBar");
		_healthBar.Value = _healthBar.MaxValue;
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
		_healthBar.Value -= GD.RandRange(0, 25);
		GD.Print("health " + _healthBar.Value);

		if (_healthBar.Value < 67 && _healthBar.Value > 33)
		{
			TransitionTo("Yellow");
		}
		else if (_healthBar.Value <= 33 && _healthBar.Value > 0)
		{
			TransitionTo("Red");
		}
		else if (_healthBar.Value <= 0)
		{
			QueueFree();
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
