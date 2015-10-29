﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class ParticleManager : Singleton<ParticleManager>
{
	public ParticleEffect[] ParticleEffects = new ParticleEffect[0];

	Dictionary<string, ParticleEffect> particleEffects = new Dictionary<string, ParticleEffect>();

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
	/// <returns>The instantiated ParticleEffect.</returns>
	public ParticleEffect Create(string name, Vector3 position, Transform parent = null)
	{
		ParticleEffect particleEffect;

		if (!particleEffects.TryGetValue(name, out particleEffect))
		{
			Debug.LogError(string.Format("ParticleEffect named {0} was not found.", name));
			return null;
		}

		return Create(particleEffect, position);
	}

	/// <summary>
	/// Creates an instance of the provided ParticleEffect.
	/// </summary>
	/// <param name="effect">The ParticleEffect to instantiate.</param>
	/// <param name="position">The position at which the ParticleEffect should be placed.</param>
	/// <returns>The instantiated ParticleEffect.</returns>
	public ParticleEffect Create(ParticleEffect effect, Vector3 position, Transform parent = null)
	{
		ParticleEffect particleEffect = PoolManager.Instance.Create(effect, position: position, parent: parent);

		return particleEffect;
	}
}
