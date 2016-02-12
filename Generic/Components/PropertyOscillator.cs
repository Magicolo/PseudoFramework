using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Internal.Oscillation;
using Pseudo.Internal.Pool;

namespace Pseudo
{
	public class PropertyOscillator : ComponentBehaviour
	{
		[Serializable]
		public class OscillationRangeSettings
		{
			public WaveShapes WaveShape;
			public MinMax Frequency = new MinMax(1f, 2f);
			public MinMax Amplitude = new MinMax(0f, 1f);
			public MinMax Center = new MinMax(0f, 0f);
			public MinMax Offset = new MinMax(0f, 100f);
			public MinMax Ratio = new MinMax(0.5f, 0.5f);

			public static implicit operator OscillationSettings(OscillationRangeSettings settings)
			{
				return new OscillationSettings
				{
					WaveShape = settings.WaveShape,
					Frequency = settings.Frequency.GetRandom(),
					Amplitude = settings.Amplitude.GetRandom(),
					Center = settings.Center.GetRandom(),
					Offset = settings.Offset.GetRandom(),
					Ratio = settings.Ratio.GetRandom(),
				};
			}
		}

		[Serializable]
		public class OscillatorData : IPoolSettersInitializable, ISerializationCallbackReceiver
		{
			public UnityEngine.Object Target;
			public string PropertyName;
			public int Flags;
			public OscillationRangeSettings[] Settings = new OscillationRangeSettings[0];

			public PropertyInfo Property
			{
				get { return property; }
			}

			PropertyInfo property;
			IOscillator oscillator;
			OscillationSettings[] settings;

			public void Initialize()
			{
				if (Target == null || string.IsNullOrEmpty(PropertyName))
					property = null;
				else if (property == null || property.Name != PropertyName || property.DeclaringType != Target.GetType())
					property = Target.GetType().GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

				if (property == null)
					oscillator = null;
				else if (oscillator == null)
					oscillator = OscillationUtility.GetOscillator(property);

				if (settings == null || settings.Length != Settings.Length)
					settings = new OscillationSettings[Settings.Length];

				for (int i = 0; i < Settings.Length; i++)
					settings[i] = Settings[i];
			}

			public void Update(float time)
			{
				if (oscillator == null)
					return;

				oscillator.Oscillate(Target, settings, Flags, time);
			}

			void IPoolSettersInitializable.OnPrePoolSettersInitialize()
			{
				Initialize();
			}

			void IPoolSettersInitializable.OnPostPoolSettersInitialize(List<IPoolSetter> setters) { }

			void ISerializationCallbackReceiver.OnBeforeSerialize() { }

			void ISerializationCallbackReceiver.OnAfterDeserialize()
			{
				Initialize();
			}
		}

		[InitializeContent]
		public OscillatorData[] Oscillators = new OscillatorData[0];
		public TimeComponent Time;

		[Message(ComponentMessages.OnAdded)]
		void OnAdd()
		{
			Initialize();
		}

		void Initialize()
		{
			for (int i = 0; i < Oscillators.Length; i++)
				Oscillators[i].Initialize();

			Update();
		}

		void Update()
		{
			for (int i = 0; i < Oscillators.Length; i++)
				Oscillators[i].Update(Time.Time);
		}
	}
}