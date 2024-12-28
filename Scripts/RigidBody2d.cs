using System;
using Godot;

public partial class RigidBody2d : RigidBody2D
{
	public void OnRigidBodyEntered(Node node)
	{
		GD.Print("Rigid body entered by " + node.Name);
	}
}
