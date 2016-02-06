using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public class BehaviourEventRelayComponent : ComponentBehaviour
	{
		public static readonly Pool<BehaviourEventData> EventDataPool = new Pool<BehaviourEventData>(new BehaviourEventData(), () => new BehaviourEventData(), 0);
		public static readonly Queue<BehaviourEventData> QueuedEvents = new Queue<BehaviourEventData>();

		public class BehaviourEventData
		{
			public BehaviourEvents Event;
			public IEntity Entity;
			public object Argument;
		}

		public BehaviourEvents Events;

		public void EnqueueEvent(BehaviourEvents identifier, object argument = null)
		{
			if (Events == identifier)
			{
				var behaviourEvent = TypePoolManager.Create<BehaviourEventData>();
				behaviourEvent.Event = identifier;
				behaviourEvent.Entity = Entity;
				behaviourEvent.Argument = argument;

				QueuedEvents.Enqueue(behaviourEvent);
			}
		}

		void Awake()
		{
			EnqueueEvent(BehaviourEvents.OnAwake);
		}

		protected override void Start()
		{
			base.Start();

			EnqueueEvent(BehaviourEvents.OnStart);
		}

		void OnEnable()
		{
			EnqueueEvent(BehaviourEvents.OnEnable);
		}

		void OnDisable()
		{
			EnqueueEvent(BehaviourEvents.OnDisable);
		}

		void OnLevelWasLoaded(int level)
		{
			EnqueueEvent(BehaviourEvents.OnLevelWasLoaded, level);
		}
	}
}