using System;

namespace Pseudo
{
	[Flags]
	public enum Axes
	{
		None = 0,
		X = 1,
		Y = 2,
		Z = 4,
		XY = 3,
		XZ = 5,
		YZ = 6,
		XYZ = 7,
		W = 8,
		XW = 9,
		YW = 10,
		XYW = 11,
		ZW = 12,
		XZW = 13,
		YZW = 14,
		XYZW = 15
	}

	[Flags]
	public enum Channels
	{
		None = 0,
		R = 1,
		G = 2,
		B = 4,
		RG = 3,
		RB = 5,
		GB = 6,
		RGB = 7,
		A = 8,
		RA = 9,
		GA = 10,
		RGA = 11,
		BA = 12,
		RBA = 13,
		GBA = 14,
		RGBA = 15
	}

	[Flags]
	public enum TransformModes
	{
		None = 0,
		Position = 1,
		Rotation = 2,
		Scale = 4,
		PositionRotation = 3,
		PositionScale = 5,
		RotationScale = 6,
		PositionRotationScale = 7
	}

	public enum InterpolationModes
	{
		Quadratic,
		Linear
	}

	public enum ProbabilityDistributions
	{
		Uniform,
		Normal,
		Proportional
	}

	public enum RaycastHitModes
	{
		First,
		FirstOfEach,
		All
	}

	public enum ControllerTypes
	{
		Keyboard,
		Joystick
	}

	public enum ButtonStates
	{
		Down,
		Up,
		Pressed
	}

	public enum Joysticks
	{
		Any = 0,
		Joystick1 = 1,
		Joystick2 = 2,
		Joystick3 = 3,
		Joystick4 = 4,
		Joystick5 = 5,
		Joystick6 = 6,
		Joystick7 = 7,
		Joystick8 = 8,
	}

	public enum JoystickButtons
	{
		Cross_A = 0,
		Circle_B = 1,
		Square_X = 2,
		Triangle_Y = 3,
		L1 = 4,
		R1 = 5,
		Select = 6,
		Start = 7,
		LeftStick = 8,
		RigthStick = 9,
		Button10 = 10,
		Button11 = 11,
		Button12 = 12,
		Button13 = 13,
		Button14 = 14,
		Button15 = 15,
		Button16 = 16,
		Button17 = 17,
		Button18 = 18,
		Button19 = 19
	}

	public enum JoystickAxes
	{
		LeftStickX = 0,
		LeftStickY = 1,
		RightStickX = 3,
		RightStickY = 4,
		DirectionalPadX = 5,
		DirectionalPadY = 6,
		LeftTrigger = 7,
		RightTrigger = 8
	}
}

