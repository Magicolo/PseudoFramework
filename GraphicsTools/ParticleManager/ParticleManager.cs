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
			ParticleEffect particleEffect = ParticleEffects[i];
			particleEffects[particleEffect.name] = particleEffect;
		}
	}

	/// <summary>
	/// Creates a ParticleEffect corresponding to the <paramref name="name"/> from the list of registered ParticleEffects.
	/// </summary>
	/// <param name="name">The name of the ParticleEffect.</param>
	/// <param name="position">The position at which the ParticleEffect should be placed.</param>
	/// <param name="parent">The parent under which the ParicleEffect will be placed.</param>
	/// <returns>The instantiated ParticleEffect.</returns>
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

	/// <summary>
	/// Creates an instance of the provided ParticleEffect.
	/// </summary>
	/// <param name="effect">The ParticleEffect to instantiate.</param>
	/// <param name="position">The position at which the ParticleEffect should be placed.</param>
	/// <param name="parent">The parent under which the ParicleEffect will be placed.</param>
	/// <returns>The instantiated ParticleEffect.</returns>
	public virtual T Create<T>(T effect, Vector3 position, Transform parent) where T : ParticleEffect, ICopyable<T>
	{
		T particleEffect = Pools.BehaviourPool.CreateCopy(effect, position, parent);
		particleEffect.CachedParticleSystem.Play(true);

		return particleEffect;
	}
}
