using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public class LoadSceneOnMessage : ComponentBehaviour, IMessageable
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
	}
}