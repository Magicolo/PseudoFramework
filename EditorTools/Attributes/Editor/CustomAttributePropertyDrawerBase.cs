using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Pseudo.Internal.Editor
{
	public class CustomAttributePropertyDrawerBase : CustomPropertyDrawerBase
	{
		public string prefixLabel;
		public bool noFieldLabel;
		public bool noPrefixLabel;
		public bool noIndex;
		public bool disableOnPlay;
		public bool disableOnStop;
		public string disableBool;
		public bool beforeSeparator;
		public bool afterSeparator;
		public int indent;

		public bool drawPrefixLabel = true;
		public bool boolDisabled;

		public override void Begin(Rect position, SerializedProperty property, GUIContent label)
		{
			base.Begin(position, property, label);

			initPosition = position;

			if (beforeSeparator)
			{
				position.y += 4;
				EditorGUI.LabelField(position, GUIContent.none, new GUIStyle("RL DragHandle"));
				position.y += 12;
			}

			EditorGUI.BeginDisabledGroup((Application.isPlaying && disableOnPlay) || (!Application.isPlaying && disableOnStop) || boolDisabled);
			EditorGUI.indentLevel += indent;

			if (isArray)
			{
				if (noIndex)
				{
					if (string.IsNullOrEmpty(prefixLabel))
					{
						label.text = label.text.Substring(0, label.text.Length - 2);
					}
				}
				else if (!string.IsNullOrEmpty(prefixLabel))
				{
					prefixLabel += " " + index;
				}
			}

			if (drawPrefixLabel)
			{
				if (!noPrefixLabel)
				{
					if (!string.IsNullOrEmpty(prefixLabel))
					{
						label.text = prefixLabel;
					}

					position = EditorGUI.PrefixLabel(position, label);
				}
			}
			else
			{
				if (noPrefixLabel)
				{
					label.text = "";
				}
				else if (!string.IsNullOrEmpty(prefixLabel))
				{
					label.text = prefixLabel;
				}
			}

			currentPosition = position;
			currentLabel = label;
		}

		public override void End()
		{
			base.End();

			EditorGUI.indentLevel -= indent;
			EditorGUI.EndDisabledGroup();

			if (afterSeparator)
			{
				EditorGUI.LabelField(new Rect(initPosition.x, initPosition.yMax - 10, initPosition.width, initPosition.height), "", new GUIStyle("RL DragHandle"));
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			var customAttribute = (CustomAttributeBase)attribute;
			noFieldLabel = customAttribute.NoFieldLabel;
			noPrefixLabel = customAttribute.NoPrefixLabel;
			noIndex = customAttribute.NoIndex;
			prefixLabel = customAttribute.PrefixLabel;
			disableOnPlay = customAttribute.DisableOnPlay;
			disableOnStop = customAttribute.DisableOnStop;
			disableBool = customAttribute.DisableBool;
			indent = customAttribute.Indent;
			beforeSeparator = customAttribute.BeforeSeparator;
			afterSeparator = customAttribute.AfterSeparator;

			bool inverseBool = false;

			if (!string.IsNullOrEmpty(disableBool))
			{
				inverseBool = disableBool.StartsWith("!");

				string boolPath = property.GetParent().FindPropertyRelative(inverseBool ? disableBool.Substring(1) : disableBool).GetAdjustedPath();

				boolDisabled = property.serializedObject.targetObject.GetValueFromMemberAtPath<bool>(boolPath);
			}

			boolDisabled = inverseBool ? !boolDisabled : boolDisabled;

			return EditorGUI.GetPropertyHeight(property, label, true) + (beforeSeparator ? 16 : 0) + (afterSeparator ? 16 : 0);
		}
	}
}