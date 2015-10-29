using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using UnityEngine.Audio;

namespace Pseudo
{
	[Serializable]
	public class AudioOption : IPoolable, ICopyable<AudioOption>
	{
		public enum Types
		{
			VolumeScale,
			PitchScale,
			RandomVolume,
			RandomPitch,
			FadeIn,
			FadeOut,
			Loop,
			Clip,
			Output,
			Mute,
			Spatialize,
			BypassEffects,
			BypassListenerEffects,
			BypassReverbZones,
			Priority,
			StereoPan,
			DopplerLevel,
			MinDistance,
			MaxDistance,
			SpatialBlend,
			ReverbZoneMix,
			Spread,
			RolloffMode,
			PlayRange,
			Time,
			TimeSamples,
			VelocityUpdateMode,
			IgnoreListenerPause,
			IgnoreListenerVolume
		}

		[SerializeField]
		Types type;
		[SerializeField]
		DynamicValue value;
		[SerializeField, Min]
		float delay;

		public Types Type { get { return type; } }
		public DynamicValue Value { get { return value; } }
		public float Delay { get { return delay; } }

		public static readonly AudioOption Default = new AudioOption();

		public static AudioOption Clip(AudioClip clip, float delay = 0f)
		{
			return Create(Types.Clip, clip, delay);
		}

		public static AudioOption Output(AudioMixerGroup mixerGroup, float delay = 0f)
		{
			return Create(Types.Output, mixerGroup, delay);
		}

		public static AudioOption FadeIn(float fadeIn, Tweening.Ease ease = Tweening.Ease.OutQuad, float delay = 0f)
		{
			return Create(Types.FadeIn, new[] { fadeIn, (float)ease }, delay);
		}

		public static AudioOption FadeOut(float fadeOut, Tweening.Ease ease = Tweening.Ease.InQuad, float delay = 0f)
		{
			return Create(Types.FadeOut, new[] { fadeOut, (float)ease }, delay);
		}

		public static AudioOption VolumeScale(float volume, float time = 0f, Tweening.Ease ease = Tweening.Ease.Linear, float delay = 0f)
		{
			return Create(Types.VolumeScale, new[] { volume, time, (float)ease }, delay);
		}

		public static AudioOption PitchScale(float pitch, float time = 0f, Tweening.Ease ease = Tweening.Ease.Linear, float delay = 0f)
		{
			return Create(Types.PitchScale, new[] { pitch, time, (float)ease }, delay);
		}

		public static AudioOption RandomVolume(float randomVolume, float delay = 0f)
		{
			return Create(Types.RandomVolume, randomVolume, delay);
		}

		public static AudioOption RandomPitch(float randomPitch, float delay = 0f)
		{
			return Create(Types.RandomPitch, randomPitch, delay);
		}

		public static AudioOption Loop(bool loop, float delay = 0f)
		{
			return Create(Types.Loop, loop, delay);
		}

		public static AudioOption DopplerLevel(float dopplerLevel, float delay = 0f)
		{
			return Create(Types.DopplerLevel, dopplerLevel, delay);
		}

		public static AudioOption RolloffMode(AudioRolloffMode rolloff, float delay = 0f)
		{
			return Create(Types.RolloffMode, (int)rolloff, delay);
		}

		public static AudioOption RolloffMode(AnimationCurve rolloff, float delay = 0f)
		{
			return Create(Types.RolloffMode, rolloff, delay);
		}

		public static AudioOption MinDistance(float minDistance, float delay = 0f)
		{
			return Create(Types.MinDistance, minDistance, delay);
		}

		public static AudioOption MaxDistance(float maxDistance, float delay = 0f)
		{
			return Create(Types.MaxDistance, maxDistance, delay);
		}

		public static AudioOption Spread(float spread, float delay = 0f)
		{
			return Create(Types.Spread, spread, delay);
		}

		public static AudioOption Spread(AnimationCurve spread, float delay = 0f)
		{
			return Create(Types.Spread, spread, delay);
		}

		public static AudioOption Mute(bool mute, float delay = 0f)
		{
			return Create(Types.Mute, mute, delay);
		}

		public static AudioOption BypassEffects(bool bypass, float delay = 0f)
		{
			return Create(Types.BypassEffects, bypass, delay);
		}

		public static AudioOption BypassListenerEffects(bool bypass, float delay = 0f)
		{
			return Create(Types.BypassListenerEffects, bypass, delay);
		}

		public static AudioOption BypassReverbZones(bool bypass, float delay = 0f)
		{
			return Create(Types.BypassReverbZones, bypass, delay);
		}

		public static AudioOption Priority(int priority, float delay = 0f)
		{
			return Create(Types.Priority, priority, delay);
		}

		public static AudioOption StereoPan(float stereoPan, float delay = 0f)
		{
			return Create(Types.StereoPan, stereoPan, delay);
		}

