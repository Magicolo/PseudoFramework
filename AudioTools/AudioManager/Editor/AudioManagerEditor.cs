using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Pseudo;

namespace Pseudo.Internal.Editor
{
	[CustomEditor(typeof(AudioManager), true), CanEditMultipleObjects]
	public class AudioManagerEditor : CustomEditorBase
	{
		static AudioItem previewItem;
		static AudioSettingsBase previewSettings;
		static bool stopPreview;
		static bool audioManagerExists;

		public override void OnInspectorGUI()
		{
			Begin();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("UseCustomCurves"));

			End();
		}

		[UnityEditor.Callbacks.DidReloadScripts, InitializeOnLoadMethod]
		static void InitializeCallbacks()
		{
			EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
			EditorApplication.projectWindowItemOnGUI -= OnProjectWindowItemGUI;
			EditorApplication.update -= Update;

			EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
			EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
			EditorApplication.update += Update;
		}

		static void OnPlaymodeStateChanged()
		{
			if (!audioManagerExists)
				return;

			StopPreview();
		}

		static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
		{
			if (!audioManagerExists)
				return;

			AudioSettingsBase settings = AssetDatabase.LoadAssetAtPath<AudioSettingsBase>(AssetDatabase.GUIDToAssetPath(guid));

			if (settings != null)
				ShowPreviewButton(selectionRect, settings);
		}

		static void Update()
		{
			audioManagerExists = AudioManager.Find() != null;

			if (!audioManagerExists || Application.isPlaying)
				return;

			if (stopPreview || previewItem == null || previewItem.State == AudioItem.AudioStates.Stopped || Selection.activeObject != previewSettings)
				StopPreview();

			AudioManager.Instance.ItemManager.Update();
		}

		static void PlayPreview(AudioSettingsBase settings)
		{
			StopPreview();
			EditorUtility.SetDirty(settings);
			previewSettings = settings;
			previewItem = AudioManager.Instance.CreateItem(previewSettings);
			previewItem.OnStop += item => { stopPreview = true; previewItem = null; };
			previewItem.Play();
		}

		static void StopPreview()
		{
			if (AudioManager.Instance == null)
				return;

			if (previewItem != null)
			{
				previewItem.StopImmediate();
				previewItem = null;
			}

			if (previewSettings != null)
			{
				EditorUtility.SetDirty(previewSettings);
				EditorApplication.RepaintProjectWindow();
				previewSettings = null;
			}

			if (!Application.isPlaying)
				PrefabPoolManager.ClearPool(AudioManager.Instance.Reference);

			stopPreview = false;
		}

		public static void ShowPreviewButton(Rect rect, AudioSettingsBase settings)
		{
			if (AudioManager.Instance == null)
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

				if (previewSettings != settings || (previewItem != null && previewItem.State == AudioItem.AudioStates.Stopping))
					PlayPreview(settings);
				else if (previewItem != null)
					previewItem.Stop();
				else
					StopPreview();
			}

			bool playing = previewItem == null || previewItem.State == AudioItem.AudioStates.Stopping || previewSettings != settings;
			GUIStyle labelStyle = new GUIStyle("boldLabel");
			labelStyle.fixedHeight += 1;
			labelStyle.fontSize = playing ? 14 : 20;
			labelStyle.contentOffset = playing ? new Vector2(2f, -1f) : new Vector2(2f, -8f);
			labelStyle.clipping = TextClipping.Overflow;

			GUI.Label(rect, playing ? "►" : "■", labelStyle);
		}
	}
}