using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Zenject;

namespace Pseudo
{
	public class LoadSceneOnMessage : ComponentBehaviour, IMessageable, ICopyable<LoadSceneOnMessage>
	{
		public MessageEnum LoadMessage;
		public string Scene;

		bool load;

		[Inject]
		readonly IGameManager gameManager = null;

		void LateUpdate()
		{
			if (load)
			{
				load = false;
				gameManager.LoadScene(Scene);
			}
		}

		public void OnMessage<TId>(TId message)
		{
			load |= LoadMessage.Equals(message);
		}

		public void Copy(LoadSceneOnMessage reference)
		{
			base.Copy(reference);

			LoadMessage = reference.LoadMessage;
			Scene = reference.Scene;
			load = reference.load;
		}
	}
}