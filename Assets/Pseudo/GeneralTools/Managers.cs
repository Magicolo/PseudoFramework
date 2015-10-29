using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class Managers : Singleton<Managers>
	{
		public AudioManager AudioManager;
		public InputManager InputManager;
		public ParticleManager ParticleManager;
		public PoolManager PoolManager;
		public TimeManager TimeManager;

		protected override void Awake()
		{
			base.Awake();

			if (AudioManager.Instance == null && AudioManager != null)
				Instantiate(AudioManager).transform.parent = CachedTransform;

			if (InputManager.Instance == null && InputManager != null)
				Instantiate(InputManager).transform.parent = CachedTransform;

			if (ParticleManager.Instance == null && ParticleManager != null)
				Instantiate(ParticleManager).transform.parent = CachedTransform;

			if (PoolManager.Instance == null && PoolManager != null)
				Instantiate(PoolManager).transform.parent = CachedTransform;

			if (TimeManager.Instance == null && TimeManager != null)
				Instantiate(TimeManager).transform.parent = CachedTransform;

			DontDestroyOnLoad(CachedGameObject);
		}

		void Reset()
		{
			this.SetExecutionOrder(-3117);
		}
	}
}