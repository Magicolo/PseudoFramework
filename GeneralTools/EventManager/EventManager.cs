using Pseudo.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenject;

namespace Pseudo
{
	public class EventManager : IEventManager, ILateTickable
	{
		readonly Dictionary<Type, IEventGroup> eventGroups = new Dictionary<Type, IEventGroup>();
		Queue<IEventData> queuedEvents = new Queue<IEventData>();
		Queue<IEventData> resolvingEvents = new Queue<IEventData>();

		public void Subscribe<TId>(Action<TId> receiver)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Subscribe(receiver);
		}

		public void Subscribe<TId>(TId identifier, Action receiver)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Subscribe(identifier, receiver);
		}

		public void Subscribe<TId, TArg>(TId identifier, Action<TArg> receiver)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Subscribe(identifier, receiver);
		}

		public void Subscribe<TId, TArg1, TArg2>(TId identifier, Action<TArg1, TArg2> receiver)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Subscribe(identifier, receiver);
		}

		public void Subscribe<TId, TArg1, TArg2, TArg3>(TId identifier, Action<TArg1, TArg2, TArg3> receiver)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Subscribe(identifier, receiver);
		}

		public void Subscribe<TId, TArg1, TArg2, TArg3, TArg4>(TId identifier, Action<TArg1, TArg2, TArg3, TArg4> receiver)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Subscribe(identifier, receiver);
		}

		public void Unsubscribe<TId>(Action<TId> receiver)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Unsubscribe(receiver);
		}

		public void Unsubscribe<TId>(TId identifier, Action receiver)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Unsubscribe(identifier, receiver);
		}

		public void Unsubscribe<TId, TArg>(TId identifier, Action<TArg> receiver)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Unsubscribe(identifier, receiver);
		}

		public void Unsubscribe<TId, TArg1, TArg2>(TId identifier, Action<TArg1, TArg2> receiver)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Unsubscribe(identifier, receiver);
		}

		public void Unsubscribe<TId, TArg1, TArg2, TArg3>(TId identifier, Action<TArg1, TArg2, TArg3> receiver)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Unsubscribe(identifier, receiver);
		}

		public void Unsubscribe<TId, TArg1, TArg2, TArg3, TArg4>(TId identifier, Action<TArg1, TArg2, TArg3, TArg4> receiver)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Unsubscribe(identifier, receiver);
		}

		public void Trigger<TId>(TId identifier)
		{
			var eventGroup = GetEventGroup<TId>();
			var eventData = TypePoolManager.Create<EventData<TId>>();
			eventData.EventGroup = eventGroup;
			eventData.Identifier = identifier;
			EnqueueEvent(eventData);
		}

		public void Trigger<TId, TArg>(TId identifier, TArg argument)
		{
			var eventGroup = GetEventGroup<TId>();
			var eventData = TypePoolManager.Create<EventData<TId, TArg>>();
			eventData.EventGroup = eventGroup;
			eventData.Identifier = identifier;
			eventData.Argument = argument;
			EnqueueEvent(eventData);
		}

		public void Trigger<TId, TArg1, TArg2>(TId identifier, TArg1 argument1, TArg2 argument2)
		{
			var eventGroup = GetEventGroup<TId>();
			var eventData = TypePoolManager.Create<EventData<TId, TArg1, TArg2>>();
			eventData.EventGroup = eventGroup;
			eventData.Identifier = identifier;
			eventData.Argument1 = argument1;
			eventData.Argument2 = argument2;
			EnqueueEvent(eventData);
		}

		public void Trigger<TId, TArg1, TArg2, TArg3>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3)
		{
			var eventGroup = GetEventGroup<TId>();
			var eventData = TypePoolManager.Create<EventData<TId, TArg1, TArg2, TArg3>>();
			eventData.EventGroup = eventGroup;
			eventData.Identifier = identifier;
			eventData.Argument1 = argument1;
			eventData.Argument2 = argument2;
			eventData.Argument3 = argument3;
			EnqueueEvent(eventData);
		}

		public void Trigger<TId, TArg1, TArg2, TArg3, TArg4>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3, TArg4 argument4)
		{
			var eventGroup = GetEventGroup<TId>();
			var eventData = TypePoolManager.Create<EventData<TId, TArg1, TArg2, TArg3, TArg4>>();
			eventData.EventGroup = eventGroup;
			eventData.Identifier = identifier;
			eventData.Argument1 = argument1;
			eventData.Argument2 = argument2;
			eventData.Argument3 = argument3;
			eventData.Argument4 = argument4;
			EnqueueEvent(eventData);
		}

		void EnqueueEvent(IEventData eventData)
		{
			queuedEvents.Enqueue(eventData);
		}

		void ResolveQueuedEvents()
		{
			SwitchQueues();

			while (resolvingEvents.Count > 0)
			{
				var eventData = resolvingEvents.Dequeue();
				eventData.Resolve();
				TypePoolManager.Recycle(eventData);
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

			if (!eventGroups.TryGetValue(typeof(TId), out eventGroup))
			{
				eventGroup = new EventGroup<TId>();
				eventGroups[typeof(TId)] = eventGroup;
			}

			return (EventGroup<TId>)eventGroup;
		}

		void ILateTickable.LateTick()
		{
			ResolveQueuedEvents();
		}
	}
}