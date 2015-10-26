using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Input System")]
	public class InputSystem : MonoBehaviourExtended
	{
		[SerializeField]
		List<KeyboardInfo> _keyboardInfos = new List<KeyboardInfo>();
		[SerializeField]
		List<JoystickInfo> _joystickInfos = new List<JoystickInfo>();

		Dictionary<string, KeyboardInfo> _nameKeyboardInfosDict;
		Dictionary<string, KeyboardInfo> NameKeyboardInfosDict
		{
			get
			{
				if (_nameKeyboardInfosDict == null)
					BuildKeyboardInfosDict();

				return _nameKeyboardInfosDict;
			}
		}

		Dictionary<string, JoystickInfo> _nameJoystickInfosDict;
		Dictionary<string, JoystickInfo> NameJoystickInfosDict
		{
			get
			{
				if (_nameJoystickInfosDict == null)
					BuildJoystickInfosDict();

				return _nameJoystickInfosDict;
			}
		}

		static KeyCode[] _allKeys;
		static KeyCode[] AllKeys
		{
			get
			{
				if (_allKeys == null)
					SortKeys();

				return _allKeys;
			}
		}

		static KeyCode[] _keyboardKeys;
		static KeyCode[] KeyboardKeys
		{
			get
			{
				if (_keyboardKeys == null)
					SortKeys();

				return _keyboardKeys;
			}
		}

		static KeyCode[] _joystickKeys;
		static KeyCode[] JoystickKeys
		{
			get
			{
				if (_joystickKeys == null)
					SortKeys();

				return _joystickKeys;
			}
		}

		static Dictionary<int, KeyCode[]> _joystickKeysDict;
		static Dictionary<int, KeyCode[]> JoystickKeysDict
		{
			get
			{
				if (_joystickKeysDict == null)
					SortKeys();

				return _joystickKeysDict;
			}
		}

		static KeyCode[] _nonjoystickKeys;
		static KeyCode[] NonJoystickKeys
		{
			get
			{
				if (_nonjoystickKeys == null)
					SortKeys();

				return _nonjoystickKeys;
			}
		}

		static KeyCode[] _mouseKeys;
		static KeyCode[] MouseKeys
		{
			get
			{
				if (_mouseKeys == null)
					SortKeys();

				return _mouseKeys;
			}
		}

		static KeyCode[] _letterKeys;
		static KeyCode[] LetterKeys
		{
			get
			{
				if (_letterKeys == null)
					SortKeys();

				return _letterKeys;
			}
		}

		static KeyCode[] _functionKeys;
		static KeyCode[] FunctionKeys
		{
			get
			{
				if (_functionKeys == null)
					SortKeys();

				return _functionKeys;
			}
		}

		static KeyCode[] _numberKeys;
		static KeyCode[] NumberKeys
		{
			get
			{
				if (_numberKeys == null)
					SortKeys();

				return _numberKeys;
			}
		}

		static KeyCode[] _keypadKeys;
		static KeyCode[] KeypadKeys
		{
			get
			{
				if (_keypadKeys == null)
					SortKeys();

				return _keypadKeys;
			}
		}

		static KeyCode[] _arrowKeys;
		static KeyCode[] ArrowKeys
		{
			get
			{
				if (_arrowKeys == null)
					SortKeys();

				return _arrowKeys;
			}
		}

		static KeyCode[] _modifierKeys;
		static KeyCode[] ModifierKeys
		{
			get
			{
				if (_modifierKeys == null)
					SortKeys();

				return _modifierKeys;
			}
		}

		void Awake()
		{
			InitializeKeyboards();
			InitializeJoysticks();
		}

		void Update()
		{
			UpdateInput();
		}

		void UpdateInput()
		{
			for (int i = 0; i < _keyboardInfos.Count; i++)
				_keyboardInfos[i].UpdateInput();

			for (int i = 0; i < _joystickInfos.Count; i++)
				_joystickInfos[i].UpdateInput();
		}

		void Reset()
		{
			this.SetExecutionOrder(-13);
			InputSystemUtility.SetInputManager();
		}

		public void AddKeyboardInfo(KeyboardInfo info)
		{
			info.SetListeners();
			NameKeyboardInfosDict[info.Name] = info;
			_keyboardInfos.Add(info);
		}

		public void RemoveKeyboardInfo(KeyboardInfo info)
		{
			NameKeyboardInfosDict.Remove(info.Name);
			_keyboardInfos.Remove(info);
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
			return _keyboardInfos.ToArray();
		}

		public void AddJoystickInfo(JoystickInfo info)
		{
			info.SetListeners();
			NameJoystickInfosDict[info.Name] = info;
			_joystickInfos.Add(info);
		}

		public void RemoveJoystickInfo(JoystickInfo info)
		{
			NameJoystickInfosDict.Remove(info.Name);
			_joystickInfos.Remove(info);
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
			return _joystickInfos.ToArray();
		}

		public void SimulateButtonInput(ControllerInfo info, string inputName, ButtonStates state)
		{
			SendInput(info, inputName, state);
		}

		public void SimulateAxisInput(ControllerInfo info, string inputName, float value)
		{
			SendInput(info, inputName, value);
		}

		void SendInput(ControllerInfo info, string inputName, ButtonStates state)
		{
			IInputListener[] listeners = info.GetListeners();

			for (int i = 0; i < listeners.Length; i++)
				listeners[i].OnButtonInput(new ButtonInput(info.Name, inputName, state));
		}

		void SendInput(ControllerInfo info, string inputName, float value)
		{
			IInputListener[] listeners = info.GetListeners();

			for (int i = 0; i < listeners.Length; i++)
				listeners[i].OnAxisInput(new AxisInput(info.Name, inputName, value));
		}

		void InitializeKeyboards()
		{
			for (int i = 0; i < _keyboardInfos.Count; i++)
				_keyboardInfos[i].SetListeners();
		}

		void InitializeJoysticks()
		{
			for (int i = 0; i < _joystickInfos.Count; i++)
				_joystickInfos[i].SetListeners();
		}

		void BuildKeyboardInfosDict()
		{
			_nameKeyboardInfosDict = new Dictionary<string, KeyboardInfo>();

			for (int i = 0; i < _keyboardInfos.Count; i++)
			{
				KeyboardInfo info = _keyboardInfos[i];
				_nameKeyboardInfosDict[info.Name] = info;
			}
		}

		void BuildJoystickInfosDict()
		{
			_nameJoystickInfosDict = new Dictionary<string, JoystickInfo>();

			for (int i = 0; i < _joystickInfos.Count; i++)
			{
				JoystickInfo info = _joystickInfos[i];
				_nameJoystickInfosDict[info.Name] = info;
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
			_allKeys = new KeyCode[321];
			_keyboardKeys = new KeyCode[134];
			_joystickKeys = new KeyCode[180];
			_joystickKeysDict = new Dictionary<int, KeyCode[]>();
			_nonjoystickKeys = new KeyCode[141];
			_mouseKeys = new KeyCode[7];
			_letterKeys = new KeyCode[26];
			_functionKeys = new KeyCode[15];
			_numberKeys = new KeyCode[10];
			_keypadKeys = new KeyCode[17];
			_arrowKeys = new KeyCode[4];
			_modifierKeys = new KeyCode[7];

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

				_allKeys[allCounter++] = key;

				if (!keyName.StartsWith("Joystick") && !keyName.StartsWith("Mouse"))
					_keyboardKeys[keyboardCounter++] = key;

				if (keyName.StartsWith("Joystick"))
				{
					int joystick = (int)KeyToJoystick(key);
					int joystickButton = (int)KeyToJoystickButton(key);

					if (!_joystickKeysDict.ContainsKey(joystick))
						_joystickKeysDict[joystick] = new KeyCode[20];

					_joystickKeysDict[joystick][joystickButton] = key;
					_joystickKeys[joystickCounter++] = key;
				}
				else
					_nonjoystickKeys[nonJoystickCounter++] = key;

				if (keyName.StartsWith("Mouse"))
					_mouseKeys[mouseCounter++] = key;

				if (keyName.Length == 1 && char.IsLetter(keyName[0]))
					_letterKeys[letterCounter++] = key;

				if ((keyName.Length == 2 || keyName.Length == 3) && keyName.StartsWith("F"))
					_functionKeys[functionCounter++] = key;

				if (keyName.StartsWith("Alpha"))
					_numberKeys[alphaCounter++] = key;

				if (keyName.StartsWith("Keypad"))
					_keypadKeys[keypadCounter++] = key;

				if (keyName.EndsWith("Arrow"))
					_arrowKeys[arrowCounter++] = key;

				if (keyName.EndsWith("Shift") || keyName.EndsWith("Alt") || keyName.EndsWith("Control") || keyName.StartsWith("Alt"))
					_modifierKeys[modifierCounter++] = key;
			}
		}
	}
}