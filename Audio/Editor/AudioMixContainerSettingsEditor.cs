using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using UnityEditor;
using Pseudo.Editor.Internal;

namespace Pseudo.Audio.Internal
{
	[CustomEditor(typeof(AudioMixContainerSettings)), CanEditMultipleObjects]
	public class AudioMixContainerSettingsEditor : AudioContainerSettingsEditor
	{
		SerializedProperty delaysProperty;

		public override void OnInspectorGUI()
		{
			Begin(false);

			delaysProperty = serializedObject.FindProperty("Delays");

			base.OnInspectorGUI();

			End(false);
		}

		public override void ShowSource(SerializedProperty arrayProperty, int index, SerializedProperty sourceProperty)
		{
			base.ShowSource(arrayProperty, index, sourceProperty);

			delaysProperty.arraySize = arrayProperty.arraySize;

			if (sourceProperty.isExpanded)
			{
				EditorGUI.indentLevel++;

				EditorGUILayout.PropertyField(sourceSettingsProperty);
				EditorGUILayout.PropertyField(delaysProperty.GetArrayElementAtIndex(index), "Delay".ToGUIContent());
				ArrayFoldout(sourceProperty.FindPropertyRelative("Options"), disableOnPlay: false);

				EditorGUI.indentLevel--;
			}
		}

		public override void OnSourceDeleted(SerializedProperty arrayProperty, int index)
		{
			base.OnSourceDeleted(arrayProperty, index);

			DeleteFromArray(delaysProperty, index);
		}

		public override void OnSourceReordered(SerializedProperty arrayProperty, int sourceIndex, int targetIndex)
		{
			base.OnSourceReordered(arrayProperty, sourceIndex, targetIndex);

			ReorderArray(delaysProperty, sourceIndex, targetIndex);
		}
	}
}