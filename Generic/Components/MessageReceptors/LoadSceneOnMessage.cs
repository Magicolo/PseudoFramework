using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.SceneManagement;
using Pseudo.Injection;
using Pseudo.EntityFramework;
using Pseudo.Communication;

namespace Pseudo
{
	public class LoadSceneOnMessage : ComponentBehaviourBase, IMessageable
	{
		public string Scene;
		public MessageEnum Message;

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
			load |= Message.Equals(message) && !string.IsNullOrEmpty(Scene);
		}
	}
}