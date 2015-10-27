using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class ParticleManager : Singleton<ParticleManager>
{
	public List<ParticleEffect> ParticleEffects = new List<ParticleEffect>();

	List<ParticleSystem> activeParticleEffects = new List<ParticleSystem>();

	Dictionary<string, ParticleEffect> nameEffectsDict;
	Dictionary<string, ParticleEffect> NameEffectsDict
	{
		get
		{
			if (nameEffectsDict == null)
				BuildDict();

			return nameEffectsDict;
		}
	}

	/// <summary>
	/// Creates a ParticleEffect corresponding to the <paramref name="name"/> from the list of registered ParticleEffects.
	/// </summary>
	/// <param name="name">The name of the ParticleEffect.</param>
	/// <param name="position">The position at which the ParticleEffect should be placed.</param>
	/// <returns>The instantiated particle system.</returns>
	public ParticleSystem CreateParticleEffect(string name, Vector3 position)
	{
		return CreateParticleEffect(NameEffectsDict[name], position);
	}

	/// <summary>
	/// Creates an instance of the provided ParticleEffect.
	/// </summary>
	/// <param name="effect">The ParticleEffect to instantiate.</param>
	/// <param name="position">The position at which the ParticleEffect should be placed.</param>
	/// <returns>The instantiated particle system.</returns>
	public ParticleSystem CreateParticleEffect(ParticleEffect effect, Vector3 position)
	{
		// TODO Insert Pooling Logic Herer
		ParticleSystem particleSystem = Instantiate(effect.Prefab);

		particleSystem.transform.parent = transform;
		particleSystem.transform.position = position;
		activeParticleEffects.Add(particleSystem);

		return particleSystem;
	}

	public void AddParticleEffect(ParticleEffect effect)
	{
		NameEffectsDict[effect.Name] = effect;
	}

	public void RemoveParticleEffect(ParticleEffect effect)
	{
		RemoveParticleEffect(effect.Name);
	}

	public void RemoveParticleEffect(string name)
	{
		NameEffectsDict.Remove(name);
	}

	void Update()
	{
		for (int i = 0; i < activeParticleEffects.Count; i++)
		{
			ParticleSystem particleSystem = activeParticleEffects[i];

			if (!particleSystem.IsAlive(true))
			{
				// TODO Insert Pool Recycle Logic
				activeParticleEffects.RemoveAt(i--);
			}
		}
	}

	void BuildDict()
	{
		nameEffectsDict = new Dictionary<string, ParticleEffect>();

		for (int i = 0; i < ParticleEffects.Count; i++)
		{
			ParticleEffect effect = ParticleEffects[i];
			nameEffectsDict[effect.Name] = effect;
		}
	}
}
