using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pseudo.Internal.Editor
{
	[CustomPropertyDrawer(typeof(FlagAttribute))]
	public class FlagDrawer : CustomAttributePropertyDrawerBase
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			drawPrefixLabel = false;

			Type type = ((FlagAttribute)attribute).Type;

			Begin(position, property, label);

			EditorGUI.BeginChangeCheck();

			int value = property.GetValue<int>();

			value = EditorGUI.MaskField(currentPosition, label, value, Enum.GetNames(type));

			if (EditorGUI.EndChangeCheck())
				property.SetValue(value);

			End();
		}
	}
}