using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection;
using Pseudo.Audio;
using Pseudo.Particle;
using Pseudo.Input;

namespace Pseudo
{
	public class GlobalInstaller : BehaviourInstallerBase
	{
		[Header("Prefabs")]
		public GameManager GameManager;
		public AudioManager AudioManager;
		public ParticleManager ParticleManager;
		public InputManager InputManager;

		public override void Install(IContainer container)
		{
			container.Binder.Bind<GameManager, IGameManager>().ToMethod(c => InstantiateOrFind(GameManager)).AsSingleton();
			container.Binder.Bind<AudioManager, IAudioManager>().ToMethod(c => InstantiateOrFind(AudioManager)).AsSingleton();
			container.Binder.Bind<ParticleManager, IParticleManager>().ToMethod(c => InstantiateOrFind(ParticleManager)).AsSingleton();
			container.Binder.Bind<InputManager, IInputManager>().ToMethod(c => InstantiateOrFind(InputManager)).AsSingleton();
		}

		T InstantiateOrFind<T>(T prefab) where T : Component
		{
			var instance = FindObjectOfType<T>();

			if (instance == null)
				instance = Instantiate(prefab);

			instance.transform.parent = transform;

			return instance;
		}
	}
}
