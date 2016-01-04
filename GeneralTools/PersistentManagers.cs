using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class PersistentManagers : Singleton<PersistentManagers>
	{
		public AudioManager AudioManager;
		public GravityManager GravityManager;
		public InputManager InputManager;
		public ParticleManager ParticleManager;
		public TimeManager TimeManager;

		protected override void Awake()
		{
			if (Instance != null)
			{
				CachedGameObject.Destroy();
				return;
			}

			base.Awake();

			CreateManager(AudioManager);
			CreateManager(GravityManager);
			CreateManager(InputManager);
			CreateManager(ParticleManager);
			CreateManager(TimeManager);
			DontDestroyOnLoad(CachedGameObject);
		}

		public Singleton<T> CreateManager<T>(Singleton<T> managerPrefab) where T : Singleton<T>
		{
			if (managerPrefab == null)
				return null;

			Singleton<T> manager = Singleton<T>.Find();

			if (manager == null)
			{
				manager = Instantiate(managerPrefab);
				manager.CachedTransform.parent = CachedTransform;
			}

			return manager;
		}

		public void DestroyManager<T>() where T : Singleton<T>
		{
			Singleton<T> manager = Singleton<T>.Find();

			if (manager != null)
				manager.Destroy();
		}

		protected virtual void Reset()
		{
			this.SetExecutionOrder(-3117);
		}
	}
}