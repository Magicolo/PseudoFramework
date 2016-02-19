using Pseudo.Internal;
using UnityEngine;

namespace Pseudo
{
	public enum ControllerTypes
	{
		Mouse,
		Keyboard,
		Joystick
	}

	public enum MouseButtons
	{
		LeftClick = KeyCode.Mouse0,
		RightClick = KeyCode.Mouse1,
		MiddleClick = KeyCode.Mouse2,
		Mouse3 = KeyCode.Mouse3,
		Mouse4 = KeyCode.Mouse4,
		Mouse5 = KeyCode.Mouse5,
		Mouse6 = KeyCode.Mouse6,
	}

	public enum MouseAxes
	{
		X,
		Y,
		WheelX,
		WheelY
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

	public enum Players
	{
		None,
		Player1,
		Player2,
		Player3,
		Player4,
		Player5,
		Player6,
		Player7,
		Player8
	}

	public interface IInputManager
	{
		void AddInput(PlayerInput input);
		void AssignInput(Players player, string inputName);
		void AssignInput(Players player, PlayerInput input);
		PlayerInput GetAssignedInput(Players player);
		float GetAxis(Players player, string actionName);
		float GetAxis(Players player, string actionName, Vector2 relativeScreenPosition);
		PlayerInput GetInput(string inputName);
		bool GetKey(Players player, string actionName);
		bool GetKey(Players player, string actionName, Vector2 relativeScreenPosition);
		bool GetKeyDown(Players player, string actionName);
		bool GetKeyDown(Players player, string actionName, Vector2 relativeScreenPosition);
		bool GetKeyUp(Players player, string actionName);
		bool GetKeyUp(Players player, string actionName, Vector2 relativeScreenPosition);
		bool IsAssigned(Players player);
		void UnassignInput(Players player);
	}
}