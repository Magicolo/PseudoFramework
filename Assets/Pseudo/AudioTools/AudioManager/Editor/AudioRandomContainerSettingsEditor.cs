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
	[CustomEditor(typeof(AudioRandomContainerSettings)), CanEditMultipleObjects]
	public class AudioRandomContainerSettingsEditor : AudioContainerSettingsEditor
	{
		SerializedProperty _weigthsProperty;

		public override void OnInspectorGUI()
		{
			Begin(false);

			_weigthsProperty = serializedObject.FindProperty("Weights");

			base.OnInspectorGUI();

			End(false);
		}

		public override void ShowSource(SerializedProperty arrayProperty, int index, SerializedProperty sourceProperty)
		{
			base.ShowSource(arrayProperty, index, sourceProperty);

			_weigthsProperty.arraySize = arrayProperty.arraySize;

			if (sourceProperty.isExpanded)
			{
				EditorGUI.indentLevel++;

				EditorGUILayout.PropertyField(_sourceSettingsProperty);
				EditorGUILayout.PropertyField(_weigthsProperty.GetArrayElementAtIndex(index), "Weight".ToGUIContent());
				ArrayFoldout(sourceProperty.FindPropertyRelative("Options"), disableOnPlay: false);

				EditorGUI.indentLevel--;
			}
		}

		public override void OnSourceDeleted(SerializedProperty arrayProperty, int index)
		{
			base.OnSourceDeleted(arrayProperty, index);

			DeleteFromArray(_weigthsProperty, index);
		}

		public override void OnSourceReordered(SerializedProperty arrayProperty, int sourceIndex, int targetIndex)
		{
			base.OnSourceReordered(arrayProperty, sourceIndex, targetIndex);

			ReorderArray(_weigthsProperty, sourceIndex, targetIndex);
		}
	}
}