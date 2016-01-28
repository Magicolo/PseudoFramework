using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Interal;
using Pseudo.Internal;

namespace Pseudo
{
	public class PhysicsEventRelayComponent : ComponentBehaviour
	{
		public static readonly Pool<PhysicsEventData> EventDataPool = new Pool<PhysicsEventData>(new PhysicsEventData(), () => new PhysicsEventData(), 0);
		public static readonly Queue<PhysicsEventData> QueuedEvents = new Queue<PhysicsEventData>();

		public class PhysicsEventData
		{
			public PhysicsEvents Event;
			public IEntity Entity;
			public object Data;
		}

		public PhysicsEvents Events;

		public void EnqueueEvent(PhysicsEvents identifier, object data)
		{
			if (Events.HasAll(identifier))
			{
				var physicsEvent = EventDataPool.Create();
				physicsEvent.Event = identifier;
				physicsEvent.Entity = Entity.Entity;
				physicsEvent.Data = data;

				QueuedEvents.Enqueue(physicsEvent);
			}
		}

		void Awake()
		{
			TryAddRelay<OnTriggerEnterRelay>(PhysicsEvents.OnTriggerEnter);
			TryAddRelay<OnTriggerStayRelay>(PhysicsEvents.OnTriggerStay);
			TryAddRelay<OnTriggerExitRelay>(PhysicsEvents.OnTriggerExit);

			TryAddRelay<OnCollisionEnterRelay>(PhysicsEvents.OnCollisionEnter);
			TryAddRelay<OnCollisionStayRelay>(PhysicsEvents.OnCollisionStay);
			TryAddRelay<OnCollisionExitRelay>(PhysicsEvents.OnCollisionExit);

			TryAddRelay<OnTriggerEnter2DRelay>(PhysicsEvents.OnTriggerEnter2D);
			TryAddRelay<OnTriggerStay2DRelay>(PhysicsEvents.OnTriggerStay2D);
			TryAddRelay<OnTriggerExit2DRelay>(PhysicsEvents.OnTriggerExit2D);

			TryAddRelay<OnCollisionEnter2DRelay>(PhysicsEvents.OnCollisionEnter2D);
			TryAddRelay<OnCollisionStay2DRelay>(PhysicsEvents.OnCollisionStay2D);
			TryAddRelay<OnCollisionExit2DRelay>(PhysicsEvents.OnCollisionExit2D);
		}

		void TryAddRelay<T>(PhysicsEvents identifier) where T : PhysicsEventRelayBase
		{
			if (Events.HasAll(identifier))
			{
				var relay = CachedGameObject.GetOrAddComponent<T>();
				relay.hideFlags = HideFlags.HideInInspector;
				relay.Relay = this;
			}
		}
	}
}