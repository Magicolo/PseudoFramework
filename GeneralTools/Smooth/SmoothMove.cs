using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Smooth/Move"), Copy]
	public class SmoothMove : PMonoBehaviour, ICopyable<SmoothMove>
	{
		[Mask]
		public TransformModes Mode = TransformModes.Position;
		[Mask(Axes.XYZ)]
		public Axes Axes = Axes.XYZ;
		public TimeManager.TimeChannels TimeChannel;
		public bool Culling = true;

		[Slider(BeforeSeparator = true)]
		public float Randomness;
		public Vector3 Speed = Vector3.one;

		readonly CachedValue<Renderer> cachedRenderer;
		public Renderer CachedRenderer { get { return cachedRenderer; } }

		public SmoothMove()
		{
			cachedRenderer = new CachedValue<Renderer>(GetComponent<Renderer>);
		}

		void Awake()
		{
			ApplyRandomness();
		}

		void Update()
		{
			if (Mode == TransformModes.None || Axes == Axes.None)
				return;

			if (!Culling || CachedRenderer.isVisible)
			{
				float deltaTime = TimeManager.GetDeltaTime(TimeChannel);

				if ((Mode & TransformModes.Position) != 0)
					CachedTransform.TranslateLocal(Speed * deltaTime, Axes);

				if ((Mode & TransformModes.Rotation) != 0)
					CachedTransform.RotateLocal(Speed * deltaTime, Axes);

				if ((Mode & TransformModes.Scale) != 0)
					CachedTransform.ScaleLocal(Speed * deltaTime, Axes);
			}
		}

		public void ApplyRandomness()
		{
			Speed += Speed.SetValues(new Vector3(UnityEngine.Random.Range(-Randomness * Speed.x, Randomness * Speed.x), UnityEngine.Random.Range(-Randomness * Speed.y, Randomness * Speed.y), UnityEngine.Random.Range(-Randomness * Speed.z, Randomness * Speed.z)), Axes);
		}

		public void Copy(SmoothMove reference)
		{
			Mode = reference.Mode;
			Axes = reference.Axes;
			TimeChannel = reference.TimeChannel;
			Culling = reference.Culling;
			Randomness = reference.Randomness;
			Speed = reference.Speed;
		}
	}
}