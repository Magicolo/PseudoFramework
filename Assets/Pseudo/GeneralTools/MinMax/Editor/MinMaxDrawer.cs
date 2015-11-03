using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEditor;

namespace Pseudo.Internal.Editor
{
	[CustomPropertyDrawer(typeof(MinMax))]
	public class MinMaxDrawer : CustomPropertyDrawerBase
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			//PropertyField(property, label, false);
			currentPosition = EditorGUI.PrefixLabel(currentPosition, label);
			currentPosition.x -= 1f;

			BeginIndent(0);
			BeginLabelWidth(27f);

			EditorGUI.BeginChangeCheck();

			currentPosition.width = currentPosition.width / 2f;
			EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("min"));

			if (EditorGUI.EndChangeCheck())
				property.SetValue("max", Mathf.Max(property.GetValue<float>("max"), property.GetValue<float>("min")));

			EditorGUI.BeginChangeCheck();

			currentPosition.x += currentPosition.width + 1f;
			EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("max"));

			if (EditorGUI.EndChangeCheck())
				property.SetValue("min", Mathf.Min(property.GetValue<float>("min"), property.GetValue<float>("max")));

			EndLabelWidth();
			EndIndent();

			End();
		}
	}
}