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
	public class MinMaxDrawer : PPropertyDrawer
	{
		bool isEditingMin;
		bool isEditingMax;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			currentPosition = EditorGUI.PrefixLabel(currentPosition, label);
			currentPosition.x -= 1f;

			BeginIndent(0);
			BeginLabelWidth(27f);

			if (isEditingMin && !EditorGUIUtility.editingTextField)
			{
				property.SetValue("max", Mathf.Max(property.GetValue<float>("max"), property.GetValue<float>("min")));
				isEditingMin = false;
			}
			else if (isEditingMax && !EditorGUIUtility.editingTextField)
			{
				property.SetValue("min", Mathf.Min(property.GetValue<float>("min"), property.GetValue<float>("max")));
				isEditingMax = false;
			}

			EditorGUI.BeginChangeCheck();

			currentPosition.width = currentPosition.width / 2f;
			EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("min"));

			if (EditorGUI.EndChangeCheck())
			{
				if (EditorGUIUtility.editingTextField)
					isEditingMin = true;
				else
					property.SetValue("max", Mathf.Max(property.GetValue<float>("max"), property.GetValue<float>("min")));
			}

			EditorGUI.BeginChangeCheck();

			currentPosition.x += currentPosition.width + 1f;
			EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("max"));

			if (EditorGUI.EndChangeCheck())
			{
				if (EditorGUIUtility.editingTextField)
					isEditingMax = true;
				else
					property.SetValue("min", Mathf.Min(property.GetValue<float>("min"), property.GetValue<float>("max")));
			}

			EndLabelWidth();
			EndIndent();

			End();
		}
	}
}