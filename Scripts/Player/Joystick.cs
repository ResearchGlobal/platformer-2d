using Godot;

/// <summary>
/// <para>
/// Joystick API based on this github repository:
/// https://github.com/elmarti/react-joystick-component
/// </para>
/// <para>
/// and extended here:
/// https://github.com/ResearchGlobal/universal-react-gamepad/blob/main/libs/input-components/src/types/InputComponent.types.ts
/// </para>
/// </summary>
public class Joystick
{
	public JoyDirection? Direction { get; set; }
	public JoyLeftOrRight LeftOrRight { get; set; }
	public JoyType Type { get; set; }

	public double? Distance { get; set; }
	public double? TimestampInMs { get; set; }
	public double? X { get; set; } = 0;
	public double? Y { get; set; } = 0;

	public JoyAxis GetJoyAxis(XorY xOrY)
	{
		if (LeftOrRight == JoyLeftOrRight.L)
		{
			if (xOrY == XorY.X)
			{
				return JoyAxis.LeftX;
			}
			else if (xOrY == XorY.Y)
			{
				return JoyAxis.LeftY;
			}
		}
		if (LeftOrRight == JoyLeftOrRight.R)
		{
			if (xOrY == XorY.X)
			{
				return JoyAxis.RightX;
			}
			else if (xOrY == XorY.Y)
			{
				return JoyAxis.RightY;
			}
		}
		return JoyAxis.Invalid;
	}
}

public enum JoyType
{
	move,
	stop,
	start,
}

public enum JoyDirection
{
	FORWARD,
	RIGHT,
	LEFT,
	BACKWARD,
}

public enum JoyLeftOrRight
{
	L,
	R,
}

public enum XorY
{
	X,
	Y,
}
