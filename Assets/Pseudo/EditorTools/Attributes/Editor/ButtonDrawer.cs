using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Pseudo.Internal.Editor
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
				_currentPosition = AttributeUtility.BeginIndentation(_currentPosition);

				if (noFieldLabel) buttonLabel = "";

				bool pressed;
				if (buttonStyle != null)
					pressed = GUI.Button(_currentPosition, buttonLabel, buttonStyle);
				else
					pressed = GUI.Button(_currentPosition, buttonLabel);

				AttributeUtility.EndIndentation();

				if (pressed)
				{
					if (!string.IsNullOrEmpty(buttonIndexVariableName))
						property.serializedObject.FindProperty(buttonIndexVariableName).intValue = _index;

					if (!string.IsNullOrEmpty(buttonPressedMethodName))
					{
						MethodInfo method = property.serializedObject.targetObject.GetType().GetMethod(buttonPressedMethodName, ObjectExtensions.AllFlags);

						if (method != null)
							method.Invoke(property.serializedObject.targetObject, null);
					}

					EditorUtility.SetDirty(property.serializedObject.targetObject);
				}
				property.boolValue = pressed;
			}
			else
				EditorGUI.LabelField(_currentPosition, "Button variable must be of type boolean.");

			End();
		}
	}
}
