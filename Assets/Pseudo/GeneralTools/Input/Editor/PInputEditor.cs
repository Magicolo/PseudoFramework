using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal
{
	[CustomEditor(typeof(PInput)), CanEditMultipleObjects]
	public class PInputEditor : CustomEditorBase
	{
		PInput pInput;

		SerializedProperty keyboardInfosProperty;
		PKeyboardInfo currentKeyboardInfo;
		SerializedProperty currentKeyboardInfoProperty;
		SerializedProperty keyboardButtonsProperty;
		SerializedProperty currentKeyboardButtonProperty;
		SerializedProperty keyboardAxesProperty;
		SerializedProperty currentKeyboardAxisProperty;

		SerializedProperty joystickInfosProperty;
		PJoystickInfo currentJoystickInfo;
		SerializedProperty currentJoystickInfoProperty;
		SerializedProperty joystickButtonsProperty;
		SerializedProperty currentJoystickButtonProperty;
		SerializedProperty joystickAxesProperty;
		SerializedProperty currentJoystickAxisProperty;

		KeyCode[] keyboardKeys;
		string[] keyboardKeyNames;
		string[] keyboardAxes;

		public override void OnEnable()
		{
			base.OnEnable();

			pInput = (PInput)target;
			keyboardKeys = PInput.GetNonJoystickKeys();
			keyboardKeyNames = keyboardKeyNames ?? keyboardKeys.ToStringArray();
			keyboardAxes = PInputUtility.GetKeyboardAxes();
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
			keyboardInfosProperty = serializedObject.FindProperty("keyboardInfos");

			if (AddFoldOut(keyboardInfosProperty, "Keyboards".ToGUIContent()))
			{
				PKeyboardInfo[] keyboardInfos = pInput.GetKeyboardInfos();
				PKeyboardInfo keyboardInfo = keyboardInfos.Last();

				keyboardInfo.SetUniqueName(keyboardInfos, "default", "");

				serializedObject.Update();
			}

			if (keyboardInfosProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < keyboardInfosProperty.arraySize; i++)
				{
					currentKeyboardInfo = pInput.GetKeyboardInfos()[i];
					currentKeyboardInfoProperty = keyboardInfosProperty.GetArrayElementAtIndex(i);

					BeginBox();

					if (DeleteFoldOut(keyboardInfosProperty, i, currentKeyboardInfo.Name.ToGUIContent(), CustomEditorStyles.BoldFoldout))
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
			keyboardButtonsProperty = currentKeyboardInfoProperty.FindPropertyRelative("buttons");
			keyboardAxesProperty = currentKeyboardInfoProperty.FindPropertyRelative("axes");

			if (currentKeyboardInfoProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				UniqueNameField(currentKeyboardInfo, pInput.GetKeyboardInfos());

				Separator();

				ShowKeyboardButtons();
				ShowKeyboardAxes();

				EditorGUI.indentLevel -= 1;
			}
		}

		void ShowKeyboardButtons()
		{
			BeginBox();

			if (AddFoldOut(keyboardButtonsProperty))
				keyboardButtonsProperty.Last().FindPropertyRelative("name").SetValue("default");

			if (keyboardButtonsProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < keyboardButtonsProperty.arraySize; i++)
				{
					currentKeyboardButtonProperty = keyboardButtonsProperty.GetArrayElementAtIndex(i);

					EditorGUILayout.BeginHorizontal();
					EditorGUI.BeginChangeCheck();

					PKeyboardButton currentKeyboardKey = currentKeyboardInfo.GetButtons()[i];
					int index = System.Array.IndexOf(keyboardKeys, currentKeyboardKey.Key);

					index = EditorGUILayout.Popup(index, keyboardKeyNames);

					if (EditorGUI.EndChangeCheck())
					{
						currentKeyboardKey.Key = keyboardKeys[Mathf.Clamp(index, 0, Mathf.Max(keyboardKeys.Length - 1, 0))];
						serializedObject.Update();
					}

					int indent = EditorGUI.indentLevel;
					EditorGUI.indentLevel = 0;

					EditorGUILayout.PropertyField(currentKeyboardButtonProperty.FindPropertyRelative("name"), GUIContent.none);

					EditorGUI.indentLevel = indent;

					if (DeleteButton(keyboardButtonsProperty, i, ButtonAlign.Right))
					{
						break;
					}

					EditorGUILayout.EndHorizontal();

					Reorderable(keyboardButtonsProperty, i, true);
				}

				Separator();
				EditorGUI.indentLevel -= 1;

			}

			EndBox();
		}

		void ShowKeyboardAxes()
		{
			BeginBox();

			if (AddFoldOut(keyboardAxesProperty))
				keyboardAxesProperty.Last().FindPropertyRelative("name").SetValue("default");

			if (keyboardAxesProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < keyboardAxesProperty.arraySize; i++)
				{
					currentKeyboardAxisProperty = keyboardAxesProperty.GetArrayElementAtIndex(i);

					EditorGUILayout.BeginHorizontal();

					SerializedProperty axisProperty = currentKeyboardAxisProperty.FindPropertyRelative("axis");
					int index = System.Array.IndexOf(keyboardAxes, axisProperty.GetValue<string>());

					index = EditorGUILayout.Popup(index, keyboardAxes);

					axisProperty.SetValue(keyboardAxes[Mathf.Clamp(index, 0, Mathf.Max(keyboardAxes.Length - 1, 0))]);

					int indent = EditorGUI.indentLevel;
					EditorGUI.indentLevel = 0;

					EditorGUILayout.PropertyField(currentKeyboardAxisProperty.FindPropertyRelative("name"), GUIContent.none);

					EditorGUI.indentLevel = indent;

					if (DeleteButton(keyboardAxesProperty, i, ButtonAlign.Right))
					{
						break;
					}

					EditorGUILayout.EndHorizontal();

					Reorderable(keyboardAxesProperty, i, true);
				}

				Separator();
				EditorGUI.indentLevel -= 1;
			}

			EndBox();
		}

		void ShowJoystickInfos()
		{
			joystickInfosProperty = serializedObject.FindProperty("joystickInfos");

			if (AddFoldOut(joystickInfosProperty, "Joysticks".ToGUIContent()))
			{
				PJoystickInfo[] joystickInfos = pInput.GetJoystickInfos();
				PJoystickInfo joystickInfo = joystickInfos.Last();

				joystickInfo.SetUniqueName(joystickInfos, "default", "");

				serializedObject.Update();
			}

			if (joystickInfosProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < joystickInfosProperty.arraySize; i++)
				{
					currentJoystickInfo = pInput.GetJoystickInfos()[i];
					currentJoystickInfoProperty = joystickInfosProperty.GetArrayElementAtIndex(i);

					BeginBox();

					string joystickName = currentJoystickInfoProperty.FindPropertyRelative("joystick").GetValue<Joysticks>().ToString();

					if (DeleteFoldOut(joystickInfosProperty, i, string.Format("{0} ({1})", currentJoystickInfo.Name, joystickName).ToGUIContent(), CustomEditorStyles.BoldFoldout))
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
			joystickButtonsProperty = currentJoystickInfoProperty.FindPropertyRelative("buttons");
			joystickAxesProperty = currentJoystickInfoProperty.FindPropertyRelative("axes");

			if (currentJoystickInfoProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				UniqueNameField(currentJoystickInfo, pInput.GetJoystickInfos());
				EditorGUILayout.PropertyField(currentJoystickInfoProperty.FindPropertyRelative("joystick"), "Input".ToGUIContent());

				Separator();

				ShowJoystickButtons();
				ShowJoystickAxes();

				EditorGUI.indentLevel -= 1;
			}
		}

		void ShowJoystickButtons()
		{
			BeginBox();

			if (AddFoldOut(joystickButtonsProperty))
			{
				joystickButtonsProperty.Last().FindPropertyRelative("name").SetValue("default");

				PJoystickButton button = currentJoystickInfo.GetButtons().Last();

				button.Joystick = currentJoystickInfo.Joystick;
				button.Button = JoystickButtons.Cross_A;

				serializedObject.Update();
			}

			if (joystickButtonsProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < joystickButtonsProperty.arraySize; i++)
				{
					currentJoystickButtonProperty = joystickButtonsProperty.GetArrayElementAtIndex(i);

					EditorGUILayout.BeginHorizontal();

					EditorGUILayout.PropertyField(currentJoystickButtonProperty.FindPropertyRelative("button"), GUIContent.none);

					int indent = EditorGUI.indentLevel;
					EditorGUI.indentLevel = 0;

					EditorGUILayout.PropertyField(currentJoystickButtonProperty.FindPropertyRelative("name"), GUIContent.none);

					EditorGUI.indentLevel = indent;

					if (DeleteButton(joystickButtonsProperty, i, ButtonAlign.Right))
					{
						break;
					}

					EditorGUILayout.EndHorizontal();

					Reorderable(joystickButtonsProperty, i, true);
				}

				Separator();
				EditorGUI.indentLevel -= 1;

			}

			EndBox();
		}

		void ShowJoystickAxes()
		{
			BeginBox();

			if (AddFoldOut(joystickAxesProperty))
			{
				joystickAxesProperty.Last().FindPropertyRelative("name").SetValue("default");

				PJoystickAxis axis = currentJoystickInfo.GetAxes().Last();

				axis.Joystick = currentJoystickInfo.Joystick;
				axis.AxisInput = JoystickAxes.LeftStickX;

				serializedObject.Update();
			}

			if (joystickAxesProperty.isExpanded)
			{
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < joystickAxesProperty.arraySize; i++)
				{
					currentJoystickAxisProperty = joystickAxesProperty.GetArrayElementAtIndex(i);

					EditorGUILayout.BeginHorizontal();

					EditorGUILayout.PropertyField(currentJoystickAxisProperty.FindPropertyRelative("axisInput"), GUIContent.none);

					int indent = EditorGUI.indentLevel;
					EditorGUI.indentLevel = 0;

					EditorGUILayout.PropertyField(currentJoystickAxisProperty.FindPropertyRelative("name"), GUIContent.none);

					EditorGUI.indentLevel = indent;

					if (DeleteButton(joystickAxesProperty, i, ButtonAlign.Right))
						break;

					EditorGUILayout.EndHorizontal();

					Reorderable(joystickAxesProperty, i, true);
				}

				Separator();
				EditorGUI.indentLevel -= 1;
			}

			EndBox();
		}
	}
}
