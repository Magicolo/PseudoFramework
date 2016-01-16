using Pseudo.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	public class EventManager : IEventManager
	{
		readonly Dictionary<Type, IEventGroup> eventGroups = new Dictionary<Type, IEventGroup>();

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
			eventGroup.Trigger(identifier);
		}

		public void Trigger<TId, TArg>(TId identifier, TArg argument)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Trigger(identifier, argument);
		}

		public void Trigger<TId, TArg1, TArg2>(TId identifier, TArg1 argument1, TArg2 argument2)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Trigger(identifier, argument1, argument2);
		}

		public void Trigger<TId, TArg1, TArg2, TArg3>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Trigger(identifier, argument1, argument2, argument3);
		}

		public void Trigger<TId, TArg1, TArg2, TArg3, TArg4>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3, TArg4 argument4)
		{
			var eventGroup = GetEventGroup<TId>();
			eventGroup.Trigger(identifier, argument1, argument2, argument3, argument4);
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
	}
}
