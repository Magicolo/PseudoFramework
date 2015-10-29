using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[RequireComponent(typeof(ParticleSystem))]
	public class ParticleEffect : PMonoBehaviour, IPoolable
	{
		readonly CachedValue<ParticleSystem> cachedParticleSystem;

		public ParticleSystem CachedParticleSystem { get { return cachedParticleSystem; } }
		public bool IsAlive { get { return cachedParticleSystem.Value.IsAlive(true); } }

		public ParticleEffect()
		{
			cachedParticleSystem = new CachedValue<ParticleSystem>(GetComponent<ParticleSystem>);
		}

		protected virtual void Awake()
		{
			CachedGameObject.SetActive(false);
		}

		protected virtual void Update()
		{
			if (!IsAlive)
				PoolManager.Instance.Recycle(this);
		}

		public virtual void OnCreate()
		{
			CachedParticleSystem.Play(true);
		}

		public virtual void OnRecycle()
		{
		}
	}
}