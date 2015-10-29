using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Particle Effect")]
	[RequireComponent(typeof(ParticleSystem))]
	public class ParticleEffect : PMonoBehaviour, IPoolable, ICopyable<ParticleEffect>
	{
		protected readonly CachedValue<ParticleSystem> cachedParticleSystem;

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

		public virtual void Stop()
		{
			cachedParticleSystem.Value.Stop(true);
		}

		public virtual void OnCreate()
		{
			CachedParticleSystem.Play(true);
		}

		public virtual void OnRecycle()
		{
		}

		public void Copy(ParticleEffect reference)
		{
		}
	}
}