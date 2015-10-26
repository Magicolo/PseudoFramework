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
		protected SerializedProperty _sourceSettingsProperty;

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
			ArrayFoldout(serializedObject.FindProperty("Sources"), disableOnPlay: false, drawer: ShowSource, addCallback: OnSourceAdded, deleteCallback: OnSourceDeleted, reorderCallback: OnSourceReordered);
		}

		public virtual void ShowSource(SerializedProperty arrayProperty, int index, SerializedProperty sourceProperty)
		{
			_sourceSettingsProperty = sourceProperty.FindPropertyRelative("Settings");
			AudioSettingsBase settings = _sourceSettingsProperty.GetValue<AudioSettingsBase>();

			Foldout(sourceProperty, string.Format("{0}", settings == null ? "null" : settings.Name).ToGUIContent(), CustomEditorStyles.BoldFoldout);

			Rect rect = EditorGUI.IndentedRect(GUILayoutUtility.GetLastRect());
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
					AudioContainerSourceData source = settings.Sources[i];

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
						AudioContainerSettings containerSettings = source.Settings as AudioContainerSettings;

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

		public virtual void OnSettingsDropped(AudioSettingsBase settings)
		{
			_sourceSettingsProperty.SetValue(settings);
		}

		public override float GetSettingsLength(AudioSettingsBase settings)
		{
			return float.MaxValue;
		}
	}
}