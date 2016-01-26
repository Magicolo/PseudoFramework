using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal
{
	public class EventGroup<TId> : IEventGroup
	{
		static readonly EqualityComparer<TId> comparer = EqualityComparer<TId>.Default;

		IEventDispatcher allDispatcher0;
		IEventDispatcher allDispatcher1;
		IEventDispatcher allDispatcher2;
		IEventDispatcher allDispatcher3;
		readonly Dictionary<TId, IEventDispatcher> idToDispatchers = new Dictionary<TId, IEventDispatcher>(comparer);
		readonly Queue<IEventDispatcher> toDispatch = new Queue<IEventDispatcher>();

		public void SubscribeAll(Action<TId> receiver)
		{
			if (allDispatcher0 == null)
				allDispatcher0 = new EventDispatcher<TId>();

			allDispatcher0.Subscribe(receiver);
		}

		public void SubscribeAll<TArg>(Action<TId, TArg> receiver)
		{
			if (allDispatcher1 == null)
				allDispatcher1 = new EventDispatcher<TId, TArg>();

			allDispatcher1.Subscribe(receiver);
		}

		public void SubscribeAll<TArg1, TArg2>(Action<TId, TArg1, TArg2> receiver)
		{
			if (allDispatcher2 == null)
				allDispatcher2 = new EventDispatcher<TId, TArg1, TArg2>();

			allDispatcher2.Subscribe(receiver);
		}

		public void SubscribeAll<TArg1, TArg2, TArg3>(Action<TId, TArg1, TArg2, TArg3> receiver)
		{
			if (allDispatcher3 == null)
				allDispatcher3 = new EventDispatcher<TId, TArg1, TArg2, TArg3>();

			allDispatcher3.Subscribe(receiver);
		}

		public void Subscribe(TId identifier, Action receiver)
		{
			IEventDispatcher dispatcher;

			if (!idToDispatchers.TryGetValue(identifier, out dispatcher))
			{
				dispatcher = new EventDispatcher();
				idToDispatchers[identifier] = dispatcher;
			}

			dispatcher.Subscribe(receiver);
		}

		public void Subscribe<TArg>(TId identifier, Action<TArg> receiver)
		{
			IEventDispatcher dispatcher;

			if (!idToDispatchers.TryGetValue(identifier, out dispatcher))
			{
				dispatcher = new EventDispatcher<TArg>();
				idToDispatchers[identifier] = dispatcher;
			}

			dispatcher.Subscribe(receiver);
		}

		public void Subscribe<TArg1, TArg2>(TId identifier, Action<TArg1, TArg2> receiver)
		{
			IEventDispatcher dispatcher;

			if (!idToDispatchers.TryGetValue(identifier, out dispatcher))
			{
				dispatcher = new EventDispatcher<TArg1, TArg2>();
				idToDispatchers[identifier] = dispatcher;
			}

			dispatcher.Subscribe(receiver);
		}

		public void Subscribe<TArg1, TArg2, TArg3>(TId identifier, Action<TArg1, TArg2, TArg3> receiver)
		{
			IEventDispatcher dispatcher;

			if (!idToDispatchers.TryGetValue(identifier, out dispatcher))
			{
				dispatcher = new EventDispatcher<TArg1, TArg2, TArg3>();
				idToDispatchers[identifier] = dispatcher;
			}

			dispatcher.Subscribe(receiver);
		}

		public void UnsubscribeAll(Action<TId> receiver)
		{
			if (allDispatcher0 != null)
				allDispatcher0.Unsubscribe(receiver);
		}

		public void UnsubscribeAll<TArg>(Action<TId, TArg> receiver)
		{
			if (allDispatcher1 != null)
				allDispatcher1.Unsubscribe(receiver);
		}

		public void UnsubscribeAll<TArg1, TArg2>(Action<TId, TArg1, TArg2> receiver)
		{
			if (allDispatcher2 != null)
				allDispatcher2.Unsubscribe(receiver);
		}

		public void UnsubscribeAll<TArg1, TArg2, TArg3>(Action<TId, TArg1, TArg2, TArg3> receiver)
		{
			if (allDispatcher3 != null)
				allDispatcher3.Unsubscribe(receiver);
		}

		public void Unsubscribe(TId identifier, Delegate receiver)
		{
			IEventDispatcher dispatcher;

			if (idToDispatchers.TryGetValue(identifier, out dispatcher))
				dispatcher.Unsubscribe(receiver);
		}

		public void Trigger<TArg1, TArg2, TArg3>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3)
		{
			var enumerator = idToDispatchers.GetEnumerator();

			while (enumerator.MoveNext())
			{
				var pair = enumerator.Current;

				if (comparer.Equals(pair.Key, identifier))
					toDispatch.Enqueue(pair.Value);
			}

			enumerator.Dispose();

			while (toDispatch.Count > 0)
				Dispatch(toDispatch.Dequeue(), argument1, argument2, argument3);

			DispatchAll(identifier, argument1, argument2, argument3);
		}

		void Dispatch<TArg1, TArg2, TArg3>(IEventDispatcher dispatcher, TArg1 argument1, TArg2 argument2, TArg3 argument3)
		{
			if (dispatcher is EventDispatcher<TArg1, TArg2, TArg3>)
				((EventDispatcher<TArg1, TArg2, TArg3>)dispatcher).Trigger(argument1, argument2, argument3);
			else if (dispatcher is EventDispatcher<TArg1, TArg2>)
				((EventDispatcher<TArg1, TArg2>)dispatcher).Trigger(argument1, argument2);
			else if (dispatcher is EventDispatcher<TArg1>)
				((EventDispatcher<TArg1>)dispatcher).Trigger(argument1);
			else if (dispatcher is EventDispatcher)
				((EventDispatcher)dispatcher).Trigger();
			else
				dispatcher.Trigger(argument1, argument2, argument3, null);
		}

		void DispatchAll<TArg1, TArg2, TArg3>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3)
		{
			// Zero Arguments
			if (allDispatcher0 is EventDispatcher<TId>)
				((EventDispatcher<TId>)allDispatcher0).Trigger(identifier);
			else if (allDispatcher0 != null)
				allDispatcher0.Trigger(identifier, null, null, null);

			// One Arguments
			if (allDispatcher1 is EventDispatcher<TId, TArg1>)
				((EventDispatcher<TId, TArg1>)allDispatcher1).Trigger(identifier, argument1);
			else if (allDispatcher1 != null)
				allDispatcher1.Trigger(identifier, argument1, null, null);

			// Two Arguments
			if (allDispatcher2 is EventDispatcher<TId, TArg1, TArg2>)
				((EventDispatcher<TId, TArg1, TArg2>)allDispatcher2).Trigger(identifier, argument1, argument2);
			else if (allDispatcher2 != null)
				allDispatcher2.Trigger(identifier, argument1, argument2, null);

			// Three Arguments
			if (allDispatcher3 is EventDispatcher<TId, TArg1, TArg2, TArg3>)
				((EventDispatcher<TId, TArg1, TArg2, TArg3>)allDispatcher3).Trigger(identifier, argument1, argument2, argument3);
			else if (allDispatcher3 != null)
				allDispatcher3.Trigger(identifier, argument1, argument2, argument3);
		}
	}
}
