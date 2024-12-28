using System;
using Godot;

public partial class RigidBody2d : RigidBody2D
{
	[Signal]
	//https://www.youtube.com/watch?v=Kcg1SEgDqyk // 12:55
	public delegate void ThisSignalEventHandler();

	public void OnRigidBodyEntered(Node node)
	{
		GD.Print("Rigid body entered by " + node.Name);
	}
}
