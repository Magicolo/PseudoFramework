﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Pseudo.Internal.Editor
{
	[System.Serializable]
	[CustomPropertyDrawer(typeof(ShowPropertiesAttribute)), CanEditMultipleObjects]
	public class ShowPropertiesDrawer : CustomAttributePropertyDrawerBase
	{
		SerializedObject serialized;
		SerializedProperty iterator;
		float totalHeight;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			drawPrefixLabel = false;
			totalHeight = 0;

			Begin(position, property, label);

			position.height = EditorGUI.GetPropertyHeight(property, label, true);

			EditorGUI.PropertyField(position, property);

			if (property.GetValue() != null)
				property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);

			totalHeight += position.height;
			position.y += position.height;

			if (property.isExpanded && serialized != null)
			{
				iterator = serialized.GetIterator();
				iterator.NextVisible(true);
				iterator.NextVisible(true);

				EditorGUI.indentLevel += 1;
				int currentIndent = EditorGUI.indentLevel;

				while (true)
				{
					position.height = EditorGUI.GetPropertyHeight(iterator, iterator.displayName.ToGUIContent(), false);

					totalHeight += position.height;

					EditorGUI.indentLevel = currentIndent + iterator.depth;
					EditorGUI.PropertyField(position, iterator);

					position.y += position.height;

					if (!iterator.NextVisible(iterator.isExpanded))
					{
						break;
					}
				}

				EditorGUI.indentLevel = currentIndent;
				EditorGUI.indentLevel -= 1;

				serialized.ApplyModifiedProperties();
			}

			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			serialized = property.objectReferenceValue == null ? null : new SerializedObject(property.objectReferenceValue);

			if (totalHeight <= 0f)
				InitializeHeight(property, label);

			return totalHeight + (beforeSeparator ? 16f : 0f) + (afterSeparator ? 16f : 0f);
		}

		public void InitializeHeight(SerializedProperty property, GUIContent label)
		{
			totalHeight = EditorGUI.GetPropertyHeight(property, label, true);

			if (property.isExpanded && serialized != null)
			{
				iterator = serialized.GetIterator();
				iterator.NextVisible(true);
				iterator.NextVisible(true);

				while (true)
				{
					totalHeight += EditorGUI.GetPropertyHeight(iterator, iterator.displayName.ToGUIContent(), false);

					if (!iterator.NextVisible(iterator.isExpanded))
					{
						break;
					}
				}
			}
		}
	}
}

