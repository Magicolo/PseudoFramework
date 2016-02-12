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
	[CustomEditor(typeof(AudioEnumeratorContainerSettings)), CanEditMultipleObjects]
	public class AudioEnumeratorContainerSettingsEditor : AudioContainerSettingsEditor
	{
		SerializedProperty repeats;

		public override void OnInspectorGUI()
		{
			Begin(false);

			repeats = serializedObject.FindProperty("Repeats");

			base.OnInspectorGUI();

			End(false);
		}

		public override void ShowSource(SerializedProperty arrayProperty, int index, SerializedProperty sourceProperty)
		{
			base.ShowSource(arrayProperty, index, sourceProperty);

			repeats.arraySize = arrayProperty.arraySize;

			if (sourceProperty.isExpanded)
			{
				EditorGUI.indentLevel++;

				EditorGUILayout.PropertyField(sourceSettingsProperty);
				EditorGUILayout.PropertyField(repeats.GetArrayElementAtIndex(index), "Repeat".ToGUIContent());
				repeats.GetArrayElementAtIndex(index).Max(1f);
				ArrayFoldout(sourceProperty.FindPropertyRelative("Options"), disableOnPlay: false);

				EditorGUI.indentLevel--;
			}
		}

		public override void OnSourceDeleted(SerializedProperty arrayProperty, int index)
		{
			base.OnSourceDeleted(arrayProperty, index);

			DeleteFromArray(repeats, index);
		}

		public override void OnSourceReordered(SerializedProperty arrayProperty, int sourceIndex, int targetIndex)
		{
			base.OnSourceReordered(arrayProperty, sourceIndex, targetIndex);

			ReorderArray(repeats, sourceIndex, targetIndex);
		}
	}
}