		public static AudioOption SpatialBlend(float spatialBlend, float delay = 0f)
		{
			return Create(Types.SpatialBlend, spatialBlend, delay);
		}

		public static AudioOption SpatialBlend(AnimationCurve spatialBlend, float delay = 0f)
		{
			return Create(Types.SpatialBlend, spatialBlend, delay);
		}

		public static AudioOption ReverbZoneMix(float reverbZoneMix, float delay = 0f)
		{
			return Create(Types.ReverbZoneMix, reverbZoneMix, delay);
		}

		public static AudioOption ReverbZoneMix(AnimationCurve reverbZoneMix, float delay = 0f)
		{
			return Create(Types.ReverbZoneMix, reverbZoneMix, delay);
		}

		public static AudioOption PlayRange(float start, float end, float delay = 0f)
		{
			return Create(Types.Time, new Vector2(start, end), delay);
		}

		public static AudioOption Time(float time, float delay = 0f)
		{
			return Create(Types.Time, time, delay);
		}

		public static AudioOption TimeSamples(int timeSamples, float delay = 0f)
		{
			return Create(Types.TimeSamples, timeSamples, delay);
		}

		public static AudioOption VelocityUpdateMode(AudioVelocityUpdateMode updateMode, float delay = 0f)
		{
			return Create(Types.VelocityUpdateMode, (int)updateMode, delay);
		}

		public static AudioOption IgnoreListenerPause(bool ignore, float delay = 0f)
		{
			return Create(Types.IgnoreListenerPause, ignore, delay);
		}

		public static AudioOption IgnoreListenerVolume(bool ignore, float delay = 0f)
		{
			return Create(Types.IgnoreListenerVolume, ignore, delay);
		}

		public static AudioOption Spatialize(bool spatialize, float delay = 0f)
		{
			return Create(Types.Spatialize, spatialize, delay);
		}

		static AudioOption Create(Types type, object value, float delay = 0f)
		{
			AudioOption option = Pool<AudioOption>.Create(Default);

			option.Initialize(type, value, delay);

			return option;
		}

		public object GetValue()
		{
			DynamicValue.ValueTypes valueType;
			bool isArray;

			ToValueType(type, HasCurve(), out valueType, out isArray);

			if (value.GetValueType() != valueType || value.IsArray != isArray)
				value.SetValue(GetDefaultValue(type));

			return value.GetValue();
		}

		public T GetValue<T>()
		{
			return (T)GetValue();
		}

		public bool HasCurve()
		{
			if (type == Types.SpatialBlend || type == Types.ReverbZoneMix || type == Types.Spread || type == Types.RolloffMode)
				return value.GetValue() is AnimationCurve;
			else
				return false;
		}

		public void Initialize(Types type, object value, float delay = 0f)
		{
			this.type = type;
			this.value = Pool<DynamicValue>.Create(this.value);
			this.value.SetValue(value);
			this.delay = delay;
		}

		public void OnCreate()
		{
			value = Pool<DynamicValue>.Create(value);
		}

		public void OnRecycle()
		{
			Pool<DynamicValue>.Recycle(ref value);
		}

		public void Copy(AudioOption reference)
		{
			type = reference.type;
			value = reference.value;
			delay = reference.delay;
		}

		public override string ToString()
		{
			return string.Format("{0}({1}, {2})", GetType().Name, type, value);
		}

		public static object GetDefaultValue(Types type, bool hasCurve = false)
		{
			object defaultValue = null;

			switch (type)
			{
				case Types.VolumeScale:
					defaultValue = new float[] { 1f, 0f, 0f };
					break;
				case Types.PitchScale:
					defaultValue = new float[] { 1f, 0f, 0f };
					break;
				case Types.RandomVolume:
					defaultValue = 0f;
					break;
				case Types.RandomPitch:
					defaultValue = 0f;
					break;
				case Types.FadeIn:
					defaultValue = new float[] { 0f, 0f };
					break;
				case Types.FadeOut:
					defaultValue = new float[] { 0f, 0f };
					break;
				case Types.Loop:
					defaultValue = false;
					break;
				case Types.Clip:
					defaultValue = null;
					break;
				case Types.Output:
					defaultValue = null;
					break;
				case Types.DopplerLevel:
					defaultValue = 1f;
					break;
				case Types.RolloffMode:
					if (hasCurve)
						defaultValue = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
					else
						defaultValue = AudioRolloffMode.Logarithmic;
					break;
				case Types.MinDistance:
					defaultValue = 1f;
					break;
				case Types.MaxDistance:
					defaultValue = 500f;
					break;
				case Types.Spread:
					if (hasCurve)
						defaultValue = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
					else
						defaultValue = 0f;
					break;
				case Types.Mute:
					defaultValue = false;
					break;
				case Types.BypassEffects:
					defaultValue = false;
					break;
				case Types.BypassListenerEffects:
					defaultValue = false;
					break;
				case Types.BypassReverbZones:
					defaultValue = false;
					break;
				case Types.Priority:
					defaultValue = 128;
					break;
				case Types.StereoPan:
					defaultValue = 0f;
					break;
				case Types.SpatialBlend:
					if (hasCurve)
						defaultValue = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
					else
						defaultValue = 0f;
					break;
				case Types.ReverbZoneMix:
					if (hasCurve)
						defaultValue = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
					else
						defaultValue = 1f;
					break;
				case Types.PlayRange:
					defaultValue = Vector2.zero;
					break;
				case Types.Time:
					defaultValue = 0f;
					break;
				case Types.TimeSamples:
					defaultValue = 0;
					break;
				case Types.VelocityUpdateMode:
					defaultValue = AudioVelocityUpdateMode.Auto;
					break;
				case Types.IgnoreListenerPause:
					defaultValue = false;
					break;
				case Types.IgnoreListenerVolume:
					defaultValue = false;
					break;
				case Types.Spatialize:
					defaultValue = false;
					break;
			}

