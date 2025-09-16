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

	public void ProcessJoystickEvent(Joystick? joystickCommand)
	{
		if (joystickCommand != null)
		{
			FireJoystickEvent(joystickCommand);
		}
	}

	public void ProcessInputEvent(string message)
	{
		//assert input event type: joystick, button press, etc...
		//right now this is only configured to handle joystick input events
		Joystick? joystickCommand = JsonConvert.DeserializeObject<Joystick>(message);
		ProcessJoystickEvent(joystickCommand);
	}

	public void FireJoystickEvent(Joystick command)
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
