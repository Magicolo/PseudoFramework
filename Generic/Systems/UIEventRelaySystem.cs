using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class UIEventRelaySystem : SystemBase, IUpdateable
	{
		public override IEntityGroup GetEntities()
		{
			return null;
		}

		public void Update()
		{
			while (UIEventRelayComponent.QueuedEvents.Count > 0)
			{
				var uiEvent = UIEventRelayComponent.QueuedEvents.Dequeue();
				EventManager.Trigger(uiEvent.Event, uiEvent.Entity, uiEvent.Data);
				UIEventRelayComponent.EventDataPool.Recycle(uiEvent);
			}
		}
	}
}