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
	[CustomPropertyDrawer(typeof(AudioOption))]
	public class AudioOptionDrawer : CustomPropertyDrawerBase
	{
		AudioOptionDrawerDummy _dummy;
		SerializedObject _dummySerialized;

		AudioOption _audioOption;
		PDynamicValue _dynamicValue;
		SerializedProperty _typeProperty;
		SerializedProperty _valueProperty;
		SerializedProperty _timeProperty;
		SerializedProperty _easeProperty;
		SerializedProperty _delayProperty;
		bool _hasCurve;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			PropertyField(property, string.Format("{0} : {1}", _typeProperty.GetValue<AudioOption.Types>(), _valueProperty.GetValue() ?? "null").ToGUIContent(), false);

			if (property.isExpanded)
			{
				EditorGUI.indentLevel++;

				EditorGUI.BeginChangeCheck();

				PropertyField(_typeProperty, GUIContent.none);

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
					_hasCurve = false;
					UpdateProperties();

					SetValue(_typeProperty.GetValue<AudioOption.Types>(), _hasCurve);
				}

				EditorGUI.BeginChangeCheck();

				ShowValue();

				if (_timeProperty != null)
					PropertyField(_timeProperty, "Time".ToGUIContent());

				if (_easeProperty != null)
					PropertyField(_easeProperty, "Ease".ToGUIContent());

				PropertyField(_delayProperty);

				if (EditorGUI.EndChangeCheck())
					SetValue(_typeProperty.GetValue<AudioOption.Types>(), _hasCurve);

				EditorGUI.indentLevel--;
			}

			End();
		}

		public override void Initialize(SerializedProperty property, GUIContent label)
		{
			base.Initialize(property, label);

			_dummy = ScriptableObject.CreateInstance<AudioOptionDrawerDummy>();
			_dummy.hideFlags = HideFlags.DontSave;
			_dummySerialized = new SerializedObject(_dummy);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			base.GetPropertyHeight(property, label);

			_audioOption = property.GetValue<AudioOption>();
			_dynamicValue = _audioOption.Value;
			_hasCurve = _audioOption.HasCurve();
			_typeProperty = property.FindPropertyRelative("_type");
			_delayProperty = property.FindPropertyRelative("_delay");

			UpdateProperties();

			InitializeValue(_typeProperty.GetValue<AudioOption.Types>());

			float height = 16f;

			if (property.isExpanded)
			{
				height += 38f + EditorGUI.GetPropertyHeight(_valueProperty, label, true);

				if (_timeProperty != null)
					height += EditorGUI.GetPropertyHeight(_timeProperty) + 2f;
				if (_easeProperty != null)
					height += EditorGUI.GetPropertyHeight(_easeProperty) + 2f;
			}

			return height;
		}

		void UpdateProperties()
		{
			_valueProperty = GetValueProperty(_typeProperty.GetValue<AudioOption.Types>(), _hasCurve, _dummySerialized);
			_timeProperty = GetTimeProperty(_typeProperty.GetValue<AudioOption.Types>(), _dummySerialized);
			_easeProperty = GetEaseProperty(_typeProperty.GetValue<AudioOption.Types>(), _dummySerialized);
		}

		void ShowValue()
		{
			if (CanHaveCurve(_typeProperty.GetValue<AudioOption.Types>()))
			{
				EditorGUI.BeginChangeCheck();

				_hasCurve = ToggleButton(new Rect(currentPosition.x + currentPosition.width - 16f, currentPosition.y + 1f, 16f, 14f), _hasCurve, "C".ToGUIContent(), "C".ToGUIContent());

				if (EditorGUI.EndChangeCheck())
					UpdateProperties();

				EditorGUI.PropertyField(new Rect(currentPosition.x, currentPosition.y, currentPosition.width - 20f, currentPosition.height), _valueProperty, "Value".ToGUIContent());
			}
			else
				EditorGUI.PropertyField(currentPosition, _valueProperty, "Value".ToGUIContent());

			currentPosition.y += EditorGUI.GetPropertyHeight(_valueProperty) + 2f;
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
				curveProperty = property.FindPropertyRelative("_curve");

			return curveProperty;
		}

		bool CanHaveCurve(AudioOption.Types type)
		{
			return type == AudioOption.Types.SpatialBlend || type == AudioOption.Types.ReverbZoneMix || type == AudioOption.Types.Spread || type == AudioOption.Types.RolloffMode;
		}

		void InitializeValue(AudioOption.Types type)
		{
			if (_dynamicValue.GetValueType() == PDynamicValue.ValueTypes.Null && _dynamicValue.GetValue() == null)
				_dynamicValue.SetValue(AudioOption.GetDefaultValue(type));

			if (type == AudioOption.Types.VolumeScale || type == AudioOption.Types.PitchScale)
			{
				float[] data = _audioOption.GetValue<float[]>();
				data = data == null || data.Length != 3 ? AudioOption.GetDefaultValue(type) as float[] : data;

				_valueProperty.SetValue(data[0]);
				_timeProperty.SetValue(data[1]);
				_easeProperty.SetValue((Tweening.Ease)data[2]);
			}
			else if (type == AudioOption.Types.FadeIn || type == AudioOption.Types.FadeOut)
			{
				float[] data = _audioOption.GetValue<float[]>();
				data = data == null || data.Length != 2 ? AudioOption.GetDefaultValue(type) as float[] : data;

				_valueProperty.SetValue(data[0]);
				_easeProperty.SetValue((Tweening.Ease)data[1]);
			}
			else
				_valueProperty.SetValue(_dynamicValue.GetValue());
		}

		void SetValue(AudioOption.Types type, bool hasCurve)
		{
			serializedObject.ApplyModifiedProperties();

			PDynamicValue.ValueTypes valueType;
			bool isArray;

			AudioOption.ToValueType(type, hasCurve, out valueType, out isArray);
			_dynamicValue.SetValueType(valueType, isArray);

			if (type == AudioOption.Types.VolumeScale || type == AudioOption.Types.PitchScale)
				_dynamicValue.SetValue(new float[] { _valueProperty.GetValue<float>(), _timeProperty.GetValue<float>(), (float)_easeProperty.GetValue<Tweening.Ease>() });
			else if (type == AudioOption.Types.FadeIn || type == AudioOption.Types.FadeOut)
				_dynamicValue.SetValue(new float[] { _valueProperty.GetValue<float>(), (float)_easeProperty.GetValue<Tweening.Ease>() });
			else
				_dynamicValue.SetValue(_valueProperty.GetValue());

			serializedObject.Update();
		}
	}
}