			return defaultValue;
		}

		public static void ToValueType(Types type, bool hasCurve, out DynamicValue.ValueTypes valueType, out bool isArray)
		{
			valueType = DynamicValue.ValueTypes.Null;
			isArray = false;

			switch (type)
			{
				case Types.VolumeScale:
					valueType = DynamicValue.ValueTypes.Float;
					isArray = true;
					break;
				case Types.PitchScale:
					valueType = DynamicValue.ValueTypes.Float;
					isArray = true;
					break;
				case Types.RandomVolume:
					valueType = DynamicValue.ValueTypes.Float;
					break;
				case Types.RandomPitch:
					valueType = DynamicValue.ValueTypes.Float;
					break;
				case Types.FadeIn:
					valueType = DynamicValue.ValueTypes.Float;
					isArray = true;
					break;
				case Types.FadeOut:
					valueType = DynamicValue.ValueTypes.Float;
					isArray = true;
					break;
				case Types.Loop:
					valueType = DynamicValue.ValueTypes.Bool;
					break;
				case Types.Clip:
					valueType = DynamicValue.ValueTypes.Object;
					break;
				case Types.Output:
					valueType = DynamicValue.ValueTypes.Object;
					break;
				case Types.DopplerLevel:
					valueType = DynamicValue.ValueTypes.Float;
					break;
				case Types.RolloffMode:
					valueType = hasCurve ? DynamicValue.ValueTypes.AnimationCurve : DynamicValue.ValueTypes.Int;
					break;
				case Types.MinDistance:
					valueType = DynamicValue.ValueTypes.Float;
					break;
				case Types.MaxDistance:
					valueType = DynamicValue.ValueTypes.Float;
					break;
				case Types.Spread:
					valueType = hasCurve ? DynamicValue.ValueTypes.AnimationCurve : DynamicValue.ValueTypes.Float;
					break;
				case Types.Mute:
					valueType = DynamicValue.ValueTypes.Bool;
					break;
				case Types.BypassEffects:
					valueType = DynamicValue.ValueTypes.Bool;
					break;
				case Types.BypassListenerEffects:
					valueType = DynamicValue.ValueTypes.Bool;
					break;
				case Types.BypassReverbZones:
					valueType = DynamicValue.ValueTypes.Bool;
					break;
				case Types.Priority:
					valueType = DynamicValue.ValueTypes.Int;
					break;
				case Types.StereoPan:
					valueType = DynamicValue.ValueTypes.Float;
					break;
				case Types.SpatialBlend:
					valueType = hasCurve ? DynamicValue.ValueTypes.AnimationCurve : DynamicValue.ValueTypes.Float;
					break;
				case Types.ReverbZoneMix:
					valueType = hasCurve ? DynamicValue.ValueTypes.AnimationCurve : DynamicValue.ValueTypes.Float;
					break;
				case Types.PlayRange:
					valueType = DynamicValue.ValueTypes.Vector2;
					break;
				case Types.Time:
					valueType = DynamicValue.ValueTypes.Float;
					break;
				case Types.TimeSamples:
					valueType = DynamicValue.ValueTypes.Int;
					break;
				case Types.VelocityUpdateMode:
					valueType = DynamicValue.ValueTypes.Int;
					break;
				case Types.IgnoreListenerPause:
					valueType = DynamicValue.ValueTypes.Bool;
					break;
				case Types.IgnoreListenerVolume:
					valueType = DynamicValue.ValueTypes.Bool;
					break;
				case Types.Spatialize:
					valueType = DynamicValue.ValueTypes.Bool;
					break;
			}
		}
	}
}