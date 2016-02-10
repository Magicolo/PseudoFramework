using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Editor;
using UnityEditor;
using System.Reflection;

namespace Pseudo.Internal.Oscillation
{
	[CustomPropertyDrawer(typeof(PropertyOscillator.OscillatorData))]
	public class OscillationDataDrawer : CustomPropertyDrawerBase
	{
		PropertyOscillator.OscillatorData data;
		PropertyInfo[] properties = new PropertyInfo[0];
		string[] propertyNames = new string[0];
		string[] propertyDisplayNames = new string[0];
		float height;

		readonly Dictionary<PropertyOscillator.OscillatorData, float> dataToHeight = new Dictionary<PropertyOscillator.OscillatorData, float>();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			UpdateProperties();
			GUIContent foldoutLabel;

			if (data.Target == null)
				foldoutLabel = label;
			else
				foldoutLabel = string.Format("{0}.{1}.{2}", data.Target.name, data.Target.GetType().Name, string.IsNullOrEmpty(data.PropertyName) ? "null" : data.PropertyName).ToGUIContent();

			PropertyField(property, foldoutLabel, false);

			if (property.isExpanded)
			{
				EditorGUI.indentLevel++;

				ShowTarget();
				ShowProperties();
				ShowAllSettings();

				EditorGUI.indentLevel--;
			}
			else
				currentPosition.y -= 2f;

			if (height == 0f)
				Event.current.Use();

			dataToHeight[data] = currentPosition.y - initPosition.y;

			if (Application.isPlaying && GUI.changed && PrefabUtility.GetPrefabType(target) != PrefabType.Prefab)
				data.Initialize();

			End();
		}

		public override void Initialize(SerializedProperty property, GUIContent label)
		{
			base.Initialize(property, label);

			UpdateProperties();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			data = property.GetValue<PropertyOscillator.OscillatorData>();
			height = GetHeight();

			if (height == 0f)
				return 18f;
			else
				return height;
		}

		float GetHeight()
		{
			float height;

			if (!dataToHeight.TryGetValue(data, out height))
			{
				height = 0f;
				dataToHeight[data] = height;
			}

			return height;
		}

		void ShowTarget()
		{
			//EditorGUI.BeginChangeCheck();

			PropertyField(currentProperty.FindPropertyRelative("Target"));

			//if (EditorGUI.EndChangeCheck())
			//	UpdateProperties();
		}

		void ShowProperties()
		{
			var propertyNameProperty = currentProperty.FindPropertyRelative("PropertyName");

			EditorGUI.BeginProperty(currentPosition, propertyNameProperty.ToGUIContent(), propertyNameProperty);
			EditorGUI.BeginChangeCheck();

			int index = Array.IndexOf(propertyNames, propertyNameProperty.GetValue<string>());
			index = EditorGUI.Popup(currentPosition, "Property", index, propertyDisplayNames);
			currentPosition.y += currentPosition.height + 2f;

			if (EditorGUI.EndChangeCheck())
				propertyNameProperty.SetValue(propertyNames[index]);
			EditorGUI.EndProperty();
		}

		void ShowAllSettings()
		{
			if (data.Property == null)
				return;

			var flagsProperty = currentProperty.FindPropertyRelative("Flags");
			var dataProperty = currentProperty.FindPropertyRelative("Settings");

			if (data.Property.PropertyType == typeof(float))
			{
				dataProperty.EnsureCapacity(1, () => new PropertyOscillator.OscillationRangeSettings());
				ShowSettings(dataProperty.GetArrayElementAtIndex(0));
			}
			else if (data.Property.PropertyType == typeof(Vector2))
			{
				dataProperty.EnsureCapacity(2, () => new PropertyOscillator.OscillationRangeSettings());
				EnumFlag(currentPosition, flagsProperty, "Axes".ToGUIContent(), Axes.X, Axes.Y);
				currentPosition.y += currentPosition.height + 2f;

				if ((flagsProperty.GetValue<int>() & (int)Axes.X) != 0)
					ShowSettingsFoldout(dataProperty.GetArrayElementAtIndex(0), "X".ToGUIContent());
				if ((flagsProperty.GetValue<int>() & (int)Axes.Y) != 0)
					ShowSettingsFoldout(dataProperty.GetArrayElementAtIndex(1), "Y".ToGUIContent());
			}
			else if (data.Property.PropertyType == typeof(Vector3))
			{
				dataProperty.EnsureCapacity(3, () => new PropertyOscillator.OscillationRangeSettings());
				EnumFlag(currentPosition, flagsProperty, "Axes".ToGUIContent(), Axes.X, Axes.Y, Axes.Z);
				currentPosition.y += currentPosition.height + 2f;

				if ((flagsProperty.GetValue<int>() & (int)Axes.X) != 0)
					ShowSettingsFoldout(dataProperty.GetArrayElementAtIndex(0), "X".ToGUIContent());
				if ((flagsProperty.GetValue<int>() & (int)Axes.Y) != 0)
					ShowSettingsFoldout(dataProperty.GetArrayElementAtIndex(1), "Y".ToGUIContent());
				if ((flagsProperty.GetValue<int>() & (int)Axes.Z) != 0)
					ShowSettingsFoldout(dataProperty.GetArrayElementAtIndex(2), "Z".ToGUIContent());
			}
			else if (data.Property.PropertyType == typeof(Vector4))
			{
				dataProperty.EnsureCapacity(4, () => new PropertyOscillator.OscillationRangeSettings());
				EnumFlag(currentPosition, flagsProperty, "Axes".ToGUIContent(), Axes.X, Axes.Y, Axes.Z, Axes.W);
				currentPosition.y += currentPosition.height + 2f;

				if ((flagsProperty.GetValue<int>() & (int)Axes.X) != 0)
					ShowSettingsFoldout(dataProperty.GetArrayElementAtIndex(0), "X".ToGUIContent());
				if ((flagsProperty.GetValue<int>() & (int)Axes.Y) != 0)
					ShowSettingsFoldout(dataProperty.GetArrayElementAtIndex(1), "Y".ToGUIContent());
				if ((flagsProperty.GetValue<int>() & (int)Axes.Z) != 0)
					ShowSettingsFoldout(dataProperty.GetArrayElementAtIndex(2), "Z".ToGUIContent());
				if ((flagsProperty.GetValue<int>() & (int)Axes.W) != 0)
					ShowSettingsFoldout(dataProperty.GetArrayElementAtIndex(3), "W".ToGUIContent());
			}
			else if (data.Property.PropertyType == typeof(Color))
			{
				dataProperty.EnsureCapacity(4, () => new PropertyOscillator.OscillationRangeSettings());
				EnumFlag(currentPosition, flagsProperty, "Channels".ToGUIContent(), Channels.R, Channels.G, Channels.B, Channels.A);
				currentPosition.y += currentPosition.height + 2f;

				if ((flagsProperty.GetValue<int>() & (int)Channels.R) != 0)
					ShowSettingsFoldout(dataProperty.GetArrayElementAtIndex(0), "R".ToGUIContent());
				if ((flagsProperty.GetValue<int>() & (int)Channels.G) != 0)
					ShowSettingsFoldout(dataProperty.GetArrayElementAtIndex(1), "G".ToGUIContent());
				if ((flagsProperty.GetValue<int>() & (int)Channels.B) != 0)
					ShowSettingsFoldout(dataProperty.GetArrayElementAtIndex(2), "B".ToGUIContent());
				if ((flagsProperty.GetValue<int>() & (int)Channels.A) != 0)
					ShowSettingsFoldout(dataProperty.GetArrayElementAtIndex(3), "A".ToGUIContent());
			}
		}

