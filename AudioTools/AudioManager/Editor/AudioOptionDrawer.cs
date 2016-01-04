﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using UnityEditor;
using Pseudo.Internal.Editor;

namespace Pseudo.Internal.Audio
{
	[CustomPropertyDrawer(typeof(AudioOption))]
	public class AudioOptionDrawer : CustomPropertyDrawerBase
	{
		AudioOptionDrawerDummy dummy;
		SerializedObject dummySerialized;

		AudioOption audioOption;
		DynamicValue dynamicValue;
		SerializedProperty typeProperty;
		SerializedProperty valueProperty;
		SerializedProperty timeProperty;
		SerializedProperty easeProperty;
		SerializedProperty delayProperty;
		bool hasCurve;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			PropertyField(property, string.Format("{0} : {1}", typeProperty.GetValue<AudioOption.Types>(), valueProperty.GetValue() ?? "null").ToGUIContent(), false);

			if (property.isExpanded)
			{
				EditorGUI.indentLevel++;

				EditorGUI.BeginChangeCheck();

				PropertyField(typeProperty, GUIContent.none);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					hasCurve = false;
					UpdateProperties();

					SetValue(typeProperty.GetValue<AudioOption.Types>(), hasCurve);
				}

				EditorGUI.BeginChangeCheck();

				ShowValue();

				if (timeProperty != null)
					PropertyField(timeProperty, "Time".ToGUIContent());

				if (easeProperty != null)
					PropertyField(easeProperty, "Ease".ToGUIContent());

				PropertyField(delayProperty);

				if (EditorGUI.EndChangeCheck())
					SetValue(typeProperty.GetValue<AudioOption.Types>(), hasCurve);

				EditorGUI.indentLevel--;
			}

