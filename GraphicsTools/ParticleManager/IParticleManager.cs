using Pseudo;
using UnityEngine;

namespace Pseudo
{
	public interface IParticleManager
	{
		ParticleEffect CreateEffect(string name, Vector3 position);
		ParticleEffect CreateEffect(string name, Vector3 position, Transform parent);
		T CreateEffect<T>(T prefab, Vector3 position) where T : ParticleEffect;
		T CreateEffect<T>(T prefab, Vector3 position, Transform parent) where T : ParticleEffect;
		void RecycleEffect<T>(T instance) where T : ParticleEffect;
		void StopEffects<T>(T prefab) where T : ParticleEffect;
		void StopAllEffects();
	}
}