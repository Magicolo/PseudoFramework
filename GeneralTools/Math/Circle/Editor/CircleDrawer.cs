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

			currentPosition.height = EditorGUI.GetPropertyHeight(property, label, false);
			EditorGUI.LabelField(currentPosition, label);
			currentPosition.y += currentPosition.height;

			EditorGUI.indentLevel++;
			BeginLabelWidth(13f);

			// X
			var xProperty = property.FindPropertyRelative("X");
			var rect = EditorGUI.IndentedRect(currentPosition);

			BeginIndent(0);

			rect.width = rect.width / 2f - 1f;
			rect.height = EditorGUI.GetPropertyHeight(xProperty, xProperty.ToGUIContent());
			EditorGUI.BeginProperty(rect, label, xProperty);

			EditorGUI.BeginChangeCheck();

			float x = EditorGUI.FloatField(rect, xProperty.ToGUIContent(), xProperty.GetValue<float>());

			if (EditorGUI.EndChangeCheck())
				xProperty.SetValue(x);

			EditorGUI.EndProperty();

			// Y
			var yProperty = property.FindPropertyRelative("Y");
			rect.x += rect.width + 2f;
			rect.height = EditorGUI.GetPropertyHeight(yProperty, yProperty.ToGUIContent());
			EditorGUI.BeginProperty(rect, label, yProperty);

			EditorGUI.BeginChangeCheck();

			float y = EditorGUI.FloatField(rect, yProperty.ToGUIContent(), yProperty.GetValue<float>());

			if (EditorGUI.EndChangeCheck())
				yProperty.SetValue(y);

			currentPosition.y += currentPosition.height;
			EditorGUI.EndProperty();

			EndLabelWidth();
			EndIndent();

			// Radius
			PropertyField(property.FindPropertyRelative("Radius"));

			EditorGUI.indentLevel--;

			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 48f;
		}
	}
}