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
		public struct ActiveAction
		{
			public MessageEnum Message;
			public MonoBehaviour Target;
			public bool Enabled;
		}

		[InitializeContent]
		public List<ActiveAction> Actions = new List<ActiveAction>();

		void ApplyAction(ActiveAction action)
		{
			if (action.Target == null)
				return;

			action.Target.enabled = action.Enabled;
		}

		void IMessageable.OnMessage<TId>(TId message)
		{
			for (int i = 0; i < Actions.Count; i++)
			{
				var action = Actions[i];

				if (action.Message.Equals(message))
					ApplyAction(action);
			}
		}
	}
}