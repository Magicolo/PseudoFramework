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
		DynamicValueDrawerDummy _dummy;
		SerializedObject _dummySerialized;

		DynamicValue _dynamicValue;
		SerializedProperty _typeProperty;
		SerializedProperty _isArrayProperty;
		SerializedProperty _valueProperty;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			if (_valueProperty != null)
				_valueProperty.SetValue(_dynamicValue.GetValue());

			_currentPosition.height = 16f;
			EditorGUI.PropertyField(_currentPosition, property, label, false);
			_currentPosition.y += 16f;

			if (property.isExpanded)
			{
				EditorGUI.indentLevel++;

				EditorGUI.BeginChangeCheck();

				EditorGUI.PropertyField(new Rect(_currentPosition.x, _currentPosition.y, _currentPosition.width - 48f, _currentPosition.height), _typeProperty, GUIContent.none);
				EditorGUI.PropertyField(new Rect(_currentPosition.width - 27f - EditorGUI.indentLevel * 16f, _currentPosition.y, 40f + EditorGUI.indentLevel * 16f, _currentPosition.height), _isArrayProperty);
				_currentPosition.y += 16f;

				if (EditorGUI.EndChangeCheck())
				{
					_valueProperty = GetValueProperty(_typeProperty.GetValue<DynamicValue.ValueTypes>(), _isArrayProperty.GetValue<bool>());
					_dynamicValue.SetValue(_valueProperty == null ? null : _valueProperty.GetValue());
				}

				if (_valueProperty != null)
				{
					EditorGUI.BeginChangeCheck();

					EditorGUI.PropertyField(_currentPosition, _valueProperty, new GUIContent("Value"), true);

					if (EditorGUI.EndChangeCheck())
					{
						_dummySerialized.ApplyModifiedProperties();
						_valueProperty = GetValueProperty(_typeProperty.GetValue<DynamicValue.ValueTypes>(), _isArrayProperty.GetValue<bool>());
						_dynamicValue.SetValue(_valueProperty.GetValue());
					}
				}

				EditorGUI.indentLevel--;
			}

			End();
		}

		public override void Initialize(SerializedProperty property, GUIContent label)
		{
			base.Initialize(property, label);

			_dummy = ScriptableObject.CreateInstance<DynamicValueDrawerDummy>();
			_dummy.hideFlags = HideFlags.DontSave;
			_dummySerialized = new SerializedObject(_dummy);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			_dynamicValue = property.GetValue<DynamicValue>();
			_typeProperty = property.FindPropertyRelative("_type");
			_isArrayProperty = property.FindPropertyRelative("_isArray");
			_valueProperty = GetValueProperty(_typeProperty.GetValue<DynamicValue.ValueTypes>(), _isArrayProperty.GetValue<bool>());

			if (property.isExpanded)
				if (_valueProperty == null)
					return 32f;
				else
					return EditorGUI.GetPropertyHeight(_valueProperty, label, true) + 32f;
			else
				return 16f;
		}

		SerializedProperty GetValueProperty(DynamicValue.ValueTypes type, bool isArray)
		{
			string propertyName = type.ToString();
			SerializedProperty valueProperty = null;

			if (type != DynamicValue.ValueTypes.Null)
				valueProperty = _dummySerialized.FindProperty(isArray ? propertyName + "Array" : propertyName);

			return valueProperty;
		}
	}
}