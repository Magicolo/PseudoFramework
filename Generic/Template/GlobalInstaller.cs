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
			container.Binder.Bind<GameManager, IGameManager>().ToSingletonMethod(c => InstantiateOrFind(GameManager));
			container.Binder.Bind<AudioManager, IAudioManager>().ToSingletonMethod(c => InstantiateOrFind(AudioManager));
			container.Binder.Bind<ParticleManager, IParticleManager>().ToSingletonMethod(c => InstantiateOrFind(ParticleManager));
			container.Binder.Bind<InputManager, Input.IInputManager>().ToSingletonMethod(c => InstantiateOrFind(InputManager));
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
