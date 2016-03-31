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
	[MessageEnum]
	public enum StateMachineMessages
	{
		OnStateEnter,
		OnStateExit
	}

	public class StateMachine : ComponentBehaviourBase, IMessageable
	{
		[Serializable]
		public struct StateData
		{
			public MessageEnum Message;
			public EntityBehaviour State;
		}

		[InitializeContent]
		public StateData[] States = new StateData[0];

		EntityBehaviour currentState;

		public override void OnAdded()
		{
			base.OnAdded();

			for (int i = 0; i < States.Length; i++)
			{
				var state = States[i];

				if (state.State != null)
					state.State.CachedGameObject.SetActive(false);
			}
		}

		void SwitchState(EntityBehaviour state)
		{
			if (currentState != null)
			{
				Entity.SendMessage(StateMachineMessages.OnStateExit, HierarchyScope.Children | HierarchyScope.Local);
				currentState.CachedGameObject.SetActive(false);
			}

			currentState = state;

			if (currentState != null)
			{
				currentState.CachedGameObject.SetActive(true);
				Entity.SendMessage(StateMachineMessages.OnStateEnter, HierarchyScope.Children | HierarchyScope.Local);
			}
		}

		void IMessageable.OnMessage<TId>(TId message)
		{
			for (int i = 0; i < States.Length; i++)
			{
				var state = States[i];

				if (state.Message.Equals(message))
					SwitchState(state.State);
			}
		}
	}
}