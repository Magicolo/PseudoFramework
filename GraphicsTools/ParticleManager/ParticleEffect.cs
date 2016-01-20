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
		public ParticleSystem CachedParticleSystem { get { return cachedParticleSystem; } }
		public bool IsPlaying { get { return CachedParticleSystem.isPlaying; } }

		readonly CachedValue<ParticleSystem> cachedParticleSystem;
		IParticleManager particleManager;
		bool hasPlayed;

		public ParticleEffect()
		{
			cachedParticleSystem = new CachedValue<ParticleSystem>(GetComponent<ParticleSystem>);
		}

		public virtual void Initialize(IParticleManager particleManager, Vector3 position, Transform parent)
		{
			this.particleManager = particleManager;
			CachedTransform.position = position;
			CachedTransform.parent = parent;
		}

		public void Play()
		{
			CachedParticleSystem.Play(true);
			hasPlayed = true;
		}

		public void Stop()
		{
			CachedParticleSystem.Stop(true);
		}

		void LateUpdate()
		{
			if (hasPlayed && !IsPlaying)
				particleManager.RecycleEffect(this);
		}
	}
}