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
	public abstract class AudioSettingsBaseEditor : CustomEditorBase
	{
		AudioSettingsBase settings;

		public override void OnEnable()
		{
			base.OnEnable();

			settings = (AudioSettingsBase)target;
		}

		public void ShowType()
		{
			GUIStyle style = new GUIStyle("boldLabel");
			style.alignment = TextAnchor.MiddleCenter;
			EditorGUILayout.LabelField(settings.Type.ToString().ToUpper(), style);
		}

		public void ShowGeneral()
		{
			Separator();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("Loop"));

			ShowFades();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("VolumeScale"));
			ShowPitchScale();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("RandomVolume"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("RandomPitch"));

			Separator();
		}

		public void ShowRTPCs()
		{
			ArrayFoldout(serializedObject.FindProperty("RTPCs"), "RTPCs".ToGUIContent(), disableOnPlay: false);
		}

		public void ShowOptions()
		{
			ArrayFoldout(serializedObject.FindProperty("Options"), disableOnPlay: false, reorderCallback: (p, s, t) => Repaint());
		}

		void ShowPitchScale()
		{
			EditorGUIUtility.fieldWidth -= 20f;

			EditorGUILayout.BeginHorizontal();

			SerializedProperty pitchScaleModeProperty = serializedObject.FindProperty("PitchScaleMode");
			AudioSettingsBase.PitchScaleModes pitchScaleMode = pitchScaleModeProperty.GetValue<AudioSettingsBase.PitchScaleModes>();
			SerializedProperty pitchScaleProperty = serializedObject.FindProperty("PitchScale");

			if (pitchScaleMode == AudioSettingsBase.PitchScaleModes.Ratio)
				EditorGUILayout.PropertyField(pitchScaleProperty);
			else
			{
				float pitchScale = pitchScaleProperty.GetValue<float>();
				int selectedValue = Mathf.RoundToInt(Mathf.Log(pitchScale, 2f) * 12f);

				EditorGUI.BeginChangeCheck();

				selectedValue = EditorGUILayout.IntSlider("Pitch Scale", selectedValue, -24, 24);

				if (EditorGUI.EndChangeCheck())
				{
					pitchScale = Mathf.Pow(2f, selectedValue / 12f);

					for (int i = 0; i < targets.Length; i++)
					{
						AudioSettingsBase settings = (AudioSettingsBase)targets[i];
						settings.PitchScale = pitchScale;
					}

					serializedObject.Update();
				}
			}

			GUIStyle style = new GUIStyle("button");
			style.clipping = TextClipping.Overflow;

			EditorGUI.BeginChangeCheck();

			pitchScaleMode = GUILayout.Toggle(pitchScaleMode == AudioSettingsBase.PitchScaleModes.Ratio, "R", style, GUILayout.Width(16f), GUILayout.Height(14f)) ? AudioSettingsBase.PitchScaleModes.Ratio : AudioSettingsBase.PitchScaleModes.Semitone;

			if (EditorGUI.EndChangeCheck())
			{
				for (int i = 0; i < targets.Length; i++)
				{
					AudioSettingsBase settings = (AudioSettingsBase)targets[i];
					settings.PitchScaleMode = pitchScaleMode;
				}

				serializedObject.Update();
			}

			EditorGUILayout.EndHorizontal();

			EditorGUIUtility.fieldWidth += 20f;
		}

		void ShowFades()
		{
			SerializedProperty fadeInProperty = serializedObject.FindProperty("FadeIn");
			SerializedProperty fadeOutProperty = serializedObject.FindProperty("FadeOut");
			SerializedProperty fadeInEaseProperty = serializedObject.FindProperty("FadeInEase");
			SerializedProperty fadeOutEaseProperty = serializedObject.FindProperty("FadeOutEase");

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(fadeInProperty);

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				fadeOutProperty.Clamp(0f, GetSettingsLength(settings) - fadeInProperty.GetValue<float>());
			}

			ShowFadeEase(fadeInEaseProperty);

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(fadeOutProperty);

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				fadeInProperty.Clamp(0f, GetSettingsLength(settings) - fadeOutProperty.GetValue<float>());
			}

			ShowFadeEase(fadeOutEaseProperty);

			EditorGUILayout.EndHorizontal();
			if (EditorGUI.EndChangeCheck())
				ClampFades();
		}

		void ShowFadeEase(SerializedProperty easeProperty)
		{
			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			EditorGUILayout.PropertyField(easeProperty, GUIContent.none, GUILayout.Width(Screen.width * 0.25f));
			EditorGUI.indentLevel = indent;
		}

		public virtual void ClampFades()
		{
			for (int i = 0; i < targets.Length; i++)
			{
				AudioSettingsBase settings = (AudioSettingsBase)targets[i];
				settings.FadeIn = Mathf.Clamp(settings.FadeIn, 0f, GetSettingsLength(settings));
				settings.FadeOut = Mathf.Clamp(settings.FadeOut, 0f, GetSettingsLength(settings));
			}

			serializedObject.Update();
		}

		public abstract float GetSettingsLength(AudioSettingsBase settings);
	}
}