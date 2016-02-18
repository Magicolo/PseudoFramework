using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEditor;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal.EntityOld
{
	[CustomPropertyDrawer(typeof(EntityMatchOld))]
	public class EntityMatchDrawer : CustomPropertyDrawerBase
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			var groupProperty = property.FindPropertyRelative("groups");
			var matchProperty = property.FindPropertyRelative("match");

			currentPosition = EditorGUI.PrefixLabel(currentPosition, property.ToGUIContent());

			BeginIndent(0);

			float width = currentPosition.width;
			currentPosition.width = width * 0.65f - 1f;
			currentPosition.height = EditorGUI.GetPropertyHeight(groupProperty, label);
			EditorGUI.PropertyField(currentPosition, groupProperty, GUIContent.none);
			currentPosition.x += currentPosition.width + 2f;
			currentPosition.width = width * 0.35f - 1f;
			EditorGUI.PropertyField(currentPosition, matchProperty, GUIContent.none);

			EndIndent();
			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			return 16f;
		}
	}
}