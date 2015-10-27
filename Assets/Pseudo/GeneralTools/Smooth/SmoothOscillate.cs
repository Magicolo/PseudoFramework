﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Smooth/Oscillate")]
	public class SmoothOscillate : PMonoBehaviour, ICopyable<SmoothOscillate>
	{
		[Mask]
		public TransformModes Mode = TransformModes.Position;
		[Mask(Axes.XYZ)]
		public Axes Axes = Axes.XYZ;
		public PTime.TimeChannels TimeChannel;
		public bool Culling = true;

		[Slider(BeforeSeparator = true)]
		public float FrequencyRandomness;
		public Vector3 Frequency = Vector3.one;

		[Slider(BeforeSeparator = true)]
		public float AmplitudeRandomness;
		public Vector3 Amplitude = Vector3.one;

		[Slider(BeforeSeparator = true)]
		public float CenterRandomness;
		public Vector3 Center;

		[DoNotCopy]
		bool _rendererCached;
		[DoNotCopy]
		Renderer _renderer;
		public Renderer Renderer
		{
			get
			{
				_renderer = _rendererCached ? _renderer : GetComponent<Renderer>();
				_rendererCached = true;
				return _renderer;
			}
		}

		void Awake()
		{
			ApplyRandomness();
		}

		void Update()
		{
			if (Mode == TransformModes.None || Axes == Axes.None)
				return;

			if (!Culling || Renderer.isVisible)
			{
				if ((Mode & TransformModes.Position) != 0)
					CachedTransform.OscillateLocalPosition(Frequency, Amplitude, Center, PTime.GetTime(TimeChannel), Axes);

				if ((Mode & TransformModes.Rotation) != 0)
					CachedTransform.OscillateLocalEulerAngles(Frequency, Amplitude, Center, PTime.GetTime(TimeChannel), Axes);

				if ((Mode & TransformModes.Scale) != 0)
					CachedTransform.OscillateLocalScale(Frequency, Amplitude, Center, PTime.GetTime(TimeChannel), Axes);
			}
		}

		public void ApplyRandomness()
		{
			Frequency += Frequency.SetValues(new Vector3(UnityEngine.Random.Range(-FrequencyRandomness * Frequency.x, FrequencyRandomness * Frequency.x), UnityEngine.Random.Range(-FrequencyRandomness * Frequency.y, FrequencyRandomness * Frequency.y), UnityEngine.Random.Range(-FrequencyRandomness * Frequency.z, FrequencyRandomness * Frequency.z)), Axes);
			Amplitude += Amplitude.SetValues(new Vector3(UnityEngine.Random.Range(-AmplitudeRandomness * Amplitude.x, AmplitudeRandomness * Amplitude.x), UnityEngine.Random.Range(-AmplitudeRandomness * Amplitude.y, AmplitudeRandomness * Amplitude.y), UnityEngine.Random.Range(-AmplitudeRandomness * Amplitude.z, AmplitudeRandomness * Amplitude.z)), Axes);
			Center += Center.SetValues(new Vector3(UnityEngine.Random.Range(-CenterRandomness * Center.x, CenterRandomness * Center.x), UnityEngine.Random.Range(-CenterRandomness * Center.y, CenterRandomness * Center.y), UnityEngine.Random.Range(-CenterRandomness * Center.z, CenterRandomness * Center.z)), Axes);
		}

		public void Copy(SmoothOscillate reference)
		{
			Mode = reference.Mode;
			Axes = reference.Axes;
			TimeChannel = reference.TimeChannel;
			Culling = reference.Culling;
			FrequencyRandomness = reference.FrequencyRandomness;
			Frequency = reference.Frequency;
			AmplitudeRandomness = reference.AmplitudeRandomness;
			Amplitude = reference.Amplitude;
			CenterRandomness = reference.CenterRandomness;
			Center = reference.Center;
		}
	}
}