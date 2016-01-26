using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class EventRelaySystem : SystemBase
	{
		IEntityGroup entities;

		public override void OnInitialize()
		{
			base.OnInitialize();

			entities = EntityManager.Entities.Filter(typeof(EventRelayComponent));
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
			if (!entities.Contains(entity))
				return;

			var relay = entity.GetComponent<EventRelayComponent>();

			for (int i = 0; i < relay.Events.Length; i++)
			{
				var relayEvent = relay.Events[i];

				if (relayEvent.Event.HasAll(identifier))
					EventManager.Trigger(identifier, relayEvent.Relay);
			}
		}
	}
}