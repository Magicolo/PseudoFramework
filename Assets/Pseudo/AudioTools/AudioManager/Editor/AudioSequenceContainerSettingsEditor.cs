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
	[CustomEditor(typeof(AudioSequenceContainerSettings)), CanEditMultipleObjects]
	public class AudioSequenceContainerSettingsEditor : AudioContainerSettingsEditor
	{
		SerializedProperty _delaysProperty;

		public override void OnInspectorGUI()
		{
			Begin(false);

			_delaysProperty = serializedObject.FindProperty("Delays");

			base.OnInspectorGUI();

			End(false);
		}

		public override void ShowSource(SerializedProperty arrayProperty, int index, SerializedProperty sourceProperty)
		{
			base.ShowSource(arrayProperty, index, sourceProperty);

			_delaysProperty.arraySize = arrayProperty.arraySize - 1;

			if (sourceProperty.isExpanded)
			{
				EditorGUI.indentLevel++;

				EditorGUILayout.PropertyField(_sourceSettingsProperty);

				ArrayFoldout(sourceProperty.FindPropertyRelative("Options"), disableOnPlay: false);

				EditorGUI.indentLevel--;
			}

			if (index < arrayProperty.arraySize - 1)
				EditorGUILayout.PropertyField(_delaysProperty.GetArrayElementAtIndex(index), "Delay".ToGUIContent());
		}
	}
}