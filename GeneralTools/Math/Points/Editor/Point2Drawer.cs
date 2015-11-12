using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEditor;

namespace Pseudo.Internal.Editor
{
	[CustomPropertyDrawer(typeof(Point2))]
	public class Point2Drawer : CustomPropertyDrawerBase
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			currentPosition = EditorGUI.PrefixLabel(currentPosition, label);
			currentPosition.x -= 1f;

			BeginIndent(0);
			BeginLabelWidth(13f);

			currentPosition.width = currentPosition.width / 2f;
			EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("X"));

			currentPosition.x += currentPosition.width + 1f;
			EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("Y"));

			EndLabelWidth();
			EndIndent();

			End();
		}
	}
}