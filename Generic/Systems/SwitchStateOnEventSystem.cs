using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class SwitchStateOnEventSystem : SystemBase
	{
		public override IEntityGroup GetEntities()
		{
			return EntityManager.Entities.Filter(new Type[]
			{
				typeof(StateComponent),
				typeof(SwitchStateOnEventComponent)
			});
		}

		public override void OnActivate()
		{
			base.OnActivate();

			EventManager.SubscribeAll((Action<Events, IEntity>)OnEvent);
		}

		public override void OnDeactivate()
		{
			base.OnDeactivate();

			EventManager.UnsubscribeAll((Action<Events, IEntity>)OnEvent);
		}

		void OnEvent(Events identifier, IEntity entity)
		{
			if (!Entities.Contains(entity))
				return;

			var state = entity.GetComponent<StateComponent>();
			var switchState = entity.GetComponent<SwitchStateOnEventComponent>();

			for (int i = 0; i < switchState.Events.Length; i++)
			{
				var switchEvent = switchState.Events[i];

				if (switchEvent.Event.HasAll(identifier))
					EventManager.Trigger(StateMachineEvents.OnStateSwitch, state.StateMachine, switchEvent.State);
			}
		}
	}
}