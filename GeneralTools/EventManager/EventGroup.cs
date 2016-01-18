using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal
{
	public class EventGroup<TId> : IEventGroup where TId : IEquatable<TId>
	{
		static readonly EqualityComparer<TId> comparer = EqualityComparer<TId>.Default;

		readonly IEventDispatcher[] allDispatchers = new IEventDispatcher[4];
		readonly Dictionary<TId, IEventDispatcher> idToDispatchers = new Dictionary<TId, IEventDispatcher>(comparer);
		readonly Queue<KeyValuePair<TId, IEventDispatcher>> toDispatch = new Queue<KeyValuePair<TId, IEventDispatcher>>();

		public void SubscribeAll(Action<TId> receiver)
		{
			var dispatcher = allDispatchers[0];

			if (dispatcher == null)
			{
				dispatcher = new EventDispatcher<TId>();
				allDispatchers[0] = dispatcher;
			}

			dispatcher.Subscribe(receiver);
		}

		public void SubscribeAll<TArg>(Action<TId, TArg> receiver)
		{
			var dispatcher = allDispatchers[1];

			if (dispatcher == null)
			{
				dispatcher = new EventDispatcher<TId, TArg>();
				allDispatchers[1] = dispatcher;
			}

			dispatcher.Subscribe(receiver);
		}

		public void SubscribeAll<TArg1, TArg2>(Action<TId, TArg1, TArg2> receiver)
		{
			var dispatcher = allDispatchers[2];

			if (dispatcher == null)
			{
				dispatcher = new EventDispatcher<TId, TArg1, TArg2>();
				allDispatchers[2] = dispatcher;
			}

			dispatcher.Subscribe(receiver);
		}

		public void SubscribeAll<TArg1, TArg2, TArg3>(Action<TId, TArg1, TArg2, TArg3> receiver)
		{
			var dispatcher = allDispatchers[3];

			if (dispatcher == null)
			{
				dispatcher = new EventDispatcher<TId, TArg1, TArg2, TArg3>();
				allDispatchers[3] = dispatcher;
			}

			dispatcher.Subscribe(receiver);
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
			var dispatcher = allDispatchers[0];

			if (dispatcher != null)
				dispatcher.Unsubscribe(receiver);
		}

		public void UnsubscribeAll<TArg>(Action<TId, TArg> receiver)
		{
			var dispatcher = allDispatchers[1];

			if (dispatcher != null)
				dispatcher.Unsubscribe(receiver);
		}

		public void UnsubscribeAll<TArg1, TArg2>(Action<TId, TArg1, TArg2> receiver)
		{
			var dispatcher = allDispatchers[2];

			if (dispatcher != null)
				dispatcher.Unsubscribe(receiver);
		}

		public void UnsubscribeAll<TArg1, TArg2, TArg3>(Action<TId, TArg1, TArg2, TArg3> receiver)
		{
			var dispatcher = allDispatchers[3];

			if (dispatcher != null)
				dispatcher.Unsubscribe(receiver);
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
			Trigger(identifier, (object)null, (object)null, (object)null);
		}

		public void Trigger<TArg>(TId identifier, TArg argument)
		{
			Trigger(identifier, argument, (object)null, (object)null);
		}

		public void Trigger<TArg1, TArg2>(TId identifier, TArg1 argument1, TArg2 argument2)
		{
			Trigger(identifier, argument1, argument2, (object)null);
		}

		public void Trigger<TArg1, TArg2, TArg3>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3)
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

				if (pair.Value is EventDispatcher<TArg1, TArg2, TArg3>)
					((EventDispatcher<TArg1, TArg2, TArg3>)pair.Value).Trigger(argument1, argument2, argument3);
				else if (pair.Value is EventDispatcher<TArg1, TArg2>)
					((EventDispatcher<TArg1, TArg2>)pair.Value).Trigger(argument1, argument2);
				else if (pair.Value is EventDispatcher<TArg1>)
					((EventDispatcher<TArg1>)pair.Value).Trigger(argument1);
				else if (pair.Value is EventDispatcher)
					((EventDispatcher)pair.Value).Trigger();
				else
					pair.Value.Trigger(argument1, argument2, argument3, null);

				if (allDispatchers[3] is EventDispatcher<TId, TArg1, TArg2, TArg3>)
					((EventDispatcher<TId, TArg1, TArg2, TArg3>)allDispatchers[3]).Trigger(identifier, argument1, argument2, argument3);
				else if (allDispatchers[2] is EventDispatcher<TId, TArg1, TArg2>)
					((EventDispatcher<TId, TArg1, TArg2>)allDispatchers[2]).Trigger(identifier, argument1, argument2);
				else if (allDispatchers[1] is EventDispatcher<TId, TArg1>)
					((EventDispatcher<TId, TArg1>)allDispatchers[1]).Trigger(identifier, argument1);
				else if (allDispatchers[0] is EventDispatcher<TId>)
					((EventDispatcher<TId>)allDispatchers[0]).Trigger(identifier);
			}
		}
	}
}
