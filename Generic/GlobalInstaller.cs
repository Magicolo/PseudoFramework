using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using Pseudo.Internal.Entity;
using Pseudo;

namespace Pseudo
{
	public class GlobalInstaller : MonoInstaller
	{
		[Header("Prefabs")]
		public AudioManager AudioManager;
		public ParticleManager ParticleManager;
		public InputManager InputManager;

		public override void InstallBindings()
		{
			BindPrefabOrInstance<IAudioManager, AudioManager>(AudioManager);
			BindPrefabOrInstance<IParticleManager, ParticleManager>(ParticleManager);
			BindPrefabOrInstance<IInputManager, InputManager>(InputManager);
		}

		void BindPrefabOrInstance<TContract, TConcrete>(TConcrete prefab) where TConcrete : Component, TContract
		{
			var instance = FindObjectOfType<TConcrete>();

			if (instance != null)
			{
				Container.Bind<TContract>().ToSingleMonoBehaviour<TConcrete>(instance.gameObject);
				instance.transform.parent = Container.RootTransform;
			}
			else if (prefab != null)
				Container.Bind<TContract>().ToSinglePrefab<TConcrete>(prefab.gameObject);
		}
	}
}