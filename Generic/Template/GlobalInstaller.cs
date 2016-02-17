﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Internal.Audio;

namespace Pseudo
{
	public class GlobalInstaller : BindingInstaller
	{
		[Header("Prefabs")]
		public GameManager GameManager;
		public AudioManager AudioManager;
		public ParticleManager ParticleManager;
		public InputManager InputManager;

		public override void Install(IBinder binder)
		{
			binder.Bind<IGameManager>().ToSingleMethod(c => InstantiateOrFind(GameManager));
			binder.Bind<IAudioManager>().ToSingleMethod(c => InstantiateOrFind(AudioManager));
			binder.Bind<IParticleManager>().ToSingleMethod(c => InstantiateOrFind(ParticleManager));
			binder.Bind<IInputManager>().ToSingleMethod(c => InstantiateOrFind(InputManager));
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
