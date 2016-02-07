using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using Pseudo;
using Pseudo.Internal.Audio;
using Pseudo.Internal;

namespace Pseudo
{
	public class GlobalInstaller : MonoInstaller
	{
		[Header("Prefabs")]
		public GameManager GameManager;
		public AudioManager AudioManager;
		public ParticleManager ParticleManager;
		public InputManager InputManager;

		public override void InstallBindings()
		{
			Container.BindSinglePrefabOrInstance<IGameManager, GameManager>(GameManager);
			Container.BindSinglePrefabOrInstance<IAudioManager, AudioManager>(AudioManager);
			Container.BindSinglePrefabOrInstance<IParticleManager, ParticleManager>(ParticleManager);
			Container.BindSinglePrefabOrInstance<IInputManager, InputManager>(InputManager);
		}
	}
}