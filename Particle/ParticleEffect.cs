using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Particle
{
	[AddComponentMenu("Pseudo/General/Particle Effect")]
	[RequireComponent(typeof(ParticleSystem))]
	public class ParticleEffect : MonoBehaviour
	{
		public ParticleSystem CachedParticleSystem { get { return cachedParticleSystem; } }
		public bool IsPlaying { get { return CachedParticleSystem.isPlaying; } }

		readonly Lazy<ParticleSystem> cachedParticleSystem;
		IParticleManager particleManager;
		bool hasPlayed;

		public ParticleEffect()
		{
			cachedParticleSystem = new Lazy<ParticleSystem>(GetComponent<ParticleSystem>);
		}

		public virtual void Initialize(IParticleManager particleManager, Vector3 position, Transform parent)
		{
			this.particleManager = particleManager;
			transform.position = position;
			transform.parent = parent;
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