using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal
{
	public class EventGroup<TId> : IEventGroup
	{
		EventDispatcher<TId> allDispatcher = new EventDispatcher<TId>();
		readonly Dictionary<TId, IEventDispatcher> dispatchers = new Dictionary<TId, IEventDispatcher>();

		public void Subscribe(Action<TId> receiver)
		{
			allDispatcher.Subscribe(receiver);
		}

		public void Subscribe(TId identifier, Action receiver)
		{
			IEventDispatcher dispatcher;

			if (!dispatchers.TryGetValue(identifier, out dispatcher))
			{
				dispatcher = new EventDispatcher();
				dispatchers[identifier] = dispatcher;
			}

			dispatcher.Subscribe(receiver);
		}

		public void Subscribe<TArg>(TId identifier, Action<TArg> receiver)
		{
			IEventDispatcher dispatcher;

			if (!dispatchers.TryGetValue(identifier, out dispatcher))
			{
				dispatcher = new EventDispatcher<TArg>();
				dispatchers[identifier] = dispatcher;
			}

			dispatcher.Subscribe(receiver);
		}

		public void Subscribe<TArg1, TArg2>(TId identifier, Action<TArg1, TArg2> receiver)
		{
			IEventDispatcher dispatcher;

			if (!dispatchers.TryGetValue(identifier, out dispatcher))
			{
				dispatcher = new EventDispatcher<TArg1, TArg2>();
				dispatchers[identifier] = dispatcher;
			}

			dispatcher.Subscribe(receiver);
		}

		public void Subscribe<TArg1, TArg2, TArg3>(TId identifier, Action<TArg1, TArg2, TArg3> receiver)
		{
			IEventDispatcher dispatcher;

			if (!dispatchers.TryGetValue(identifier, out dispatcher))
			{
				dispatcher = new EventDispatcher<TArg1, TArg2, TArg3>();
				dispatchers[identifier] = dispatcher;
			}

			dispatcher.Subscribe(receiver);
		}

		public void Subscribe<TArg1, TArg2, TArg3, TArg4>(TId identifier, Action<TArg1, TArg2, TArg3, TArg4> receiver)
		{
			IEventDispatcher dispatcher;

			if (!dispatchers.TryGetValue(identifier, out dispatcher))
			{
				dispatcher = new EventDispatcher<TArg1, TArg2, TArg3, TArg4>();
				dispatchers[identifier] = dispatcher;
			}

			dispatcher.Subscribe(receiver);
		}

		public void Unsubscribe(Action<TId> receiver)
		{
			allDispatcher.Unsubscribe(receiver);
		}

		public void Unsubscribe(TId identifier, Action receiver)
		{
			IEventDispatcher dispatcher;

			if (dispatchers.TryGetValue(identifier, out dispatcher))
				dispatcher.Unsubscribe(receiver);
		}

		public void Unsubscribe<TArg>(TId identifier, Action<TArg> receiver)
		{
			IEventDispatcher dispatcher;

			if (dispatchers.TryGetValue(identifier, out dispatcher))
				dispatcher.Unsubscribe(receiver);
		}

		public void Unsubscribe<TArg1, TArg2>(TId identifier, Action<TArg1, TArg2> receiver)
		{
			IEventDispatcher dispatcher;

			if (dispatchers.TryGetValue(identifier, out dispatcher))
				dispatcher.Unsubscribe(receiver);
		}

		public void Unsubscribe<TArg1, TArg2, TArg3>(TId identifier, Action<TArg1, TArg2, TArg3> receiver)
		{
			IEventDispatcher dispatcher;

			if (dispatchers.TryGetValue(identifier, out dispatcher))
				dispatcher.Unsubscribe(receiver);
		}

		public void Unsubscribe<TArg1, TArg2, TArg3, TArg4>(TId identifier, Action<TArg1, TArg2, TArg3, TArg4> receiver)
		{
			IEventDispatcher dispatcher;

			if (dispatchers.TryGetValue(identifier, out dispatcher))
				dispatcher.Unsubscribe(receiver);
		}

		public void Trigger(TId identifier)
		{
			IEventDispatcher dispatcher;

			if (dispatchers.TryGetValue(identifier, out dispatcher))
				dispatcher.Trigger();

			allDispatcher.Trigger(identifier);
		}

		public void Trigger<TArg>(TId identifier, TArg argument)
		{
			IEventDispatcher dispatcher;

			if (dispatchers.TryGetValue(identifier, out dispatcher))
			{
				var castedDispatcher = dispatcher as EventDispatcher<TArg>;

				if (castedDispatcher == null)
					dispatcher.Trigger(argument);
				else
					castedDispatcher.Trigger(argument);
			}

			allDispatcher.Trigger(identifier);
		}

		public void Trigger<TArg1, TArg2>(TId identifier, TArg1 argument1, TArg2 argument2)
		{
			IEventDispatcher dispatcher;

			if (dispatchers.TryGetValue(identifier, out dispatcher))
			{
				var castedDispatcher = dispatcher as EventDispatcher<TArg1, TArg2>;

				if (castedDispatcher == null)
					dispatcher.Trigger(argument1, argument2);
				else
					castedDispatcher.Trigger(argument1, argument2);
			}

			allDispatcher.Trigger(identifier);
		}

		public void Trigger<TArg1, TArg2, TArg3>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3)
		{
			IEventDispatcher dispatcher;

			if (dispatchers.TryGetValue(identifier, out dispatcher))
			{
				var castedDispatcher = dispatcher as EventDispatcher<TArg1, TArg2, TArg3>;

				if (castedDispatcher == null)
					dispatcher.Trigger(argument1, argument2, argument3);
				else
					castedDispatcher.Trigger(argument1, argument2, argument3);
			}

			allDispatcher.Trigger(identifier);
		}

		public void Trigger<TArg1, TArg2, TArg3, TArg4>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3, TArg4 argument4)
		{
			IEventDispatcher dispatcher;

			if (dispatchers.TryGetValue(identifier, out dispatcher))
			{
				var castedDispatcher = dispatcher as EventDispatcher<TArg1, TArg2, TArg3, TArg4>;

				if (castedDispatcher == null)
					dispatcher.Trigger(argument1, argument2, argument3, argument4);
				else
					castedDispatcher.Trigger(argument1, argument2, argument3, argument4);
			}

			allDispatcher.Trigger(identifier);
		}
	}
}
