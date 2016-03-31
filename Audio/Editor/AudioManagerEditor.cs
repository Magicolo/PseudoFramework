using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Pseudo;
using Pseudo.Audio.Internal;
using Pseudo.Pooling;
using Pseudo.Audio;

namespace Pseudo.Editor.Internal
{
	[CustomEditor(typeof(AudioManager), true), CanEditMultipleObjects]
	public class AudioManagerEditor : PEditor
	{
		static AudioManager audioManager;
		static IAudioItem previewItem;
		static AudioSettingsBase previewSettings;
		static bool stopPreview;
		static bool audioManagerExists;

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

			var settings = AssetDatabase.LoadAssetAtPath<AudioSettingsBase>(AssetDatabase.GUIDToAssetPath(guid));

			if (settings != null)
				ShowPreviewButton(selectionRect, settings);
		}

		static void Update()
		{
			audioManager = FindObjectOfType<AudioManager>();
			audioManagerExists = audioManager != null;

			if (!audioManagerExists || Application.isPlaying)
				return;

			if (stopPreview || previewItem == null || previewItem.State == AudioStates.Stopped || Selection.activeObject != previewSettings)
				StopPreview();

			audioManager.Update();
		}

		static void PlayPreview(AudioSettingsBase settings)
		{
			StopPreview();
			EditorUtility.SetDirty(settings);
			previewSettings = settings;
			previewItem = audioManager.CreateItem(previewSettings);
			previewItem.OnStop += item => { stopPreview = true; previewItem = null; };
			previewItem.Play();
		}

		static void StopPreview()
		{
			if (audioManager == null)
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
				PrefabPoolManager.ClearPool(audioManager.Reference);

			stopPreview = false;
		}

		public static void ShowPreviewButton(Rect rect, AudioSettingsBase settings)
		{
			if (audioManager == null)
				return;

			// Check if scrollbar is visible
			if (Screen.width - rect.x - rect.width > 5f)
				rect.x = Screen.width - 40f;
			else
				rect.x = Screen.width - 24f;

			rect.width = 21f;
			rect.height = 16f;

			var buttonStyle = new GUIStyle("MiniToolbarButtonLeft");
			buttonStyle.fixedHeight += 1;

			if (GUI.Button(rect, "", buttonStyle))
			{
				Selection.activeObject = settings;

				if (previewSettings != settings || (previewItem != null && previewItem.State == AudioStates.Stopping))
					PlayPreview(settings);
				else if (previewItem != null)
					previewItem.Stop();
				else
					StopPreview();
			}

			bool playing = previewItem == null || previewItem.State == AudioStates.Stopping || previewSettings != settings;
			var labelStyle = new GUIStyle("boldLabel");
			labelStyle.fixedHeight += 1;
			labelStyle.fontSize = playing ? 14 : 20;
			labelStyle.contentOffset = playing ? new Vector2(2f, -2f) : new Vector2(2f, -7f);
			labelStyle.clipping = TextClipping.Overflow;

			GUI.Label(rect, playing ? "►" : "■", labelStyle);
		}
	}
}