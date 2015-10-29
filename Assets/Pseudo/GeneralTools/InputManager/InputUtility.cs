using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal
{
	public static class InputUtility
	{
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

		public static void SetInputManager()
		{
#if UNITY_EDITOR
			UnityEditor.SerializedObject inputManagerSerialized = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset"));
			UnityEditor.SerializedProperty axesProperty = inputManagerSerialized.FindProperty("m_Axes");

			foreach (Joysticks joystick in System.Enum.GetValues(typeof(Joysticks)))
			{
				foreach (JoystickAxes joystickAxis in System.Enum.GetValues(typeof(JoystickAxes)))
				{
					string axis = joystick.ToString() + joystickAxis;

					UnityEditor.SerializedProperty currentAxisProperty = axesProperty.Find(property => property.FindPropertyRelative("m_Name").GetValue<string>() == axis);

					if (currentAxisProperty == null)
					{
						axesProperty.arraySize += 1;

						currentAxisProperty = axesProperty.Last();
						currentAxisProperty.FindPropertyRelative("m_Name").SetValue(axis);
						currentAxisProperty.FindPropertyRelative("dead").SetValue(0.19F);
						currentAxisProperty.FindPropertyRelative("sensitivity").SetValue(1F);
						currentAxisProperty.FindPropertyRelative("invert").SetValue(joystickAxis == JoystickAxes.LeftStickY || joystickAxis == JoystickAxes.RightStickY);
						currentAxisProperty.FindPropertyRelative("type").SetValue(2);
						currentAxisProperty.FindPropertyRelative("axis").SetValue((joystickAxis == JoystickAxes.LeftTrigger || joystickAxis == JoystickAxes.RightTrigger) ? 2 : (int)joystickAxis);
						currentAxisProperty.FindPropertyRelative("joyNum").SetValue((int)joystick);
					}
				}
			}

			inputManagerSerialized.ApplyModifiedProperties();
#endif
		}
	}
}