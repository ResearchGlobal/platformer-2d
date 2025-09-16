using System;
using System.Numerics;
using Godot;
using Newtonsoft.Json;

public class InputController
{
	public void ReadDirectionInput(ref Godot.Vector2 velocity, float moveSpeed)
	{
		Godot.Vector2 moveInput = Input.GetVector("left", "right", "down", "up", 0f);
		velocity.X = moveInput.X * moveSpeed;
		if (moveInput.Y < 0f)
		{
			velocity.Y = moveInput.Y * moveSpeed;
		}
	}

	public void FireInputFromMessage(string message)
	{
		Joystick? joystickCommand = JsonConvert.DeserializeObject<Joystick>(message);
		if (joystickCommand != null)
		{
			FireJoyStickEvent(joystickCommand);
		}
	}

	public void FireJoyStickEvent(Joystick command)
	{
		//fire x and y separately
		InputEventJoypadMotion joystickInputX = new InputEventJoypadMotion
		{
			Device = 0,
			Axis = command.GetJoyAxis(XorY.X),
			AxisValue = (float)(command.X ?? 0f),
		};
		InputEventJoypadMotion joystickInputY = new InputEventJoypadMotion
		{
			Device = 0,
			Axis = command.GetJoyAxis(XorY.Y),
			AxisValue = (float)(command.Y ?? 0f),
		};
		Input.ParseInputEvent(joystickInputX);
		Input.ParseInputEvent(joystickInputY);
	}
}
