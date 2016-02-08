using Pseudo.Internal;
using Pseudo.Internal.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;
using Zenject;

namespace Pseudo.Internal
{
	public class EventManager : IEventManager, ILateTickable
	{
		static readonly Pool<DelayedEvent> delayedEventPool = new Pool<DelayedEvent>(new DelayedEvent(), () => new DelayedEvent(), 0);

		readonly Dictionary<Type, IEventGroup> typeToEventGroups = new Dictionary<Type, IEventGroup>();
		Queue<IEvent> queuedEvents = new Queue<IEvent>();
		Queue<IEvent> resolvingEvents = new Queue<IEvent>();

		public void SubscribeAll<TId>(Action<TId> receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().SubscribeAll(receiver);
		}

		public void SubscribeAll<TId, TArg>(Action<TId, TArg> receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().SubscribeAll(receiver);
		}

		public void SubscribeAll<TId, TArg1, TArg2>(Action<TId, TArg1, TArg2> receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().SubscribeAll(receiver);
		}

		public void SubscribeAll<TId, TArg1, TArg2, TArg3>(Action<TId, TArg1, TArg2, TArg3> receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().SubscribeAll(receiver);
		}

		public void Subscribe<TId>(TId identifier, Action receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().Subscribe(identifier, receiver);
		}

		public void Subscribe<TId, TArg>(TId identifier, Action<TArg> receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().Subscribe(identifier, receiver);
		}

		public void Subscribe<TId, TArg1, TArg2>(TId identifier, Action<TArg1, TArg2> receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().Subscribe(identifier, receiver);
		}

		public void Subscribe<TId, TArg1, TArg2, TArg3>(TId identifier, Action<TArg1, TArg2, TArg3> receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().Subscribe(identifier, receiver);
		}

		public void UnsubscribeAll<TId>(Action<TId> receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().UnsubscribeAll(receiver);
		}

		public void UnsubscribeAll<TId, TArg>(Action<TId, TArg> receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().UnsubscribeAll(receiver);
		}

		public void UnsubscribeAll<TId, TArg1, TArg2>(Action<TId, TArg1, TArg2> receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().UnsubscribeAll(receiver);
		}

		public void UnsubscribeAll<TId, TArg1, TArg2, TArg3>(Action<TId, TArg1, TArg2, TArg3> receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().UnsubscribeAll(receiver);
		}

		public void Unsubscribe<TId>(TId identifier, Action receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().Unsubscribe(identifier, receiver);
		}

		public void Unsubscribe<TId, TArg>(TId identifier, Action<TArg> receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().Unsubscribe(identifier, receiver);
		}

		public void Unsubscribe<TId, TArg1, TArg2>(TId identifier, Action<TArg1, TArg2> receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().Unsubscribe(identifier, receiver);
		}

		public void Unsubscribe<TId, TArg1, TArg2, TArg3>(TId identifier, Action<TArg1, TArg2, TArg3> receiver)
		{
			Assert.IsNotNull(receiver);
			GetEventGroup<TId>().Unsubscribe(identifier, receiver);
		}

		public void Trigger(IEvent eventData, float delay = 0f)
		{
			Assert.IsNotNull(eventData);

			if (delay <= 0f)
				queuedEvents.Enqueue(eventData);
			else
			{
				var delayedEvent = delayedEventPool.Create();
				delayedEvent.Delay = delay;
				delayedEvent.Event = eventData;
				queuedEvents.Enqueue(delayedEvent);
			}
		}

		public void Trigger<TId>(TId identifier)
		{
			Trigger(identifier, (object)null, (object)null, (object)null);
		}

		public void Trigger<TId, TArg>(TId identifier, TArg argument)
		{
			Trigger(identifier, argument, (object)null, (object)null);
		}

		public void Trigger<TId, TArg1, TArg2>(TId identifier, TArg1 argument1, TArg2 argument2)
		{
			Trigger(identifier, argument1, argument2, (object)null);
		}

		public void Trigger<TId, TArg1, TArg2, TArg3>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3)
		{
			var eventGroup = GetEventGroup<TId>();
			var eventData = TypePoolManager.Create<TriggerEvent<TId, TArg1, TArg2, TArg3>>();

			eventData.EventGroup = eventGroup;
			eventData.Identifier = identifier;
			eventData.Argument1 = argument1;
			eventData.Argument2 = argument2;
			eventData.Argument3 = argument3;

			Trigger((IEvent)eventData, 0f);
		}

		public void TriggerImmediate<TId>(TId identifier)
		{
			TriggerImmediate(identifier, (object)null, (object)null, (object)null);
		}

		public void TriggerImmediate<TId, TArg>(TId identifier, TArg argument)
		{
			TriggerImmediate(identifier, argument, (object)null, (object)null);
		}

		public void TriggerImmediate<TId, TArg1, TArg2>(TId identifier, TArg1 argument1, TArg2 argument2)
		{
			TriggerImmediate(identifier, argument1, argument2, (object)null);
		}

		public void TriggerImmediate<TId, TArg1, TArg2, TArg3>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3)
		{
			GetEventGroup<TId>().Trigger(identifier, argument1, argument2, argument3);
		}

		public void ResolveEvents()
		{
			SwitchQueues();

			while (resolvingEvents.Count > 0)
			{
				var eventData = resolvingEvents.Dequeue();

				if (eventData.Resolve())
					TypePoolManager.Recycle(eventData);
				else
					Trigger(eventData, 0f);
			}
		}

		void SwitchQueues()
		{
			var tempQueue = resolvingEvents;
			resolvingEvents = queuedEvents;
			queuedEvents = tempQueue;
		}

		EventGroup<TId> GetEventGroup<TId>()
		{
			IEventGroup eventGroup;

			if (!typeToEventGroups.TryGetValue(typeof(TId), out eventGroup))
			{
				eventGroup = new EventGroup<TId>();
				typeToEventGroups[typeof(TId)] = eventGroup;
			}

			return (EventGroup<TId>)eventGroup;
		}

		void ILateTickable.LateTick()
		{
			ResolveEvents();
		}
	}
}