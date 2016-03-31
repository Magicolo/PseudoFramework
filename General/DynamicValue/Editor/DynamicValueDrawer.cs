using UnityEngine;
using System.Collections;
using UnityEditor;
using Pseudo.Editor.Internal;
using Pseudo.Internal;
using Pseudo;
using System.Collections.Generic;
using System.ComponentModel;

namespace Pseudo.Internal
{
	[CustomPropertyDrawer(typeof(DynamicValue))]
	public class DynamicValueDrawer : PPropertyDrawer
	{
		DynamicValue dynamicValue;
		float height;

		readonly Dictionary<DynamicValue, float> dynamicValueToHeight = new Dictionary<DynamicValue, float>();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			currentPosition.height = 16f;
			EditorGUI.PropertyField(currentPosition, property, label, false);
			currentPosition.y += 16f;

			if (property.isExpanded)
			{
				EditorGUI.indentLevel++;

				ShowValueType();
				ShowValue();

				EditorGUI.indentLevel--;
			}

			dynamicValueToHeight[dynamicValue] = currentPosition.y - initPosition.y;

			if (height != currentPosition.y - initPosition.y)
				Event.current.Use();

			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			dynamicValue = property.GetValue<DynamicValue>();
			height = GetHeight();

			if (height == 0f)
				return 18f;
			else
				return height;
		}

		void ShowValueType()
		{
			EditorGUI.BeginProperty(currentPosition, GUIContent.none, currentProperty.FindPropertyRelative("valueType"));
			EditorGUI.BeginChangeCheck();

			dynamicValue.Type = (DynamicValue.ValueTypes)EditorGUI.EnumPopup(new Rect(currentPosition) { width = currentPosition.width - 48f }, GUIContent.none, dynamicValue.Type);

			if (EditorGUI.EndChangeCheck())
				serializedObject.Update();
			EditorGUI.EndProperty();

			EditorGUI.BeginChangeCheck();

			var isArrayProperty = currentProperty.FindPropertyRelative("isArray");
			EditorGUI.PropertyField(new Rect(currentPosition) { x = currentPosition.width - 27f - EditorGUI.indentLevel * 16f, width = 40f + EditorGUI.indentLevel * 16f }, isArrayProperty);
			currentPosition.y += 16f;

			if (EditorGUI.EndChangeCheck())
				serializedObject.ApplyModifiedProperties();
		}

		void ShowValue()
		{
			if (dynamicValue.Type == DynamicValue.ValueTypes.Object)
			{
				var objectProperty = currentProperty.FindPropertyRelative("objectValue");

				if (dynamicValue.IsArray)
					PropertyField(objectProperty);
				else
				{
					objectProperty.arraySize = 1;
					PropertyField(objectProperty.GetArrayElementAtIndex(0));
				}
			}
			else
			{
				EditorGUI.BeginProperty(currentPosition, "Value".ToGUIContent(), currentProperty.FindPropertyRelative("data"));

				float valueHeight;
				dynamicValue.Value = ObjectField(currentPosition, dynamicValue.Value, "Value".ToGUIContent(), out valueHeight);
				currentPosition.y += valueHeight;

				EditorGUI.EndProperty();
			}
		}

		float GetHeight()
		{
			float height;

			if (!dynamicValueToHeight.TryGetValue(dynamicValue, out height))
			{
				height = 0f;
				dynamicValueToHeight[dynamicValue] = height;
			}

			return height;
		}

		DynamicValue.ValueTypes GetValueType(SerializedProperty typeProperty)
		{
			return (DynamicValue.ValueTypes)System.Enum.GetValues(typeof(DynamicValue.ValueTypes)).GetValue(typeProperty.GetValue<int>());
		}
	}
}