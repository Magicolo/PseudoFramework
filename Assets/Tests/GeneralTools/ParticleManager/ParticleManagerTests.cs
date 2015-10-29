using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Tests
{
	public class ParticleManagerTests : PMonoBehaviour
	{
		public ParticleEffect[] EffectPrefabs;

		[Button]
		public bool playRandomEffectInPrefabs;
		void PlayRandomEffectInPrefabs()
		{
			ParticleManager.Instance.Create(EffectPrefabs.GetRandom(), CachedTransform.position + new Vector3(3f, 3f), CachedTransform);
		}

		[Button]
		public bool playRegisteredEffect;
		void PlayRegisteredEffect()
		{
			ParticleManager.Instance.Create("Effect3", CachedTransform.position + new Vector3(-3f, 3f), CachedTransform);
		}
	}
}