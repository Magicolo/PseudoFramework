using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using UnityEditor;
using UnityEditorInternal;
using Pseudo.Editor.Internal;

namespace Pseudo.Audio.Internal
{
	[CustomEditor(typeof(AudioSourceSettings)), CanEditMultipleObjects]
	public class AudioSourceSettingsEditor : AudioSettingsBaseEditor
	{
		AudioSourceSettings sourceSettings;
		SerializedProperty clipProperty;
		Texture textureLeft;
		Texture textureRight;

		public override void OnEnable()
		{
			base.OnEnable();

			sourceSettings = target as AudioSourceSettings;
			clipProperty = serializedObject.FindProperty("Clip");

			InitializeTextures();
		}

		public override void OnInspectorGUI()
		{
			clipProperty = serializedObject.FindProperty("Clip");

			Begin(false);

			ShowWaves();

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(clipProperty);

			if (EditorGUI.EndChangeCheck())
				InitializeTextures();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("Output"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxInstances"));

			ShowGeneral();

			ShowRTPCs();
			ShowOptions();

			End(false);
		}

		void ShowWaves()
		{
			AudioClip clip = clipProperty.GetValue<AudioClip>();
			string playRangeStartSeconds = clip == null ? "" : "(" + (sourceSettings.PlayRangeStart * clip.length).Round(0.01f) + "s)";
			string playRangeEndSeconds = clip == null ? "" : "(" + (sourceSettings.PlayRangeEnd * clip.length).Round(0.01f) + "s)";
			string curvesLabel = string.Format("Start: {0} {2} | End: {1} {3}", sourceSettings.PlayRangeStart.Round(0.01f), sourceSettings.PlayRangeEnd.Round(0.01f), playRangeStartSeconds, playRangeEndSeconds);

			EditorGUILayout.LabelField(curvesLabel, GUILayout.Height(22f));
			AudioManagerEditor.ShowPreviewButton(EditorGUI.IndentedRect(GUILayoutUtility.GetLastRect()), sourceSettings);

			EditorGUI.BeginChangeCheck();

			Rect rect = EditorGUI.IndentedRect(GUILayoutUtility.GetLastRect());
			EditorGUI.MinMaxSlider(new Rect(rect.x - 9f, rect.y + 12f, rect.width + 10f, rect.height), ref sourceSettings.PlayRangeStart, ref sourceSettings.PlayRangeEnd, 0f, 1f);

			if (EditorGUI.EndChangeCheck())
			{
				for (int i = 0; i < targets.Length; i++)
				{
					AudioSourceSettings settings = (AudioSourceSettings)targets[i];
					settings.PlayRangeStart = float.IsNaN(sourceSettings.PlayRangeStart) ? 0f : Mathf.Clamp(sourceSettings.PlayRangeStart, 0f, settings.PlayRangeEnd);
					settings.PlayRangeEnd = float.IsNaN(sourceSettings.PlayRangeEnd) ? 1f : Mathf.Clamp(sourceSettings.PlayRangeEnd, settings.PlayRangeStart, 1f);
				}

				serializedObject.Update();
				ClampFades();
			}


			if (clip == null || clip.channels == 1)
				ShowWave(textureLeft, 40f);
			else
			{
				ShowWave(textureLeft, 20f);
				ShowWave(textureRight, 20f);
			}
		}

		void ShowWave(Texture texture, float height)
		{
			EditorGUILayout.LabelField(" ", GUILayout.Height(height * Screen.width / 200f));

			Rect rect = EditorGUI.IndentedRect(GUILayoutUtility.GetLastRect());
			rect.x -= 4f;

			GUI.DrawTexture(rect, texture, ScaleMode.StretchToFill);

			rect.y += 1;
			rect.height -= 2;

			GUIStyle style = new GUIStyle("LODBlackBox");
			float indentation = EditorGUI.indentLevel * 16f;

			GUI.Box(new Rect(rect.x + indentation + 1f, rect.y, (rect.width - indentation) * sourceSettings.PlayRangeStart, rect.height), "", style);
			GUI.Box(new Rect(rect.x + indentation + (rect.width - indentation) * sourceSettings.PlayRangeEnd - 1f, rect.y, (rect.width - indentation) * (1f - sourceSettings.PlayRangeEnd), rect.height), "", style);
		}

		void InitializeTextures()
		{
			float[] dataLeft;
			float[] dataRight;
			AudioClip clip = clipProperty.GetValue<AudioClip>();

			if (clip == null)
				textureLeft = GetWaveTexture(null, 1024, 256, 2);
			else
			{
				clip.GetUntangledData(out dataLeft, out dataRight);

				textureLeft = GetWaveTexture(dataLeft, 1024, 256 / clip.channels, 2);

				if (clip.channels > 1)
					textureRight = GetWaveTexture(dataRight, 1024, 256 / clip.channels, 2);
			}
		}

		Texture2D GetWaveTexture(float[] data, int width, int height, int border)
		{
			Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
			Color activeColor = new Color(1f, 0.55f, 0f, 1f);
			Color inactiveColor = new Color(0.1914f, 0.1914f, 0.1914f, 1f);
			Color borderColor = Color.black;
			Color[] pixels = new Color[width * height];

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					Color pixel;

					if (x <= border || x >= width - border - 1 || y <= border || y >= height - border - 1 || y == height / 2)
						pixel = borderColor;
					else if (data != null && Mathf.Abs((float)y / height * 2f - 1f) <= Mathf.Abs(data[x * Mathf.Min(data.Length / width, data.Length - 1)]))
						pixel = activeColor;
					else
						pixel = inactiveColor;

					pixels[x + y * width] = pixel;
				}
			}

			texture.filterMode = FilterMode.Point;
			texture.hideFlags = HideFlags.DontSave;
			texture.SetPixels(pixels);
			texture.Apply();

			return texture;
		}

		public override float GetSettingsLength(AudioSettingsBase settings)
		{
			return ((AudioSourceSettings)settings).GetLength();
		}
	}
}