		void ShowSettings(SerializedProperty settingsProperty)
		{
			PropertyField(settingsProperty.FindPropertyRelative("WaveShape"));

			var waveShape = settingsProperty.GetValue<WaveShapes>("WaveShape");
			var frequencyProperty = settingsProperty.FindPropertyRelative("Frequency");
			var amplitudeProperty = settingsProperty.FindPropertyRelative("Amplitude");
			var centerProperty = settingsProperty.FindPropertyRelative("Center");
			var offsetProperty = settingsProperty.FindPropertyRelative("Offset");
			var ratioProperty = settingsProperty.FindPropertyRelative("Ratio");

			switch (waveShape)
			{
				case WaveShapes.Sine:
					PropertyField(frequencyProperty);
					PropertyField(amplitudeProperty);
					PropertyField(centerProperty);
					PropertyField(offsetProperty);
					offsetProperty.Clamp(0f, 1f);
					break;
				case WaveShapes.Triangle:
					PropertyField(frequencyProperty);
					PropertyField(amplitudeProperty);
					PropertyField(centerProperty);
					PropertyField(offsetProperty);
					PropertyField(ratioProperty);
					offsetProperty.Clamp(0f, 1f);
					ratioProperty.Clamp(0f, 1f);
					break;
				case WaveShapes.Sawtooth:
					PropertyField(frequencyProperty);
					PropertyField(amplitudeProperty);
					PropertyField(centerProperty);
					PropertyField(offsetProperty);
					offsetProperty.Clamp(0f, 1f);
					break;
				case WaveShapes.Square:
					PropertyField(frequencyProperty);
					PropertyField(amplitudeProperty);
					PropertyField(centerProperty);
					PropertyField(offsetProperty);
					PropertyField(ratioProperty);
					offsetProperty.Clamp(0f, 1f);
					ratioProperty.Clamp(0f, 1f);
					break;
				case WaveShapes.WhiteNoise:
					PropertyField(amplitudeProperty);
					PropertyField(centerProperty);
					break;
				case WaveShapes.PerlinNoise:
					PropertyField(frequencyProperty);
					PropertyField(amplitudeProperty);
					PropertyField(centerProperty);
					PropertyField(offsetProperty);
					offsetProperty.Clamp(0f, 1f);
					break;
			}
		}

		void ShowSettingsFoldout(SerializedProperty settingsProperty, GUIContent label)
		{
			GUI.Box(EditorGUI.IndentedRect(new Rect(currentPosition) { y = currentPosition.y + 1f, height = 14f }), GUIContent.none);
			EditorGUI.LabelField(currentPosition, label, new GUIStyle("boldLabel") { alignment = TextAnchor.MiddleCenter });
			currentPosition.y += currentPosition.height + 2f;

			EditorGUI.indentLevel++;
			ShowSettings(settingsProperty);
			EditorGUI.indentLevel--;
		}

		void UpdateProperties()
		{
			var target = currentProperty.GetValue<UnityEngine.Object>("Target");

			if (target == null)
			{
				properties = new PropertyInfo[0];
				propertyNames = new string[0];
				propertyDisplayNames = new string[0];
				return;
			}
			else
			{
				properties = target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					.Where(OscillationUtility.IsValid)
					.ToArray();
				propertyNames = properties.Convert(p => p.Name);
				propertyDisplayNames = properties.Convert(p => string.Format("{0} ({1})", p.Name, p.PropertyType.Name));
			}
		}
	}
}
