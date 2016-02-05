using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.Internal
{
	public class ParticleManager : MonoBehaviour, IParticleManager
	{
		public ParticleEffect[] Effects = new ParticleEffect[0];

		readonly Dictionary<string, ParticleEffect> nameToPrefabs = new Dictionary<string, ParticleEffect>();
		readonly Dictionary<ParticleEffect, List<ParticleEffect>> prefabToActiveEffects = new Dictionary<ParticleEffect, List<ParticleEffect>>();
		readonly Dictionary<ParticleEffect, ParticleEffect> activeEffectToPrefab = new Dictionary<ParticleEffect, ParticleEffect>();

		void Awake()
		{
			for (int i = 0; i < Effects.Length; i++)
			{
				var particleEffect = Effects[i];

				if (particleEffect != null)
					nameToPrefabs[particleEffect.name] = particleEffect;
			}
		}

		public ParticleEffect CreateEffect(string name, Vector3 position, Transform parent)
		{
			Assert.IsNotNull(name);

			ParticleEffect particleEffect;

			if (!nameToPrefabs.TryGetValue(name, out particleEffect))
				Debug.LogError(string.Format("ParticleEffect named {0} was not found.", name));

			return CreateEffect(particleEffect, position, parent);
		}

		public ParticleEffect CreateEffect(string name, Vector3 position)
		{
			return CreateEffect(name, position, transform);
		}

		public T CreateEffect<T>(T prefab, Vector3 position, Transform parent) where T : ParticleEffect
		{
			Assert.IsNotNull(prefab);

			var effect = PrefabPoolManager.Create(prefab);
			effect.Initialize(this, position, parent);
			activeEffectToPrefab[prefab] = effect;

			List<ParticleEffect> activeEffects;

			if (!prefabToActiveEffects.TryGetValue(prefab, out activeEffects))
			{
				activeEffects = new List<ParticleEffect>();
				prefabToActiveEffects[prefab] = activeEffects;
			}

			activeEffects.Add(effect);

			return effect;
		}

		public T CreateEffect<T>(T prefab, Vector3 position) where T : ParticleEffect
		{
			Assert.IsNotNull(prefab);

			return CreateEffect(prefab, position, transform);
		}

		public void RecycleEffect<T>(T instance) where T : ParticleEffect
		{
			Assert.IsNotNull(instance);

			if (instance.IsPlaying)
				instance.Stop();

			ParticleEffect prefab;

			if (activeEffectToPrefab.TryGetValue(instance, out prefab))
			{
				var activeEffects = prefabToActiveEffects[prefab];
				activeEffects.Remove(instance);
			}

			PrefabPoolManager.Recycle(instance);
		}

		public void StopEffects<T>(T prefab) where T : ParticleEffect
		{
			Assert.IsNotNull(prefab);

			var activeEffects = GetActiveEffects(prefab);

			for (int i = activeEffects.Count - 1; i >= 0; i--)
				RecycleEffect(activeEffects[i]);
		}

		public void StopAllEffects()
		{
			var enumerator = activeEffectToPrefab.GetEnumerator();

			while (enumerator.MoveNext())
				RecycleEffect(enumerator.Current.Key);

			enumerator.Dispose();
		}

		List<ParticleEffect> GetActiveEffects(ParticleEffect prefab)
		{
			List<ParticleEffect> activeEffects;

			if (prefabToActiveEffects.TryGetValue(prefab, out activeEffects))
			{
				activeEffects = new List<ParticleEffect>();
				prefabToActiveEffects[prefab] = activeEffects;
			}

			return activeEffects;
		}
	}
}
