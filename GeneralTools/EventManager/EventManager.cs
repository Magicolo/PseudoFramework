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
		readonly Dictionary<Type, IEventGroup> typeToEventGroups = new Dictionary<Type, IEventGroup>();
		Queue<IEvent> queuedEvents = new Queue<IEvent>();
		Queue<IEvent> resolvingEvents = new Queue<IEvent>();

		public void SubscribeAll<TId>(Action<TId> receiver) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.SubscribeAll(receiver);
		}

		public void Subscribe<TId>(TId identifier, Action receiver) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Subscribe(identifier, receiver);
		}

		public void Subscribe<TId, TArg>(TId identifier, Action<TArg> receiver) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Subscribe(identifier, receiver);
		}

		public void Subscribe<TId, TArg1, TArg2>(TId identifier, Action<TArg1, TArg2> receiver) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Subscribe(identifier, receiver);
		}

		public void Subscribe<TId, TArg1, TArg2, TArg3>(TId identifier, Action<TArg1, TArg2, TArg3> receiver) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Subscribe(identifier, receiver);
		}

		public void Subscribe<TId, TArg1, TArg2, TArg3, TArg4>(TId identifier, Action<TArg1, TArg2, TArg3, TArg4> receiver) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Subscribe(identifier, receiver);
		}

		public void UnsubscribeAll<TId>(Action<TId> receiver) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.UnsubscribeAll(receiver);
		}

		public void Unsubscribe<TId>(TId identifier, Action receiver) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Unsubscribe(identifier, receiver);
		}

		public void Unsubscribe<TId, TArg>(TId identifier, Action<TArg> receiver) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Unsubscribe(identifier, receiver);
		}

		public void Unsubscribe<TId, TArg1, TArg2>(TId identifier, Action<TArg1, TArg2> receiver) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Unsubscribe(identifier, receiver);
		}

		public void Unsubscribe<TId, TArg1, TArg2, TArg3>(TId identifier, Action<TArg1, TArg2, TArg3> receiver) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Unsubscribe(identifier, receiver);
		}

		public void Unsubscribe<TId, TArg1, TArg2, TArg3, TArg4>(TId identifier, Action<TArg1, TArg2, TArg3, TArg4> receiver) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Unsubscribe(identifier, receiver);
		}

		public void Trigger<TId>(TId identifier) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			var eventData = TypePoolManager.Create<TriggerEvent<TId>>();
			eventData.EventGroup = eventGroup;
			eventData.Identifier = identifier;
			EnqueueEvent(eventData);
		}

		public void Trigger<TId, TArg>(TId identifier, TArg argument) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			var eventData = TypePoolManager.Create<TriggerEvent<TId, TArg>>();
			eventData.EventGroup = eventGroup;
			eventData.Identifier = identifier;
			eventData.Argument = argument;
			EnqueueEvent(eventData);
		}

		public void Trigger<TId, TArg1, TArg2>(TId identifier, TArg1 argument1, TArg2 argument2) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			var eventData = TypePoolManager.Create<TriggerEvent<TId, TArg1, TArg2>>();
			eventData.EventGroup = eventGroup;
			eventData.Identifier = identifier;
			eventData.Argument1 = argument1;
			eventData.Argument2 = argument2;
			EnqueueEvent(eventData);
		}

		public void Trigger<TId, TArg1, TArg2, TArg3>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			var eventData = TypePoolManager.Create<TriggerEvent<TId, TArg1, TArg2, TArg3>>();
			eventData.EventGroup = eventGroup;
			eventData.Identifier = identifier;
			eventData.Argument1 = argument1;
			eventData.Argument2 = argument2;
			eventData.Argument3 = argument3;
			EnqueueEvent(eventData);
		}

		public void Trigger<TId, TArg1, TArg2, TArg3, TArg4>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3, TArg4 argument4) where TId : IEquatable<TId>
		{
			var eventGroup = GetEventGroup<TId>();
			var eventData = TypePoolManager.Create<TriggerEvent<TId, TArg1, TArg2, TArg3, TArg4>>();
			eventData.EventGroup = eventGroup;
			eventData.Identifier = identifier;
			eventData.Argument1 = argument1;
			eventData.Argument2 = argument2;
			eventData.Argument3 = argument3;
			eventData.Argument4 = argument4;
			EnqueueEvent(eventData);
		}

		public void ResolveEvents()
		{
			SwitchQueues();

			while (resolvingEvents.Count > 0)
			{
				var eventData = resolvingEvents.Dequeue();
				eventData.Resolve();
				TypePoolManager.Recycle(eventData);
			}
		}

		public void EnqueueEvent(IEvent eventData)
		{
			queuedEvents.Enqueue(eventData);
		}

		void SwitchQueues()
		{
			var tempQueue = resolvingEvents;
			resolvingEvents = queuedEvents;
			queuedEvents = tempQueue;
		}

		EventGroup<TId> GetEventGroup<TId>() where TId : IEquatable<TId>
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