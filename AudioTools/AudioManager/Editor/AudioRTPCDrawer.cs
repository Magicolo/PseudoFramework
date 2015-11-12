using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using UnityEditor;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal.Audio
{
	[CustomPropertyDrawer(typeof(AudioRTPC))]
	public class AudioRTPCDrawer : CustomPropertyDrawerBase
	{
		AudioRTPC rtpc;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			rtpc = property.GetValue<AudioRTPC>();

			Begin(position, property, label);

			string rtpcName = string.Format("{4}{0} | {1} [{2}, {3}]", rtpc.Name, rtpc.Type, rtpc.MinValue, rtpc.MaxValue, rtpc.Scope == AudioRTPC.RTPCScope.Global ? "*" : "");
			PropertyField(property, rtpcName.ToGUIContent(), false);

			if (property.isExpanded)
			{
				EditorGUI.indentLevel++;

				PropertyField(property.FindPropertyRelative("Scope"), GUIContent.none);
				PropertyField(property.FindPropertyRelative("Name"));
				PropertyField(property.FindPropertyRelative("Type"));
				PropertyField(property.FindPropertyRelative("MinValue"));
				PropertyField(property.FindPropertyRelative("MaxValue"));
				PropertyField(property.FindPropertyRelative("Curve"));

				EditorGUI.indentLevel--;
			}

			End();
		}
	}
}