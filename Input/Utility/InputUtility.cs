using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Editor.Internal;

namespace Pseudo.Input.Internal
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
			var inputManagerSerialized = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset"));
			var axesProperty = inputManagerSerialized.FindProperty("m_Axes");

			var joysticks = (Joysticks[])System.Enum.GetValues(typeof(Joysticks));

			for (int i = 0; i < joysticks.Length; i++)
			{
				var joystick = joysticks[i];
				var joystickAxes = (JoystickAxes[])System.Enum.GetValues(typeof(JoystickAxes));

				for (int j = 0; j < joystickAxes.Length; j++)
				{
					var joystickAxis = joystickAxes[j];
					var axis = joystick.ToString() + joystickAxis;
					var currentAxisProperty = axesProperty.Find(property => property.FindPropertyRelative("m_Name").GetValue<string>() == axis);

					if (currentAxisProperty == null)
					{
						axesProperty.arraySize += 1;
						currentAxisProperty = axesProperty.Last();
						currentAxisProperty.SetValue("m_Name", axis);
						currentAxisProperty.SetValue("descriptiveName", "");
						currentAxisProperty.SetValue("descriptiveNegativeName", "");
						currentAxisProperty.SetValue("negativeButton", "");
						currentAxisProperty.SetValue("positiveButton", "");
						currentAxisProperty.SetValue("altNegativeButton", "");
						currentAxisProperty.SetValue("altPositiveButton", "");
						currentAxisProperty.SetValue("gravity", 0f);
						currentAxisProperty.SetValue("dead", 0.2f);
						currentAxisProperty.SetValue("sensitivity", 1f);
						currentAxisProperty.SetValue("snap", false);
						currentAxisProperty.SetValue("invert", joystickAxis == JoystickAxes.LeftStickY || joystickAxis == JoystickAxes.RightStickY);
						currentAxisProperty.SetValue("type", value: 2);
						currentAxisProperty.SetValue("axis", value: (joystickAxis == JoystickAxes.LeftTrigger || joystickAxis == JoystickAxes.RightTrigger) ? 2 : (int)joystickAxis);
						currentAxisProperty.SetValue("joyNum", value: (int)joystick);
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
			var axes = new List<string>();

#if UNITY_EDITOR
			var inputManagerSerialized = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset"));
			var inputManagerAxesProperty = inputManagerSerialized.FindProperty("m_Axes");

			for (int i = 0; i < inputManagerAxesProperty.arraySize; i++)
			{
				string axisName = inputManagerAxesProperty.GetArrayElementAtIndex(i).FindPropertyRelative("m_Name").GetValue<string>();

				if (!axisName.StartsWith("Any") && !axisName.StartsWith("Joystick") && !axes.Contains(axisName))
					axes.Add(axisName);
			}
#endif

			return axes.ToArray();
		}

		public static Joysticks KeyToJoystick(KeyCode key)
		{
			return (Joysticks)((key - KeyCode.JoystickButton0) / 20);
		}

		public static JoystickButtons KeyToJoystickButton(KeyCode key)
		{
			return (JoystickButtons)((key - KeyCode.JoystickButton0) % 20);
		}

		public static KeyCode JoystickInputToKey(Joysticks joystick, JoystickButtons button)
		{
			return KeyCode.JoystickButton0 + ((int)joystick * 20 + (int)button);
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
			var pressed = new List<KeyCode>();

			for (int i = 0; i < keys.Length; i++)
			{
				var key = keys[i];

				if (UnityEngine.Input.GetKey(key))
					pressed.Add(key);
			}

			return pressed.ToArray();
		}

		public static Joysticks[] GetPressedJoysticks()
		{
			var joysticks = new List<Joysticks>();
			var joystickKeys = GetJoystickKeys();

			for (int i = 0; i < joystickKeys.Length; i++)
			{
				var joystickKey = joystickKeys[i];

				if (!UnityEngine.Input.GetKey(joystickKey))
					continue;

				var joystick = KeyToJoystick(joystickKey);

				if (!joysticks.Contains(joystick))
					joysticks.Add(joystick);
			}

			return joysticks.ToArray();
		}

		public static JoystickButtons[] GetPressedJoystickButtons()
		{
			var joystickButtons = new List<JoystickButtons>();
			var joystickKeys = GetJoystickKeys();

			for (int i = 0; i < joystickKeys.Length; i++)
			{
				var joystickKey = joystickKeys[i];

				if (!UnityEngine.Input.GetKey(joystickKey))
					continue;

				var joystickButton = KeyToJoystickButton(joystickKey);

				if (!joystickButtons.Contains(joystickButton))
					joystickButtons.Add(joystickButton);
			}

			return joystickButtons.ToArray();
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

		public static bool GetKeysDown(params KeyCode[] keycodes)
		{
			for (int i = 0; i < keycodes.Length; i++)
			{
				if (!UnityEngine.Input.GetKeyDown(keycodes[i]))
					return false;
			}
			return true;
		}
	}
}