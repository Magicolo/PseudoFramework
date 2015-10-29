using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal.Input
{
	public static class InputUtility
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

		static InputUtility()
		{
			SortKeys();
		}

		public static void SetupInputManager()
		{
#if UNITY_EDITOR
			UnityEditor.SerializedObject inputManagerSerialized = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset"));
			UnityEditor.SerializedProperty axesProperty = inputManagerSerialized.FindProperty("m_Axes");

			InputManager.Joysticks[] joysticks = (InputManager.Joysticks[])System.Enum.GetValues(typeof(InputManager.Joysticks));

			for (int i = 0; i < joysticks.Length; i++)
			{
				InputManager.Joysticks joystick = joysticks[i];
				InputManager.JoystickAxes[] joystickAxes = (InputManager.JoystickAxes[])System.Enum.GetValues(typeof(InputManager.JoystickAxes));

				for (int j = 0; j < joystickAxes.Length; j++)
				{
					InputManager.JoystickAxes joystickAxis = joystickAxes[j];
					string axis = joystick.ToString() + joystickAxis;

					UnityEditor.SerializedProperty currentAxisProperty = axesProperty.Find(property => property.FindPropertyRelative("m_Name").GetValue<string>() == axis);

					if (currentAxisProperty == null)
					{
						axesProperty.arraySize += 1;
						currentAxisProperty = axesProperty.Last();
						currentAxisProperty.SetValue(axis, "m_Name");
						currentAxisProperty.SetValue("", "descriptiveName");
						currentAxisProperty.SetValue("", "descriptiveNegativeName");
						currentAxisProperty.SetValue("", "negativeButton");
						currentAxisProperty.SetValue("", "positiveButton");
						currentAxisProperty.SetValue("", "altNegativeButton");
						currentAxisProperty.SetValue("", "altPositiveButton");
						currentAxisProperty.SetValue(0f, "gravity");
						currentAxisProperty.SetValue(0.2f, "dead");
						currentAxisProperty.SetValue(1f, "sensitivity");
						currentAxisProperty.SetValue(false, "snap");
						currentAxisProperty.SetValue(joystickAxis == InputManager.JoystickAxes.LeftStickY || joystickAxis == InputManager.JoystickAxes.RightStickY, "invert");
						currentAxisProperty.SetValue(2, "type");
						currentAxisProperty.SetValue((joystickAxis == InputManager.JoystickAxes.LeftTrigger || joystickAxis == InputManager.JoystickAxes.RightTrigger) ? 2 : (int)joystickAxis, "axis");
						currentAxisProperty.SetValue((int)joystick, "joyNum");
					}
					else
						break;
				}
			}

			inputManagerSerialized.ApplyModifiedProperties();
#endif
		}

		public static string[] GetKeyboardAxes()
		{
			List<string> axes = new List<string>();

#if UNITY_EDITOR
			UnityEditor.SerializedObject inputManagerSerialized = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset"));
			UnityEditor.SerializedProperty inputManagerAxesProperty = inputManagerSerialized.FindProperty("m_Axes");

			for (int i = 0; i < inputManagerAxesProperty.arraySize; i++)
			{
				string axisName = inputManagerAxesProperty.GetArrayElementAtIndex(i).FindPropertyRelative("m_Name").GetValue<string>();

				if (!axisName.StartsWith("Any") && !axisName.StartsWith("Joystick") && !axes.Contains(axisName))
					axes.Add(axisName);
			}
#endif

			return axes.ToArray();
		}

		public static InputManager.Joysticks KeyToJoystick(KeyCode key)
		{
			string keyName = key.ToString();
			int joystickIndex = int.Parse(char.IsNumber(keyName[8]) ? char.IsNumber(keyName[9]) ? keyName.GetRange(8, 2) : keyName.GetRange(8, 1) : "0");

			return (InputManager.Joysticks)joystickIndex;
		}

		public static InputManager.JoystickButtons KeyToJoystickButton(KeyCode key)
		{
			string keyName = key.ToString();
			int joystickIndex = int.Parse(char.IsNumber(keyName[keyName.Length - 2]) ? keyName.GetRange(keyName.Length - 2) : keyName.GetRange(keyName.Length - 1));

			return (InputManager.JoystickButtons)joystickIndex;
		}

		public static KeyCode JoystickInputToKey(InputManager.Joysticks joystick, InputManager.JoystickButtons button)
		{
			return GetJoystickKeys(joystick)[(int)button];
		}

		public static InputManager.Joysticks AxisToJoystick(string axis)
		{
			int length = axis.StartsWith("Any") ? 3 : char.IsNumber(axis[9]) ? 9 : 8;
			string joystickName = axis.Substring(0, length);

			return (InputManager.Joysticks)System.Enum.Parse(typeof(InputManager.Joysticks), joystickName);
		}

		public static InputManager.JoystickAxes AxisToJoystickAxis(string axis)
		{
			int startIndex = axis.StartsWith("Any") ? 3 : char.IsNumber(axis[9]) ? 9 : 8;
			string axisName = axis.Substring(startIndex);

			return (InputManager.JoystickAxes)System.Enum.Parse(typeof(InputManager.JoystickAxes), axisName);
		}

		public static string JoystickInputToAxis(InputManager.Joysticks joystick, InputManager.JoystickAxes axis)
		{
			return joystick.ToString() + axis;
		}

		public static KeyCode[] GetPressedKeys(KeyCode[] keys)
		{
			List<KeyCode> pressed = new List<KeyCode>();

			for (int i = 0; i < keys.Length; i++)
			{
				KeyCode key = keys[i];

				if (UnityEngine.Input.GetKey(key))
					pressed.Add(key);
			}

			return pressed.ToArray();
		}

		public static InputManager.Joysticks[] GetPressedJoysticks()
		{
			List<InputManager.Joysticks> joysticks = new List<InputManager.Joysticks>();
			KeyCode[] joystickKeys = GetJoystickKeys();

			for (int i = 0; i < joystickKeys.Length; i++)
			{
				KeyCode joystickKey = joystickKeys[i];

				if (!UnityEngine.Input.GetKey(joystickKey))
					continue;

				InputManager.Joysticks joystick = KeyToJoystick(joystickKey);

				if (!joysticks.Contains(joystick))
					joysticks.Add(joystick);
			}

			return joysticks.ToArray();
		}

		public static InputManager.JoystickButtons[] GetPressedJoystickButtons()
		{
			List<InputManager.JoystickButtons> joystickButtons = new List<InputManager.JoystickButtons>();
			KeyCode[] joystickKeys = GetJoystickKeys();

			for (int i = 0; i < joystickKeys.Length; i++)
			{
				KeyCode joystickKey = joystickKeys[i];

				if (!UnityEngine.Input.GetKey(joystickKey))
					continue;

				InputManager.JoystickButtons joystickButton = KeyToJoystickButton(joystickKey);

				if (!joystickButtons.Contains(joystickButton))
					joystickButtons.Add(joystickButton);
			}

			return joystickButtons.ToArray();
		}

		public static KeyCode[] GetPressedKeys() { return GetPressedKeys(allKeys); }
		public static KeyCode[] GetAllKeys() { return allKeys; }
		public static KeyCode[] GetKeyboardKeys() { return keypadKeys; }
		public static KeyCode[] GetJoystickKeys() { return joystickKeys; }
		public static KeyCode[] GetJoystickKeys(InputManager.Joysticks joystick) { return joystickKeysDict[(int)joystick]; }
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