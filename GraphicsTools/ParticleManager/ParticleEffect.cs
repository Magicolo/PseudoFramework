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
	public class ParticleEffect : PMonoBehaviour
	{
		protected readonly CachedValue<ParticleSystem> cachedParticleSystem;

		public ParticleSystem CachedParticleSystem { get { return cachedParticleSystem; } }
		public bool IsPlaying { get { return cachedParticleSystem.Value.isPlaying; } }

		public ParticleEffect()
		{
			cachedParticleSystem = new CachedValue<ParticleSystem>(GetComponent<ParticleSystem>);
		}

		protected virtual void LateUpdate()
		{
			if (!IsPlaying)
				PrefabPoolManager.Recycle(this);
		}

		public virtual void Stop()
		{
			cachedParticleSystem.Value.Stop(true);
		}
	}
}