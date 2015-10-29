using UnityEngine;
using System.Collections;
using UnityEditor;
using Pseudo.Internal.Editor;
using Pseudo.Internal;
using Pseudo;
using System.Collections.Generic;

namespace Pseudo.Internal
{
	[CustomPropertyDrawer(typeof(DynamicValue))]
	public class DynamicValueDrawer : CustomPropertyDrawerBase
	{
		DynamicValueDrawerDummy dummy;
		SerializedObject dummySerialized;

		DynamicValue dynamicValue;
		SerializedProperty typeProperty;
		SerializedProperty isArrayProperty;
		SerializedProperty valueProperty;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			if (valueProperty != null)
				valueProperty.SetValue(dynamicValue.GetValue());

			currentPosition.height = 16f;
			EditorGUI.PropertyField(currentPosition, property, label, false);
			currentPosition.y += 16f;

			if (property.isExpanded)
			{
				EditorGUI.indentLevel++;

				EditorGUI.BeginChangeCheck();

				EditorGUI.PropertyField(new Rect(currentPosition.x, currentPosition.y, currentPosition.width - 48f, currentPosition.height), typeProperty, GUIContent.none);
				EditorGUI.PropertyField(new Rect(currentPosition.width - 27f - EditorGUI.indentLevel * 16f, currentPosition.y, 40f + EditorGUI.indentLevel * 16f, currentPosition.height), isArrayProperty);
				currentPosition.y += 16f;

				if (EditorGUI.EndChangeCheck())
				{
					valueProperty = GetValueProperty(typeProperty.GetValue<DynamicValue.ValueTypes>(), isArrayProperty.GetValue<bool>());
					dynamicValue.SetValue(valueProperty == null ? null : valueProperty.GetValue());
				}

				if (valueProperty != null)
				{
					EditorGUI.BeginChangeCheck();

					EditorGUI.PropertyField(currentPosition, valueProperty, new GUIContent("Value"), true);

					if (EditorGUI.EndChangeCheck())
					{
						dummySerialized.ApplyModifiedProperties();
						valueProperty = GetValueProperty(typeProperty.GetValue<DynamicValue.ValueTypes>(), isArrayProperty.GetValue<bool>());
						dynamicValue.SetValue(valueProperty.GetValue());
					}
				}

				EditorGUI.indentLevel--;
			}

			End();
		}

		public override void Initialize(SerializedProperty property, GUIContent label)
		{
			base.Initialize(property, label);

			dummy = ScriptableObject.CreateInstance<DynamicValueDrawerDummy>();
			dummy.hideFlags = HideFlags.DontSave;
			dummySerialized = new SerializedObject(dummy);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			dynamicValue = property.GetValue<DynamicValue>();
			typeProperty = property.FindPropertyRelative("type");
			isArrayProperty = property.FindPropertyRelative("isArray");
			valueProperty = GetValueProperty(typeProperty.GetValue<DynamicValue.ValueTypes>(), isArrayProperty.GetValue<bool>());

			if (property.isExpanded)
				if (valueProperty == null)
					return 32f;
				else
					return EditorGUI.GetPropertyHeight(valueProperty, label, true) + 32f;
			else
				return 16f;
		}

		SerializedProperty GetValueProperty(DynamicValue.ValueTypes type, bool isArray)
		{
			string propertyName = type.ToString();
			SerializedProperty valueProperty = null;

			if (type != DynamicValue.ValueTypes.Null)
				valueProperty = dummySerialized.FindProperty(isArray ? propertyName + "Array" : propertyName);

			return valueProperty;
		}
	}
}