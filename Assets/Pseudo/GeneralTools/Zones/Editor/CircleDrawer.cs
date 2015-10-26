using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Editor;
using UnityEditor;

namespace Pseudo.Internal
{
	[CustomPropertyDrawer(typeof(Circle))]
	public class CircleDrawer : CustomPropertyDrawerBase
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			property.isExpanded = true;

			EditorGUI.BeginProperty(position, label, property);
			_currentPosition.height = EditorGUI.GetPropertyHeight(property, label, false);
			EditorGUI.LabelField(_currentPosition, label);
			_currentPosition.y += _currentPosition.height;

			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 13f;
			EditorGUI.indentLevel++;

			// X
			SerializedProperty xProperty = property.FindPropertyRelative("_x");
			Rect rect = EditorGUI.IndentedRect(_currentPosition);
			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			rect.width = rect.width / 2f - 1f;
			rect.height = EditorGUI.GetPropertyHeight(xProperty, xProperty.ToGUIContent());
			EditorGUI.BeginProperty(rect, label, xProperty);

			EditorGUI.BeginChangeCheck();

			float x = EditorGUI.FloatField(rect, xProperty.ToGUIContent(), xProperty.GetValue<float>());

			if (EditorGUI.EndChangeCheck())
				xProperty.SetValue(x);

			EditorGUI.EndProperty();

			// Y
			SerializedProperty yProperty = property.FindPropertyRelative("_y");
			rect.x += rect.width + 2f;
			rect.height = EditorGUI.GetPropertyHeight(yProperty, yProperty.ToGUIContent());
			EditorGUI.BeginProperty(rect, label, yProperty);

			EditorGUI.BeginChangeCheck();

			float y = EditorGUI.FloatField(rect, yProperty.ToGUIContent(), yProperty.GetValue<float>());

			if (EditorGUI.EndChangeCheck())
				yProperty.SetValue(y);

			_currentPosition.y += _currentPosition.height;
			EditorGUIUtility.labelWidth = labelWidth;
			EditorGUI.EndProperty();
			EditorGUI.indentLevel = indent;

			// Radius
			PropertyField(property.FindPropertyRelative("_radius"));

			EditorGUI.indentLevel--;
			EditorGUI.EndProperty();

			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 48f;
		}
	}
}