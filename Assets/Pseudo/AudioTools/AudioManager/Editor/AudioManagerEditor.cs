using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Pseudo;

namespace Pseudo.Internal.Editor
{
	[CustomEditor(typeof(PAudio), true), CanEditMultipleObjects]
	public class AudioManagerEditor : CustomEditorBase
	{
		static AudioItem _previewItem;
		static AudioSettingsBase _previewSettings;

		public override void OnInspectorGUI()
		{
			Begin();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("UseCustomCurves"));

			End();
		}

		[UnityEditor.Callbacks.DidReloadScripts, InitializeOnLoadMethod]
		static void InitializeCallbacks()
		{
			EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
			EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
			EditorApplication.update += Update;
		}

		static void OnPlaymodeStateChanged()
		{
			if (PAudio.Find() == null)
				return;

			StopPreview();
		}

		static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
		{
			if (PAudio.Find() == null)
				return;

			AudioSettingsBase settings = AssetDatabase.LoadAssetAtPath<AudioSettingsBase>(AssetDatabase.GUIDToAssetPath(guid));

			if (settings != null)
				ShowPreviewButton(selectionRect, settings);
		}

		static void Update()
		{
			if (PAudio.Find() == null || Application.isPlaying)
				return;

			if (_previewItem == null || _previewItem.State == AudioItem.AudioStates.Stopped || Selection.activeObject != _previewSettings)
				StopPreview();

			PAudio.Instance.ItemManager.Update();
		}

		static void StopPreview()
		{
			if (PAudio.Find() == null)
				return;

			if (_previewItem != null)
			{
				_previewItem.StopImmediate();
				_previewItem = null;
				_previewSettings = null;
			}
		}

		public static void ShowPreviewButton(Rect rect, AudioSettingsBase settings)
		{
			if (PAudio.Find() == null)
				return;

			// Check if scrollbar is visible
			if (Screen.width - rect.x - rect.width > 5f)
				rect.x = Screen.width - 40f;
			else
				rect.x = Screen.width - 24f;

			rect.width = 21f;
			rect.height = 16f;

			GUIStyle buttonStyle = new GUIStyle("MiniToolbarButtonLeft");
			buttonStyle.fixedHeight += 1;

			if (GUI.Button(rect, "", buttonStyle))
			{
				Selection.activeObject = settings;

				if (_previewSettings != settings || (_previewItem != null && _previewItem.State == AudioItem.AudioStates.Stopping))
				{
					StopPreview();

					EditorUtility.SetDirty(settings);
					_previewSettings = settings;
					_previewItem = PAudio.Instance.CreateItem(_previewSettings);
					_previewItem.OnStop += item => { StopPreview(); EditorUtility.SetDirty(settings); EditorApplication.RepaintProjectWindow(); };
					_previewItem.Play();
				}
				else if (_previewItem != null)
					_previewItem.Stop();
				else
					StopPreview();
			}

			bool playing = _previewItem == null || _previewItem.State == AudioItem.AudioStates.Stopping || _previewSettings != settings;
			GUIStyle labelStyle = new GUIStyle("boldLabel");
			labelStyle.fixedHeight += 1;
			labelStyle.fontSize = playing ? 14 : 20;
			labelStyle.contentOffset = playing ? new Vector2(2f, -1f) : new Vector2(2f, -8f);
			labelStyle.clipping = TextClipping.Overflow;

			GUI.Label(rect, playing ? "►" : "■", labelStyle);
		}
	}
}