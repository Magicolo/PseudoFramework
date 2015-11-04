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
		public GravityManager GravityManager;
		public InputManager InputManager;
		public ParticleManager ParticleManager;
		public TimeManager TimeManager;

		protected override void Awake()
		{
			base.Awake();

			if (AudioManager != null && AudioManager.Find() == null)
				Instantiate(AudioManager).transform.parent = CachedTransform;

			if (GravityManager != null && GravityManager.Find() == null)
				Instantiate(GravityManager).transform.parent = CachedTransform;

			if (InputManager != null && InputManager.Find() == null)
				Instantiate(InputManager).transform.parent = CachedTransform;

			if (ParticleManager != null && ParticleManager.Find() == null)
				Instantiate(ParticleManager).transform.parent = CachedTransform;

			if (TimeManager != null && TimeManager.Find() == null)
				Instantiate(TimeManager).transform.parent = CachedTransform;

			DontDestroyOnLoad(CachedGameObject);
		}

		void Reset()
		{
			this.SetExecutionOrder(-3117);
		}
	}
}