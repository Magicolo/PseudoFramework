using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public class SetEnabledOnMessage : ComponentBehaviour, IMessageable
	{
		[Serializable]
		public struct EnableAction
		{
			public MessageEnum Message;
			public MonoBehaviour Target;
			public bool Enabled;
		}

		[InitializeContent]
		public EnableAction[] Actions = new EnableAction[0];
		
		void IMessageable.OnMessage<TId>(TId message)
		{
			for (int i = 0; i < Actions.Length; i++)
			{
				var action = Actions[i];

				if (action.Message.Equals(message) && action.Target != null)
					action.Target.enabled = action.Enabled;
			}
		}
	}
}