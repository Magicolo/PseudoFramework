using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[MessageEnum]
	public enum StateMachineMessages
	{
		OnStateEnter,
		OnStateExit
	}

	public class StateMachine : ComponentBehaviour, IMessageable
	{
		[Serializable]
		public struct StateData
		{
			public MessageEnum Message;
			public EntityBehaviour State;
		}

		public EntityBehaviour StartState;
		[InitializeContent]
		public StateData[] States = new StateData[0];

		EntityBehaviour currentState;

		[Message(ComponentMessages.OnAdded)]
		public void OnAdded()
		{
			for (int i = 0; i < States.Length; i++)
			{
				var state = States[i];

				if (state.State != null)
					state.State.CachedGameObject.SetActive(false);
			}

			SwitchState(StartState);
		}

		void SwitchState(EntityBehaviour state)
		{
			if (currentState != null)
			{
				Entity.SendMessage(StateMachineMessages.OnStateExit, MessagePropagation.DownwardsInclusive);
				currentState.CachedGameObject.SetActive(false);
			}

			currentState = state;

			if (currentState != null)
			{
				currentState.CachedGameObject.SetActive(true);
				Entity.SendMessage(StateMachineMessages.OnStateEnter, MessagePropagation.DownwardsInclusive);
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