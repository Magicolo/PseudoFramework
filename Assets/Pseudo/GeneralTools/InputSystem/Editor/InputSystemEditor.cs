using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal
{
	[CustomEditor(typeof(InputSystem)), CanEditMultipleObjects]
	public class InputSystemEditor : CustomEditorBase
	{
		InputSystem _inputSystem;

		SerializedProperty _keyboardInfosProperty;
		KeyboardInfo _currentKeyboardInfo;
		SerializedProperty _currentKeyboardInfoProperty;
		SerializedProperty _keyboardButtonsProperty;
		SerializedProperty _currentKeyboardButtonProperty;
		SerializedProperty _keyboardAxesProperty;
		SerializedProperty _currentKeyboardAxisProperty;
		SerializedProperty _keyboardListenersProperty;
		SerializedProperty _currentKeyboardListenerProperty;

		SerializedProperty _joystickInfosProperty;
		JoystickInfo _currentJoystickInfo;
		SerializedProperty _currentJoystickInfoProperty;
		SerializedProperty _joystickButtonsProperty;
		SerializedProperty _currentJoystickButtonProperty;
		SerializedProperty _joystickAxesProperty;
		SerializedProperty _currentJoystickAxisProperty;
		SerializedProperty _joystickListenersProperty;
		SerializedProperty _currentJoystickListenerProperty;

		KeyCode[] _keyboardKeys;
		string[] _keyboardKeyNames;
		string[] _keyboardAxes;

		public override void OnEnable()
		{
			base.OnEnable();

			_inputSystem = (InputSystem)target;
			_keyboardKeys = InputSystem.GetNonJoystickKeys();
			_keyboardKeyNames = _keyboardKeyNames ?? _keyboardKeys.ToStringArray();
			_keyboardAxes = InputSystemUtility.GetKeyboardAxes();
		}

		public override void OnInspectorGUI()
		{
			Begin();

			ShowKeyboardInfos();
			ShowJoystickInfos();

			Separator();

			End();
		}

		void ShowKeyboardInfos()
		{
			_keyboardInfosProperty = serializedObject.FindProperty("_keyboardInfos");

			if (AddFoldOut(_keyboardInfosProperty, "Keyboards".ToGUIContent()))
			{
				KeyboardInfo[] keyboardInfos = _inputSystem.GetKeyboardInfos();
				KeyboardInfo keyboardInfo = keyboardInfos.Last();

				keyboardInfo.SetUniqueName("default", "", keyboardInfos);

				serializedObject.Update();
			}

			if (_keyboardInfosProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < _keyboardInfosProperty.arraySize; i++)
				{
					_currentKeyboardInfo = _inputSystem.GetKeyboardInfos()[i];
					_currentKeyboardInfoProperty = _keyboardInfosProperty.GetArrayElementAtIndex(i);

					BeginBox();

					if (DeleteFoldOut(_keyboardInfosProperty, i, _currentKeyboardInfo.Name.ToGUIContent(), CustomEditorStyles.BoldFoldout))
						break;

					ShowKeyboardInfo();

					EndBox();
				}

				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}

		void ShowKeyboardInfo()
		{
			_keyboardButtonsProperty = _currentKeyboardInfoProperty.FindPropertyRelative("_buttons");
			_keyboardAxesProperty = _currentKeyboardInfoProperty.FindPropertyRelative("_axes");

			if (_currentKeyboardInfoProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				UniqueNameField(_currentKeyboardInfo, _inputSystem.GetKeyboardInfos());

				Separator();

				ShowKeyboardButtons();
				ShowKeyboardAxes();

				Separator();

				ShowKeyboardListeners();

				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}

		void ShowKeyboardButtons()
		{
			BeginBox();

			if (AddFoldOut(_keyboardButtonsProperty))
				_keyboardButtonsProperty.Last().FindPropertyRelative("_name").SetValue("default");

			if (_keyboardButtonsProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < _keyboardButtonsProperty.arraySize; i++)
				{
					_currentKeyboardButtonProperty = _keyboardButtonsProperty.GetArrayElementAtIndex(i);

					EditorGUILayout.BeginHorizontal();
					EditorGUI.BeginChangeCheck();

					KeyboardButton currentKeyboardKey = _currentKeyboardInfo.GetButtons()[i];
					int index = System.Array.IndexOf(_keyboardKeys, currentKeyboardKey.Key);

					index = EditorGUILayout.Popup(index, _keyboardKeyNames);

					if (EditorGUI.EndChangeCheck())
					{
						currentKeyboardKey.Key = _keyboardKeys[Mathf.Clamp(index, 0, Mathf.Max(_keyboardKeys.Length - 1, 0))];
						serializedObject.Update();
					}

					int indent = EditorGUI.indentLevel;
					EditorGUI.indentLevel = 0;

					EditorGUILayout.PropertyField(_currentKeyboardButtonProperty.FindPropertyRelative("_name"), GUIContent.none);

					EditorGUI.indentLevel = indent;

					if (DeleteButton(_keyboardButtonsProperty, i, ButtonAlign.Right))
					{
						break;
					}

					EditorGUILayout.EndHorizontal();

					Reorderable(_keyboardButtonsProperty, i, true);
				}

				Separator();
				EditorGUI.indentLevel -= 1;

			}

			EndBox();
		}

		void ShowKeyboardAxes()
		{
			BeginBox();

			if (AddFoldOut(_keyboardAxesProperty))
				_keyboardAxesProperty.Last().FindPropertyRelative("_name").SetValue("default");

			if (_keyboardAxesProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < _keyboardAxesProperty.arraySize; i++)
				{
					_currentKeyboardAxisProperty = _keyboardAxesProperty.GetArrayElementAtIndex(i);

					EditorGUILayout.BeginHorizontal();

					SerializedProperty axisProperty = _currentKeyboardAxisProperty.FindPropertyRelative("_axis");
					int index = System.Array.IndexOf(_keyboardAxes, axisProperty.GetValue<string>());

					index = EditorGUILayout.Popup(index, _keyboardAxes);

					axisProperty.SetValue(_keyboardAxes[Mathf.Clamp(index, 0, Mathf.Max(_keyboardAxes.Length - 1, 0))]);

					int indent = EditorGUI.indentLevel;
					EditorGUI.indentLevel = 0;

					EditorGUILayout.PropertyField(_currentKeyboardAxisProperty.FindPropertyRelative("_name"), GUIContent.none);

					EditorGUI.indentLevel = indent;

					if (DeleteButton(_keyboardAxesProperty, i, ButtonAlign.Right))
					{
						break;
					}

					EditorGUILayout.EndHorizontal();

					Reorderable(_keyboardAxesProperty, i, true);
				}

				Separator();
				EditorGUI.indentLevel -= 1;
			}

			EndBox();
		}

		void ShowKeyboardListeners()
		{
			_keyboardListenersProperty = _currentKeyboardInfoProperty.FindPropertyRelative("_listenerReferences");

			BeginBox();

			AddFoldOut(_keyboardListenersProperty, "Listeners".ToGUIContent());

			if (_keyboardListenersProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < _keyboardListenersProperty.arraySize; i++)
				{
					_currentKeyboardListenerProperty = _keyboardListenersProperty.GetArrayElementAtIndex(i);

					EditorGUI.BeginDisabledGroup(Application.isPlaying);
					EditorGUILayout.BeginHorizontal();

					ShowAddKeyboardListenerPopup();

					if (DeleteButton(_keyboardListenersProperty, i))
						break;

					EditorGUILayout.EndHorizontal();
					EditorGUI.EndDisabledGroup();

					Reorderable(_keyboardListenersProperty, i, true);
				}

				Separator();
				EditorGUI.indentLevel -= 1;
			}

			EndBox();
		}

		void ShowAddKeyboardListenerPopup()
		{
			List<MonoBehaviour> listeners = new List<MonoBehaviour>();
			List<string> options = new List<string> { " " };
			MonoBehaviour currentListener = _currentKeyboardListenerProperty.GetValue<MonoBehaviour>();

			foreach (MonoBehaviour listener in _inputSystem.GetComponents<MonoBehaviour>())
			{
				if (listener is IInputListener && (currentListener == listener || !_keyboardListenersProperty.Contains(listener)))
				{
					listeners.Add(listener);
					options.Add(InputSystemUtility.FormatListener(listener));
				}
			}

			foreach (MonoBehaviour listener in FindObjectsOfType<MonoBehaviour>())
			{
				if (listener is IInputListener && (currentListener == listener || !_keyboardListenersProperty.Contains(listener)))
				{
					listeners.Add(listener);
					options.Add(InputSystemUtility.FormatListener(listener));
				}
			}

			int index = listeners.IndexOf(_currentKeyboardListenerProperty.GetValue<MonoBehaviour>()) + 1;

			index = EditorGUILayout.Popup(index, options.ToArray()) - 1;

			_currentKeyboardListenerProperty.SetValue(index == -1 ? null : listeners[index]);
		}

		void ShowJoystickInfos()
		{
			_joystickInfosProperty = serializedObject.FindProperty("_joystickInfos");

			if (AddFoldOut(_joystickInfosProperty, "Joysticks".ToGUIContent()))
			{
				JoystickInfo[] joystickInfos = _inputSystem.GetJoystickInfos();
				JoystickInfo joystickInfo = joystickInfos.Last();

				joystickInfo.SetUniqueName("default", "", joystickInfos);

				serializedObject.Update();
			}

			if (_joystickInfosProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < _joystickInfosProperty.arraySize; i++)
				{
					_currentJoystickInfo = _inputSystem.GetJoystickInfos()[i];
					_currentJoystickInfoProperty = _joystickInfosProperty.GetArrayElementAtIndex(i);

					BeginBox();

					string joystickName = _currentJoystickInfoProperty.FindPropertyRelative("_joystick").GetValue<Joysticks>().ToString();

					if (DeleteFoldOut(_joystickInfosProperty, i, string.Format("{0} ({1})", _currentJoystickInfo.Name, joystickName).ToGUIContent(), CustomEditorStyles.BoldFoldout))
					{
						break;
					}

					ShowJoystickInfo();

					EndBox();
				}

				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}

		void ShowJoystickInfo()
		{
			_joystickButtonsProperty = _currentJoystickInfoProperty.FindPropertyRelative("_buttons");
			_joystickAxesProperty = _currentJoystickInfoProperty.FindPropertyRelative("_axes");

			if (_currentJoystickInfoProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				UniqueNameField(_currentJoystickInfo, _inputSystem.GetJoystickInfos());
				EditorGUILayout.PropertyField(_currentJoystickInfoProperty.FindPropertyRelative("_joystick"), "Input".ToGUIContent());

				Separator();

				ShowJoystickButtons();
				ShowJoystickAxes();

				Separator();

				ShowJoystickListeners();

				Separator();

				EditorGUI.indentLevel -= 1;
			}
		}

		void ShowJoystickButtons()
		{
			BeginBox();

			if (AddFoldOut(_joystickButtonsProperty))
			{
				_joystickButtonsProperty.Last().FindPropertyRelative("_name").SetValue("default");

				JoystickButton button = _currentJoystickInfo.GetButtons().Last();

				button.Joystick = _currentJoystickInfo.Joystick;
				button.Button = JoystickButtons.Cross_A;

				serializedObject.Update();
			}

			if (_joystickButtonsProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < _joystickButtonsProperty.arraySize; i++)
				{
					_currentJoystickButtonProperty = _joystickButtonsProperty.GetArrayElementAtIndex(i);

					EditorGUILayout.BeginHorizontal();

					EditorGUILayout.PropertyField(_currentJoystickButtonProperty.FindPropertyRelative("_button"), GUIContent.none);

					int indent = EditorGUI.indentLevel;
					EditorGUI.indentLevel = 0;

					EditorGUILayout.PropertyField(_currentJoystickButtonProperty.FindPropertyRelative("_name"), GUIContent.none);

					EditorGUI.indentLevel = indent;

					if (DeleteButton(_joystickButtonsProperty, i, ButtonAlign.Right))
					{
						break;
					}

					EditorGUILayout.EndHorizontal();

					Reorderable(_joystickButtonsProperty, i, true);
				}

				Separator();
				EditorGUI.indentLevel -= 1;

			}

			EndBox();
		}

		void ShowJoystickAxes()
		{
			BeginBox();

			if (AddFoldOut(_joystickAxesProperty))
			{
				_joystickAxesProperty.Last().FindPropertyRelative("_name").SetValue("default");

				JoystickAxis axis = _currentJoystickInfo.GetAxes().Last();

				axis.Joystick = _currentJoystickInfo.Joystick;
				axis.AxisInput = JoystickAxes.LeftStickX;

				serializedObject.Update();
			}

			if (_joystickAxesProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < _joystickAxesProperty.arraySize; i++)
				{
					_currentJoystickAxisProperty = _joystickAxesProperty.GetArrayElementAtIndex(i);

					EditorGUILayout.BeginHorizontal();

					EditorGUILayout.PropertyField(_currentJoystickAxisProperty.FindPropertyRelative("_axisInput"), GUIContent.none);

					int indent = EditorGUI.indentLevel;
					EditorGUI.indentLevel = 0;

					EditorGUILayout.PropertyField(_currentJoystickAxisProperty.FindPropertyRelative("_name"), GUIContent.none);

					EditorGUI.indentLevel = indent;

					if (DeleteButton(_joystickAxesProperty, i, ButtonAlign.Right))
						break;

					EditorGUILayout.EndHorizontal();

					Reorderable(_joystickAxesProperty, i, true);
				}

				Separator();
				EditorGUI.indentLevel -= 1;
			}

			EndBox();
		}

		void ShowJoystickListeners()
		{
			_joystickListenersProperty = _currentJoystickInfoProperty.FindPropertyRelative("_listenerReferences");

			BeginBox();

			AddFoldOut(_joystickListenersProperty, "Listeners".ToGUIContent());

			if (_joystickListenersProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;


				for (int i = 0; i < _joystickListenersProperty.arraySize; i++)
				{
					_currentJoystickListenerProperty = _joystickListenersProperty.GetArrayElementAtIndex(i);

					EditorGUI.BeginDisabledGroup(Application.isPlaying);
					EditorGUILayout.BeginHorizontal();

					ShowAddJoystickListenerPopup();

					if (DeleteButton(_joystickListenersProperty, i))
						break;

					EditorGUILayout.EndHorizontal();
					EditorGUI.EndDisabledGroup();

					Reorderable(_joystickListenersProperty, i, true);
				}

				Separator();
				EditorGUI.indentLevel -= 1;
			}

			EndBox();
		}

		void ShowAddJoystickListenerPopup()
		{
			List<MonoBehaviour> listeners = new List<MonoBehaviour>();
			List<string> options = new List<string> { " " };
			MonoBehaviour currentListener = _currentJoystickListenerProperty.GetValue<MonoBehaviour>();

			foreach (MonoBehaviour listener in _inputSystem.GetComponents<MonoBehaviour>())
			{
				if (listener is IInputListener && (currentListener == listener || !_joystickListenersProperty.Contains(listener)))
				{
					listeners.Add(listener);
					options.Add(InputSystemUtility.FormatListener(listener));
				}
			}

			foreach (MonoBehaviour listener in FindObjectsOfType<MonoBehaviour>())
			{
				if (listener is IInputListener && (currentListener == listener || !_joystickListenersProperty.Contains(listener)))
				{
					listeners.Add(listener);
					options.Add(InputSystemUtility.FormatListener(listener));
				}
			}

			int index = listeners.IndexOf(_currentJoystickListenerProperty.GetValue<MonoBehaviour>()) + 1;

			index = EditorGUILayout.Popup(index, options.ToArray()) - 1;

			_currentJoystickListenerProperty.SetValue(index == -1 ? null : listeners[index]);
		}
	}
}
