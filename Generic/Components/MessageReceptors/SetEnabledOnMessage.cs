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
		public struct EnabledAction
		{
			public MonoBehaviour Target;
			public bool Enabled;
			public MessageEnum Message;
		}

		[InitializeContent]
		public EnabledAction[] Actions = new EnabledAction[0];

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