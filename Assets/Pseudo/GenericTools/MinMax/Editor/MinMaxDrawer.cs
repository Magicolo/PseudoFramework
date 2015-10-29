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

			BeginIndent(0);
			BeginLabelWidth(26f);

			currentPosition.width = currentPosition.width / 2f - 1f;
			EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("min"));
			currentPosition.x += currentPosition.width + 2f;
			EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("max"));

			EndLabelWidth();
			EndIndent();

			End();
		}
	}
}