using System;
using Godot;

public partial class Health : Node2D
{
	[Export]
	private float _maxHealth = 100f;

	[Export]
	private ProgressBar _healthBar;

	private float CurrentHealth;

	public override void _Ready()
	{
		CurrentHealth = _maxHealth;
	}

	public void Damage(float damage)
	{
		GD.Print("Damage " + damage);
		CurrentHealth -= damage;
		_healthBar.Value -= damage;
		if (CurrentHealth <= 0)
		{
			GetParent().QueueFree();
		}
	}
}
