using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.EntityFramework;
using Pseudo.Pooling;
using Pseudo.Communication;

namespace Pseudo
{
	public class SetActiveOnMessage : ComponentBehaviourBase, IMessageable
	{
		[Serializable]
		public struct ActiveAction
		{
			public GameObject Target;
			public bool Active;
			public Message Message;
		}

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