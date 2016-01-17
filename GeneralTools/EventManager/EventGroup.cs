using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal
{
	public class EventGroup<TId> : IEventGroup where TId : IEquatable<TId>
	{
		static readonly EqualityComparer<TId> comparer = EqualityComparer<TId>.Default;

		EventDispatcher<TId> allDispatcher = new EventDispatcher<TId>();
		readonly Dictionary<TId, IEventDispatcher> idToDispatchers = new Dictionary<TId, IEventDispatcher>(comparer);
		readonly Queue<KeyValuePair<TId, IEventDispatcher>> toDispatch = new Queue<KeyValuePair<TId, IEventDispatcher>>();

		public void SubscribeAll(Action<TId> receiver)
		{
			allDispatcher.Subscribe(receiver);
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

		public void Subscribe<TArg1, TArg2, TArg3, TArg4>(TId identifier, Action<TArg1, TArg2, TArg3, TArg4> receiver)
		{
			IEventDispatcher dispatcher;

			if (!idToDispatchers.TryGetValue(identifier, out dispatcher))
			{
				dispatcher = new EventDispatcher<TArg1, TArg2, TArg3, TArg4>();
				idToDispatchers[identifier] = dispatcher;
			}

			dispatcher.Subscribe(receiver);
		}

		public void UnsubscribeAll(Action<TId> receiver)
		{
			allDispatcher.Unsubscribe(receiver);
		}

		public void Unsubscribe(TId identifier, Action receiver)
		{
			IEventDispatcher dispatcher;

			if (idToDispatchers.TryGetValue(identifier, out dispatcher))
				dispatcher.Unsubscribe(receiver);
		}

		public void Unsubscribe<TArg>(TId identifier, Action<TArg> receiver)
		{
			IEventDispatcher dispatcher;

			if (idToDispatchers.TryGetValue(identifier, out dispatcher))
				dispatcher.Unsubscribe(receiver);
		}

		public void Unsubscribe<TArg1, TArg2>(TId identifier, Action<TArg1, TArg2> receiver)
		{
			IEventDispatcher dispatcher;

			if (idToDispatchers.TryGetValue(identifier, out dispatcher))
				dispatcher.Unsubscribe(receiver);
		}

		public void Unsubscribe<TArg1, TArg2, TArg3>(TId identifier, Action<TArg1, TArg2, TArg3> receiver)
		{
			IEventDispatcher dispatcher;

			if (idToDispatchers.TryGetValue(identifier, out dispatcher))
				dispatcher.Unsubscribe(receiver);
		}

		public void Unsubscribe<TArg1, TArg2, TArg3, TArg4>(TId identifier, Action<TArg1, TArg2, TArg3, TArg4> receiver)
		{
			IEventDispatcher dispatcher;

			if (idToDispatchers.TryGetValue(identifier, out dispatcher))
				dispatcher.Unsubscribe(receiver);
		}

		public void Trigger(TId identifier)
		{
			Trigger(identifier, (object)null, (object)null, (object)null, (object)null);
		}

		public void Trigger<TArg>(TId identifier, TArg argument)
		{
			Trigger(identifier, argument, (object)null, (object)null, (object)null);
		}

		public void Trigger<TArg1, TArg2>(TId identifier, TArg1 argument1, TArg2 argument2)
		{
			Trigger(identifier, argument1, argument2, (object)null, (object)null);
		}

		public void Trigger<TArg1, TArg2, TArg3>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3)
		{
			Trigger(identifier, argument1, argument2, argument3, (object)null);
		}

		public void Trigger<TArg1, TArg2, TArg3, TArg4>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3, TArg4 argument4)
		{
			var enumerator = idToDispatchers.GetEnumerator();

			while (enumerator.MoveNext())
			{
				var pair = enumerator.Current;

				if (comparer.Equals(pair.Key, identifier))
					toDispatch.Enqueue(pair);
			}

			enumerator.Dispose();

			while (toDispatch.Count > 0)
			{
				var pair = toDispatch.Dequeue();

				if (pair.Value is EventDispatcher<TArg1, TArg2, TArg3, TArg4>)
					((EventDispatcher<TArg1, TArg2, TArg3, TArg4>)pair.Value).Trigger(argument1, argument2, argument3, argument4);
				else if (pair.Value is EventDispatcher<TArg1, TArg2, TArg3>)
					((EventDispatcher<TArg1, TArg2, TArg3>)pair.Value).Trigger(argument1, argument2, argument3);
				else if (pair.Value is EventDispatcher<TArg1, TArg2>)
					((EventDispatcher<TArg1, TArg2>)pair.Value).Trigger(argument1, argument2);
				else if (pair.Value is EventDispatcher<TArg1>)
					((EventDispatcher<TArg1>)pair.Value).Trigger(argument1);
				else if (pair.Value is EventDispatcher)
					((EventDispatcher)pair.Value).Trigger();
				else
					pair.Value.Trigger(argument1, argument2, argument3, argument4);
			}

			allDispatcher.Trigger(identifier);
		}
	}
}
