﻿using UnityEngine;
using UnityEditor;

namespace Pseudo.Internal.Editor
{
	[CustomPropertyDrawer(typeof(ClampAttribute))]
	public class ClampDrawer : CustomAttributePropertyDrawerBase
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			drawPrefixLabel = false;

			Begin(position, property, label);

			float min = ((ClampAttribute)attribute).min;
			float max = ((ClampAttribute)attribute).max;

			EditorGUI.BeginChangeCheck();

			EditorGUI.PropertyField(_currentPosition, property, label, true);

			if (EditorGUI.EndChangeCheck())
			{
				property.Clamp(min, max);
			}

			End();
		}
	}
}
