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
	public class AudioContainerSettingsEditor : AudioSettingsBaseEditor
	{
		protected SerializedProperty sourcesProperty;
		protected SerializedProperty sourceSettingsProperty;

		public override void OnInspectorGUI()
		{
			ShowType();
			ShowSources();
			ShowGeneral();
			ShowRTPCs();
			ShowOptions();

			if (CheckReferenceCycles())
				Debug.LogError("Reference cycle detected.");
		}

		public void ShowSources()
		{
			sourcesProperty = serializedObject.FindProperty("Sources");
			ArrayFoldout(sourcesProperty, disableOnPlay: false, foldoutDrawer: ShowSourcesFoldout, elementDrawer: ShowSource, addCallback: OnSourceAdded, deleteCallback: OnSourceDeleted, reorderCallback: OnSourceReordered);
		}

		public void ShowSourcesFoldout(SerializedProperty arrayProperty)
		{
			AddFoldOut<AudioSettingsBase>(arrayProperty, OnSourceDropped);
		}

		public virtual void ShowSource(SerializedProperty arrayProperty, int index, SerializedProperty sourceProperty)
		{
			sourceSettingsProperty = sourceProperty.FindPropertyRelative("Settings");
			var settings = sourceSettingsProperty.GetValue<AudioSettingsBase>();

			Foldout(sourceProperty, string.Format("{0}", settings == null ? "null" : settings.name).ToGUIContent(), PEditorStyles.BoldFoldout);

			var rect = EditorGUI.IndentedRect(GUILayoutUtility.GetLastRect());
			rect.width += 6f;
			rect.height = 15f;

			DropArea<AudioSettingsBase>(rect, OnSettingsDropped);
		}

		public bool CheckReferenceCycles()
		{
			return CheckReferenceCycles((AudioContainerSettings)target, new List<AudioSettingsBase>());
		}

		bool CheckReferenceCycles(AudioContainerSettings settings, List<AudioSettingsBase> references)
		{
			bool isCycling = false;

			if (settings != null && settings.Sources != null)
			{
				for (int i = 0; i < settings.Sources.Count; i++)
				{
					var source = settings.Sources[i];

					if (source == null || source.Settings == null || isCycling)
						continue;

					if (references.Contains(source.Settings))
					{
						source.Settings = null;
						isCycling = true;
					}
					else
					{
						references.Add(source.Settings);
						var containerSettings = source.Settings as AudioContainerSettings;

						if (containerSettings != null)
							isCycling |= CheckReferenceCycles(containerSettings, references);

						references.Remove(source.Settings);
					}
				}
			}

			return isCycling;
		}

		public virtual void OnSourceAdded(SerializedProperty arrayProperty)
		{
			AddToArray(arrayProperty);
		}

		public virtual void OnSourceDeleted(SerializedProperty arrayProperty, int index)
		{
			DeleteFromArray(arrayProperty, index);
		}

		public virtual void OnSourceReordered(SerializedProperty arrayProperty, int sourceIndex, int targetIndex)
		{
			ReorderArray(arrayProperty, sourceIndex, targetIndex);
		}

		public virtual void OnSourceDropped(AudioSettingsBase settings)
		{
			AddToArray(sourcesProperty);

			var sourceProperty = sourcesProperty.Last();
			sourceProperty.SetValue("Settings", settings);
			sourceProperty.FindPropertyRelative("Options").Clear();
		}

		public virtual void OnSettingsDropped(AudioSettingsBase settings)
		{
			sourceSettingsProperty.SetValue(settings);
		}

		public override float GetSettingsLength(AudioSettingsBase settings)
		{
			return float.MaxValue;
		}
	}
}