using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Input System")]
	public class PInput : PMonoBehaviour
	{
		[SerializeField]
		List<KeyboardInfo> keyboardInfos = new List<KeyboardInfo>();
		[SerializeField]
		List<JoystickInfo> joystickInfos = new List<JoystickInfo>();

		Dictionary<string, KeyboardInfo> nameKeyboardInfosDict;
		Dictionary<string, KeyboardInfo> NameKeyboardInfosDict
		{
			get
			{
				if (nameKeyboardInfosDict == null)
					BuildKeyboardInfosDict();

				return nameKeyboardInfosDict;
			}
		}

		Dictionary<string, JoystickInfo> nameJoystickInfosDict;
		Dictionary<string, JoystickInfo> NameJoystickInfosDict
		{
			get
			{
				if (nameJoystickInfosDict == null)
					BuildJoystickInfosDict();

				return nameJoystickInfosDict;
			}
		}

		static KeyCode[] allKeys;
		static KeyCode[] AllKeys
		{
			get
			{
				if (allKeys == null)
					SortKeys();

				return allKeys;
			}
		}

		static KeyCode[] keyboardKeys;
		static KeyCode[] KeyboardKeys
		{
			get
			{
				if (keyboardKeys == null)
					SortKeys();

				return keyboardKeys;
			}
		}

		static KeyCode[] joystickKeys;
		static KeyCode[] JoystickKeys
		{
			get
			{
				if (joystickKeys == null)
					SortKeys();

				return joystickKeys;
			}
		}

		static Dictionary<int, KeyCode[]> joystickKeysDict;
		static Dictionary<int, KeyCode[]> JoystickKeysDict
		{
			get
			{
				if (joystickKeysDict == null)
					SortKeys();

				return joystickKeysDict;
			}
		}

		static KeyCode[] nonjoystickKeys;
		static KeyCode[] NonJoystickKeys
		{
			get
			{
				if (nonjoystickKeys == null)
					SortKeys();

				return nonjoystickKeys;
			}
		}

		static KeyCode[] mouseKeys;
		static KeyCode[] MouseKeys
		{
			get
			{
				if (mouseKeys == null)
					SortKeys();

				return mouseKeys;
			}
		}

		static KeyCode[] letterKeys;
		static KeyCode[] LetterKeys
		{
			get
			{
				if (letterKeys == null)
					SortKeys();

				return letterKeys;
			}
		}

		static KeyCode[] functionKeys;
		static KeyCode[] FunctionKeys
		{
			get
			{
				if (functionKeys == null)
					SortKeys();

				return functionKeys;
			}
		}

		static KeyCode[] numberKeys;
		static KeyCode[] NumberKeys
		{
			get
			{
				if (numberKeys == null)
					SortKeys();

				return numberKeys;
			}
		}

		static KeyCode[] keypadKeys;
		static KeyCode[] KeypadKeys
		{
			get
			{
				if (keypadKeys == null)
					SortKeys();

				return keypadKeys;
			}
		}

		static KeyCode[] arrowKeys;
		static KeyCode[] ArrowKeys
		{
			get
			{
				if (arrowKeys == null)
					SortKeys();

				return arrowKeys;
			}
		}

		static KeyCode[] modifierKeys;
		static KeyCode[] ModifierKeys
		{
			get
			{
				if (modifierKeys == null)
					SortKeys();

				return modifierKeys;
			}
		}
		
		void Reset()
		{
			this.SetExecutionOrder(-13);
			InputUtility.SetInputManager();
		}

		public void AddKeyboardInfo(KeyboardInfo info)
		{
			NameKeyboardInfosDict[info.Name] = info;
			keyboardInfos.Add(info);
		}

		public void RemoveKeyboardInfo(KeyboardInfo info)
		{
			NameKeyboardInfosDict.Remove(info.Name);
			keyboardInfos.Remove(info);
		}

		public void RemoveKeyboardInfo(string infoName)
		{
			RemoveKeyboardInfo(GetKeyboardInfo(infoName));
		}

		public KeyboardInfo GetKeyboardInfo(string infoName)
		{
			KeyboardInfo keyboardInfo = null;

			try
			{
				keyboardInfo = NameKeyboardInfosDict[infoName];
			}
			catch
			{
				Debug.LogError(string.Format("KeyboardInfo named {0} was not found.", infoName));
			}

			return keyboardInfo;
		}

		public KeyboardInfo[] GetKeyboardInfos()
		{
			return keyboardInfos.ToArray();
		}

		public void AddJoystickInfo(JoystickInfo info)
		{
			NameJoystickInfosDict[info.Name] = info;
			joystickInfos.Add(info);
		}

		public void RemoveJoystickInfo(JoystickInfo info)
		{
			NameJoystickInfosDict.Remove(info.Name);
			joystickInfos.Remove(info);
		}

		public void RemoveJoystickInfo(string infoName)
		{
			RemoveJoystickInfo(GetJoystickInfo(infoName));
		}

		public JoystickInfo GetJoystickInfo(string infoName)
		{
			JoystickInfo joystickInfo = null;

			try
			{
				joystickInfo = NameJoystickInfosDict[infoName];
			}
			catch
			{
				Debug.LogError(string.Format("JoystickInfo named {0} was not found.", infoName));
			}

			return joystickInfo;
		}

		public JoystickInfo[] GetJoystickInfos()
		{
			return joystickInfos.ToArray();
		}
		
		void BuildKeyboardInfosDict()
		{
			nameKeyboardInfosDict = new Dictionary<string, KeyboardInfo>();

			for (int i = 0; i < keyboardInfos.Count; i++)
			{
				KeyboardInfo info = keyboardInfos[i];
				nameKeyboardInfosDict[info.Name] = info;
			}
		}

		void BuildJoystickInfosDict()
		{
			nameJoystickInfosDict = new Dictionary<string, JoystickInfo>();

			for (int i = 0; i < joystickInfos.Count; i++)
			{
				JoystickInfo info = joystickInfos[i];
				nameJoystickInfosDict[info.Name] = info;
			}
		}

		public static bool IsKeyboardKey(KeyCode key)
		{
			string keyName = key.ToString();

			return !keyName.StartsWith("Joystick") && !keyName.StartsWith("Mouse");
		}

		public static bool IsJoystickKey(KeyCode key)
		{
			string keyName = key.ToString();

			return keyName.StartsWith("Joystick");
		}

		public static bool IsMouseKey(KeyCode key)
		{
			string keyName = key.ToString();

			return keyName.StartsWith("Mouse");
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

		public static KeyCode[] GetPressedKeys()
		{
			return GetPressedKeys(AllKeys);
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

		public static KeyCode[] GetAllKeys()
		{
			return (KeyCode[])AllKeys.Clone();
		}

		public static KeyCode[] GetKeyboardKeys()
		{
			return (KeyCode[])KeyboardKeys.Clone();
		}

		public static KeyCode[] GetJoystickKeys()
		{
			return (KeyCode[])JoystickKeys.Clone();
		}

		public static KeyCode[] GetJoystickKeys(Joysticks joystick)
		{
			return (KeyCode[])JoystickKeysDict[(int)joystick].Clone();
		}

		public static KeyCode[] GetNonJoystickKeys()
		{
			return (KeyCode[])NonJoystickKeys.Clone();
		}

		public static KeyCode[] GetMouseKeys()
		{
			return (KeyCode[])MouseKeys.Clone();
		}

		public static KeyCode[] GetLetterKeys()
		{
			return (KeyCode[])LetterKeys.Clone();
		}

		public static KeyCode[] GetFunctionKeys()
		{
			return (KeyCode[])FunctionKeys.Clone();
		}

		public static KeyCode[] GetNumberKeys()
		{
			return (KeyCode[])NumberKeys.Clone();
		}

		public static KeyCode[] GetKeypadKeys()
		{
			return (KeyCode[])KeypadKeys.Clone();
		}

		public static KeyCode[] GetArrowKeys()
		{
			return (KeyCode[])ArrowKeys.Clone();
		}

		public static KeyCode[] GetModifierKeys()
		{
			return (KeyCode[])ModifierKeys.Clone();
		}

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