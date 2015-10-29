using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class InputManager : Singleton<InputManager>
	{
		static KeyCode[] allKeys;
		static KeyCode[] keyboardKeys;
		static KeyCode[] joystickKeys;
		static Dictionary<int, KeyCode[]> joystickKeysDict;
		static KeyCode[] nonjoystickKeys;
		static KeyCode[] mouseKeys;
		static KeyCode[] letterKeys;
		static KeyCode[] functionKeys;
		static KeyCode[] numberKeys;
		static KeyCode[] keypadKeys;
		static KeyCode[] arrowKeys;
		static KeyCode[] modifierKeys;

		[SerializeField]
		PlayerInput[] players = new PlayerInput[0];

		static InputManager()
		{
			SortKeys();
		}

		public PlayerInput GetPlayer(int playerIndex)
		{
			return players[playerIndex];
		}

		void Reset()
		{
			InputUtility.SetInputManager();
		}

		public static Joysticks KeyToJoystick(KeyCode key)
		{
			string keyName = key.ToString();
			int joystickIndex = int.Parse(char.IsNumber(keyName[8]) ? char.IsNumber(keyName[9]) ? keyName.GetRange(8, 2) : keyName.GetRange(8, 1) : "0");

			return (Joysticks)joystickIndex;
		}

		public static JoystickButtons KeyToJoystickButton(KeyCode key)
		{
			string keyName = key.ToString();
			int joystickIndex = int.Parse(char.IsNumber(keyName[keyName.Length - 2]) ? keyName.GetRange(keyName.Length - 2) : keyName.GetRange(keyName.Length - 1));

			return (JoystickButtons)joystickIndex;
		}

		public static KeyCode JoystickInputToKey(Joysticks joystick, JoystickButtons button)
		{
			return GetJoystickKeys(joystick)[(int)button];
		}

		public static Joysticks AxisToJoystick(string axis)
		{
			int length = axis.StartsWith("Any") ? 3 : char.IsNumber(axis[9]) ? 9 : 8;
			string joystickName = axis.Substring(0, length);

			return (Joysticks)System.Enum.Parse(typeof(Joysticks), joystickName);
		}

		public static JoystickAxes AxisToJoystickAxis(string axis)
		{
			int startIndex = axis.StartsWith("Any") ? 3 : char.IsNumber(axis[9]) ? 9 : 8;
			string axisName = axis.Substring(startIndex);

			return (JoystickAxes)System.Enum.Parse(typeof(JoystickAxes), axisName);
		}

		public static string JoystickInputToAxis(Joysticks joystick, JoystickAxes axis)
		{
			return joystick.ToString() + axis;
		}

		public static KeyCode[] GetPressedKeys(KeyCode[] keys)
		{
			List<KeyCode> pressed = new List<KeyCode>();

			for (int i = 0; i < keys.Length; i++)
			{
				KeyCode key = keys[i];

				if (Input.GetKey(key))
					pressed.Add(key);
			}

			return pressed.ToArray();
		}

		public static KeyCode[] GetPressedKeys() { return GetPressedKeys(allKeys); }
		public static KeyCode[] GetAllKeys() { return allKeys; }
		public static KeyCode[] GetKeyboardKeys() { return keypadKeys; }
		public static KeyCode[] GetJoystickKeys() { return joystickKeys; }
		public static KeyCode[] GetJoystickKeys(Joysticks joystick) { return joystickKeysDict[(int)joystick]; }
		public static KeyCode[] GetNonJoystickKeys() { return nonjoystickKeys; }
		public static KeyCode[] GetMouseKeys() { return mouseKeys; }
		public static KeyCode[] GetLetterKeys() { return letterKeys; }
		public static KeyCode[] GetFunctionKeys() { return functionKeys; }
		public static KeyCode[] GetNumberKeys() { return numberKeys; }
		public static KeyCode[] GetKeypadKeys() { return keypadKeys; }
		public static KeyCode[] GetArrowKeys() { return arrowKeys; }
		public static KeyCode[] GetModifierKeys() { return modifierKeys; }

		public static bool IsKeyboardKey(KeyCode key) { return keyboardKeys.Contains(key); }
		public static bool IsJoystickKey(KeyCode key) { return joystickKeys.Contains(key); }
		public static bool IsMouseKey(KeyCode key) { return mouseKeys.Contains(key); }

		static void SortKeys()
		{
			allKeys = new KeyCode[321];
			keyboardKeys = new KeyCode[134];
			joystickKeys = new KeyCode[180];
			joystickKeysDict = new Dictionary<int, KeyCode[]>();
			nonjoystickKeys = new KeyCode[141];
			mouseKeys = new KeyCode[7];
			letterKeys = new KeyCode[26];
			functionKeys = new KeyCode[15];
			numberKeys = new KeyCode[10];
			keypadKeys = new KeyCode[17];
			arrowKeys = new KeyCode[4];
			modifierKeys = new KeyCode[7];

			int allCounter = 0;
			int keyboardCounter = 0;
			int joystickCounter = 0;
			int nonJoystickCounter = 0;
			int mouseCounter = 0;
			int letterCounter = 0;
			int functionCounter = 0;
			int alphaCounter = 0;
			int keypadCounter = 0;
			int arrowCounter = 0;
			int modifierCounter = 0;

			foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
			{
				string keyName = key.ToString();

				allKeys[allCounter++] = key;

				if (!keyName.StartsWith("Joystick") && !keyName.StartsWith("Mouse"))
					keyboardKeys[keyboardCounter++] = key;

				if (keyName.StartsWith("Joystick"))
				{
					int joystick = (int)KeyToJoystick(key);
					int joystickButton = (int)KeyToJoystickButton(key);

					if (!joystickKeysDict.ContainsKey(joystick))
						joystickKeysDict[joystick] = new KeyCode[20];

					joystickKeysDict[joystick][joystickButton] = key;
					joystickKeys[joystickCounter++] = key;
				}
				else
					nonjoystickKeys[nonJoystickCounter++] = key;

				if (keyName.StartsWith("Mouse"))
					mouseKeys[mouseCounter++] = key;

				if (keyName.Length == 1 && char.IsLetter(keyName[0]))
					letterKeys[letterCounter++] = key;

				if ((keyName.Length == 2 || keyName.Length == 3) && keyName.StartsWith("F"))
					functionKeys[functionCounter++] = key;

				if (keyName.StartsWith("Alpha"))
					numberKeys[alphaCounter++] = key;

				if (keyName.StartsWith("Keypad"))
					keypadKeys[keypadCounter++] = key;

				if (keyName.EndsWith("Arrow"))
					arrowKeys[arrowCounter++] = key;

				if (keyName.EndsWith("Shift") || keyName.EndsWith("Alt") || keyName.EndsWith("Control") || keyName.StartsWith("Alt"))
					modifierKeys[modifierCounter++] = key;
			}
		}
	}
}