using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class ParticleManager : Singleton<ParticleManager>
{
	public ParticleEffect[] ParticleEffects = new ParticleEffect[0];

	protected readonly Dictionary<string, ParticleEffect> particleEffects = new Dictionary<string, ParticleEffect>();

	protected override void Awake()
	{
		base.Awake();

		for (int i = 0; i < ParticleEffects.Length; i++)
		{
			var particleEffect = ParticleEffects[i];

			if (particleEffect != null)
				particleEffects[particleEffect.name] = particleEffect;
		}
	}

	public virtual ParticleEffect Create(string name, Vector3 position, Transform parent)
	{
		ParticleEffect particleEffect;

		if (!particleEffects.TryGetValue(name, out particleEffect))
		{
			Debug.LogError(string.Format("ParticleEffect named {0} was not found.", name));
			return null;
		}

		return Create(particleEffect, position, parent);
	}

	public virtual ParticleEffect Create(string name, Vector3 position)
	{
		return Create(name, position, CachedTransform);
	}

	public virtual T Create<T>(T effect, Vector3 position, Transform parent) where T : ParticleEffect
	{
		T particleEffect = PrefabPoolManager.Create(effect);
		particleEffect.CachedTransform.position = position;
		particleEffect.CachedTransform.parent = parent;
		particleEffect.CachedParticleSystem.Play(true);

		return particleEffect;
	}

	public virtual T Create<T>(T effect, Vector3 position) where T : ParticleEffect
	{
		return Create(effect, position, CachedTransform);
	}
}
