using Pseudo;
using UnityEngine;

public interface IParticleManager
{
	ParticleEffect Create(string name, Vector3 position);
	ParticleEffect Create(string name, Vector3 position, Transform parent);
	T Create<T>(T effect, Vector3 position) where T : ParticleEffect;
	T Create<T>(T effect, Vector3 position, Transform parent) where T : ParticleEffect;
}