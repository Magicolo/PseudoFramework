using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Smooth/Move")]
	public class SmoothMove : MonoBehaviourExtended, ICopyable<SmoothMove>
	{
		[Mask]
		public TransformModes Mode = TransformModes.Position;
		[Mask(Axes.XYZ)]
		public Axes Axes = Axes.XYZ;
		public Kronos.TimeChannels TimeChannel;
		public bool Culling = true;

		[Slider(BeforeSeparator = true)]
		public float Randomness;
		public Vector3 Speed = Vector3.one;

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
				float deltaTime = Kronos.GetDeltaTime(TimeChannel);

				if (Mode.Contains(TransformModes.Position))
					transform.TranslateLocal(Speed * deltaTime, Axes);

				if (Mode.Contains(TransformModes.Rotation))
					transform.RotateLocal(Speed * deltaTime, Axes);

				if (Mode.Contains(TransformModes.Scale))
					transform.ScaleLocal(Speed * deltaTime, Axes);
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