using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Pseudo.Internal;
using Pseudo.Reflection;

namespace Pseudo.Editor.Internal
{
	[CustomPropertyDrawer(typeof(ButtonAttribute))]
	public class ButtonDrawer : CustomAttributePropertyDrawerBase
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			if (property.propertyType == SerializedPropertyType.Boolean)
			{
				string buttonLabel = string.IsNullOrEmpty(((ButtonAttribute)attribute).label) ? label.text : ((ButtonAttribute)attribute).label;
				string buttonPressedMethodName = string.IsNullOrEmpty(((ButtonAttribute)attribute).methodName) ? label.text.Replace(" ", "").Replace("_", "").Capitalized() : ((ButtonAttribute)attribute).methodName;
				string buttonIndexVariableName = ((ButtonAttribute)attribute).indexVariableName;
				GUIStyle buttonStyle = ((ButtonAttribute)attribute).style;
				currentPosition = EditorGUI.IndentedRect(currentPosition);

				if (noFieldLabel) buttonLabel = "";

				bool pressed;
				if (buttonStyle != null)
					pressed = GUI.Button(currentPosition, buttonLabel, buttonStyle);
				else
					pressed = GUI.Button(currentPosition, buttonLabel);

				if (pressed)
				{
					if (!string.IsNullOrEmpty(buttonIndexVariableName))
						property.serializedObject.FindProperty(buttonIndexVariableName).intValue = index;

					if (!string.IsNullOrEmpty(buttonPressedMethodName))
					{
						MethodInfo method = property.serializedObject.targetObject.GetType().GetMethod(buttonPressedMethodName, ReflectionUtility.AllFlags);

						if (method != null)
							method.Invoke(property.serializedObject.targetObject, null);
					}

					EditorUtility.SetDirty(property.serializedObject.targetObject);
				}
				property.boolValue = pressed;
			}
			else
				EditorGUI.LabelField(currentPosition, "Button variable must be of type boolean.");

			End();
		}
	}
}
