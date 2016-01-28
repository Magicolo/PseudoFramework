using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class PhysicsEventRelaySystem : SystemBase, IUpdateable
	{
		public override IEntityGroup GetEntities()
		{
			return null;
		}

		public void Update()
		{
			while (PhysicsEventRelayComponent.QueuedEvents.Count > 0)
			{
				var physicsEvent = PhysicsEventRelayComponent.QueuedEvents.Dequeue();
				EventManager.Trigger(physicsEvent.Event, physicsEvent.Entity, physicsEvent.Data);
				PhysicsEventRelayComponent.EventDataPool.Recycle(physicsEvent);
			}
		}
	}
}