﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public class SetActiveOnMessage : ComponentBehaviour, IMessageable
	{
		[Serializable]
		public struct ActiveAction
		{
			public MessageEnum Message;
			public GameObject Target;
			public bool Active;
		}

		[InitializeContent]
		public ActiveAction[] Actions = new ActiveAction[0];

		void IMessageable.OnMessage<TId>(TId message)
		{
			for (int i = 0; i < Actions.Length; i++)
			{
				var action = Actions[i];

				if (action.Message.Equals(message) && action.Target != null)
					action.Target.SetActive(action.Active);
			}
		}
	}
}