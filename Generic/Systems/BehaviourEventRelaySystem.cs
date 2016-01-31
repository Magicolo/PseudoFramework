using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class BehaviourEventRelaySystem : SystemBase, IUpdateable
	{
		public override IEntityGroup GetEntities()
		{
			return null;
		}

		public void Update()
		{
			while (BehaviourEventRelayComponent.QueuedEvents.Count > 0)
			{
				var behaviourEvent = BehaviourEventRelayComponent.QueuedEvents.Dequeue();

				if (behaviourEvent.Argument == null)
					EventManager.Trigger(behaviourEvent.Event, behaviourEvent.Entity);
				else
					EventManager.Trigger(behaviourEvent.Event, behaviourEvent.Entity, behaviourEvent.Argument);

				BehaviourEventRelayComponent.EventDataPool.Recycle(behaviourEvent);
			}
		}
	}
}