			End();
		}

		public override void Initialize(SerializedProperty property, GUIContent label)
		{
			base.Initialize(property, label);

			dummy = ScriptableObject.CreateInstance<AudioOptionDrawerDummy>();
			dummy.hideFlags = HideFlags.DontSave;
			dummySerialized = new SerializedObject(dummy);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			audioOption = property.GetValue<AudioOption>();
			dynamicValue = audioOption.Value;
			hasCurve = audioOption.HasCurve();
			typeProperty = property.FindPropertyRelative("type");
			delayProperty = property.FindPropertyRelative("delay");

			UpdateProperties();

			InitializeValue(typeProperty.GetValue<AudioOption.Types>());

			float height = 16f;

			if (property.isExpanded)
			{
				height += 38f + EditorGUI.GetPropertyHeight(valueProperty, label, true);

				if (timeProperty != null)
					height += EditorGUI.GetPropertyHeight(timeProperty) + 2f;
				if (easeProperty != null)
					height += EditorGUI.GetPropertyHeight(easeProperty) + 2f;
			}

			return height;
		}

		void UpdateProperties()
		{
			valueProperty = GetValueProperty(typeProperty.GetValue<AudioOption.Types>(), hasCurve, dummySerialized);
			timeProperty = GetTimeProperty(typeProperty.GetValue<AudioOption.Types>(), dummySerialized);
			easeProperty = GetEaseProperty(typeProperty.GetValue<AudioOption.Types>(), dummySerialized);
		}

		void ShowValue()
		{
			if (CanHaveCurve(typeProperty.GetValue<AudioOption.Types>()))
			{
				EditorGUI.BeginChangeCheck();

				hasCurve = ToggleButton(new Rect(currentPosition.x + currentPosition.width - 16f, currentPosition.y + 1f, 16f, 14f), hasCurve, "C".ToGUIContent(), "C".ToGUIContent());

				if (EditorGUI.EndChangeCheck())
					UpdateProperties();

				EditorGUI.PropertyField(new Rect(currentPosition.x, currentPosition.y, currentPosition.width - 20f, currentPosition.height), valueProperty, "Value".ToGUIContent());
			}
			else
				EditorGUI.PropertyField(currentPosition, valueProperty, "Value".ToGUIContent());

			currentPosition.y += EditorGUI.GetPropertyHeight(valueProperty) + 2f;
		}

		SerializedProperty GetValueProperty(AudioOption.Types type, bool hasCurve, SerializedObject dummy)
		{
			string propertyName = type.ToString() + (hasCurve ? "Curve" : "");

			SerializedProperty valueProperty = dummy.FindProperty(propertyName);

			return valueProperty;
		}

		SerializedProperty GetTimeProperty(AudioOption.Types type, SerializedObject dummy)
		{
			SerializedProperty timeProperty = null;

			if (type == AudioOption.Types.VolumeScale || type == AudioOption.Types.PitchScale)
				timeProperty = dummy.FindProperty("EaseTime");

			return timeProperty;
		}

		SerializedProperty GetEaseProperty(AudioOption.Types type, SerializedObject dummy)
		{
			SerializedProperty easeProperty = null;

			if (type == AudioOption.Types.VolumeScale || type == AudioOption.Types.PitchScale || type == AudioOption.Types.FadeIn || type == AudioOption.Types.FadeOut)
				easeProperty = dummy.FindProperty("EaseType");

			return easeProperty;
		}

		SerializedProperty GetCurveProperty(AudioOption.Types type, object value, SerializedProperty property)
		{
			SerializedProperty curveProperty = null;

			if (type == AudioOption.Types.SpatialBlend || type == AudioOption.Types.ReverbZoneMix || type == AudioOption.Types.Spread || (type == AudioOption.Types.RolloffMode && (AudioRolloffMode)value == AudioRolloffMode.Custom))
				curveProperty = property.FindPropertyRelative("curve");

			return curveProperty;
		}

		bool CanHaveCurve(AudioOption.Types type)
		{
			return type == AudioOption.Types.SpatialBlend || type == AudioOption.Types.ReverbZoneMix || type == AudioOption.Types.Spread || type == AudioOption.Types.RolloffMode;
		}

		void InitializeValue(AudioOption.Types type)
		{
			if (dynamicValue.GetValueType() == DynamicValue.ValueTypes.Null && dynamicValue.GetValue() == null)
				dynamicValue.SetValue(AudioOption.GetDefaultValue(type));

			if (type == AudioOption.Types.VolumeScale || type == AudioOption.Types.PitchScale)
			{
				float[] data = audioOption.GetValue<float[]>();
				data = data == null || data.Length != 3 ? AudioOption.GetDefaultValue(type) as float[] : data;

				valueProperty.SetValue(data[0]);
				timeProperty.SetValue(data[1]);
				easeProperty.SetValue((Tweening.Ease)data[2]);
			}
			else if (type == AudioOption.Types.FadeIn || type == AudioOption.Types.FadeOut)
			{
				float[] data = audioOption.GetValue<float[]>();
				data = data == null || data.Length != 2 ? AudioOption.GetDefaultValue(type) as float[] : data;

				valueProperty.SetValue(data[0]);
				easeProperty.SetValue((Tweening.Ease)data[1]);
			}
			else
				valueProperty.SetValue(dynamicValue.GetValue());
		}

		void SetValue(AudioOption.Types type, bool hasCurve)
		{
			serializedObject.ApplyModifiedProperties();

			DynamicValue.ValueTypes valueType;
			bool isArray;

			AudioOption.ToValueType(type, hasCurve, out valueType, out isArray);
			dynamicValue.SetValueType(valueType, isArray);

			if (type == AudioOption.Types.VolumeScale || type == AudioOption.Types.PitchScale)
				dynamicValue.SetValue(new float[] { valueProperty.GetValue<float>(), timeProperty.GetValue<float>(), (float)easeProperty.GetValue<Tweening.Ease>() });
			else if (type == AudioOption.Types.FadeIn || type == AudioOption.Types.FadeOut)
				dynamicValue.SetValue(new float[] { valueProperty.GetValue<float>(), (float)easeProperty.GetValue<Tweening.Ease>() });
			else
				dynamicValue.SetValue(valueProperty.GetValue());

			serializedObject.Update();
		}
	}
}