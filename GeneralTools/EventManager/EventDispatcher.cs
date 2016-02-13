using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Communication
{
	public class EventDispatcher : IEventDispatcher
	{
		public event Action Event = delegate { };

		public void Subscribe(Action receiver)
		{
			Event += receiver;
		}

		public void Unsubscribe(Action receiver)
		{
			Event -= receiver;
		}

		public void Trigger()
		{
			Event();
		}

		void IEventDispatcher.Subscribe(Delegate receiver)
		{
			if (receiver is Action)
				Subscribe((Action)receiver);
			else
				throw new ArgumentException(string.Format("Type {0} doesn't match event type {1}. Make sure that all subscribers have the same signature.", receiver.GetType().Name, Event.GetType().Name));
		}

		void IEventDispatcher.Unsubscribe(Delegate receiver)
		{
			if (receiver is Action)
				Unsubscribe((Action)receiver);
			else
				throw new ArgumentException(string.Format("Type {0} doesn't match event type {1}. Make sure that all subscribers have the same signature.", receiver.GetType().Name, Event.GetType().Name));
		}

		void IEventDispatcher.Trigger(object argument1, object argument2, object argument3, object argument4)
		{
			Trigger();
		}
	}

	public class EventDispatcher<T> : IEventDispatcher
	{
		public event Action<T> Event = delegate { };

		public void Subscribe(Action<T> receiver)
		{
			Event += receiver;
		}

		public void Unsubscribe(Action<T> receiver)
		{
			Event -= receiver;
		}

		public void Trigger(T argument)
		{
			Event(argument);
		}

		void IEventDispatcher.Subscribe(Delegate receiver)
		{
			if (receiver is Action<T>)
				Subscribe((Action<T>)receiver);
			else
				throw new ArgumentException(string.Format("Type {0} doesn't match event type {1}. Make sure that all subscribers have the same signature.", receiver.GetType().Name, Event.GetType().Name));
		}

		void IEventDispatcher.Unsubscribe(Delegate receiver)
		{
			if (receiver is Action<T>)
				Unsubscribe((Action<T>)receiver);
			else
				throw new ArgumentException(string.Format("Type {0} doesn't match event type {1}. Make sure that all subscribers have the same signature.", receiver.GetType().Name, Event.GetType().Name));
		}

		void IEventDispatcher.Trigger(object argument1, object argument2, object argument3, object argument4)
		{
			Trigger(argument1 is T ? (T)argument1 : default(T));
		}
	}

	public class EventDispatcher<T1, T2> : IEventDispatcher
	{
		public event Action<T1, T2> Event = delegate { };

		public void Subscribe(Action<T1, T2> receiver)
		{
			Event += receiver;
		}

		public void Unsubscribe(Action<T1, T2> receiver)
		{
			Event -= receiver;
		}

		public void Trigger(T1 argument1, T2 argument2)
		{
			Event(argument1, argument2);
		}

		void IEventDispatcher.Subscribe(Delegate receiver)
		{
			if (receiver is Action<T1, T2>)
				Subscribe((Action<T1, T2>)receiver);
			else
				throw new ArgumentException(string.Format("Type {0} doesn't match event type {1}. Make sure that all subscribers have the same signature.", receiver.GetType().Name, Event.GetType().Name));
		}

		void IEventDispatcher.Unsubscribe(Delegate receiver)
		{
			if (receiver is Action<T1, T2>)
				Unsubscribe((Action<T1, T2>)receiver);
			else
				throw new ArgumentException(string.Format("Type {0} doesn't match event type {1}. Make sure that all subscribers have the same signature.", receiver.GetType().Name, Event.GetType().Name));
		}

		void IEventDispatcher.Trigger(object argument1, object argument2, object argument3, object argument4)
		{
			Trigger(
				argument1 is T1 ? (T1)argument1 : default(T1),
				argument1 is T2 ? (T2)argument2 : default(T2));
		}
	}

	public class EventDispatcher<T1, T2, T3> : IEventDispatcher
	{
		public event Action<T1, T2, T3> Event = delegate { };

		public void Subscribe(Action<T1, T2, T3> receiver)
		{
			Event += receiver;
		}

		public void Unsubscribe(Action<T1, T2, T3> receiver)
		{
			Event -= receiver;
		}

		public void Trigger(T1 argument1, T2 argument2, T3 argument3)
		{
			Event(argument1, argument2, argument3);
		}

		void IEventDispatcher.Subscribe(Delegate receiver)
		{
			if (receiver is Action<T1, T2, T3>)
				Subscribe((Action<T1, T2, T3>)receiver);
			else
				throw new ArgumentException(string.Format("Type {0} doesn't match event type {1}. Make sure that all subscribers have the same signature.", receiver.GetType().Name, Event.GetType().Name));
		}

		void IEventDispatcher.Unsubscribe(Delegate receiver)
		{
			if (receiver is Action<T1, T2, T3>)
				Unsubscribe((Action<T1, T2, T3>)receiver);
			else
				throw new ArgumentException(string.Format("Type {0} doesn't match event type {1}. Make sure that all subscribers have the same signature.", receiver.GetType().Name, Event.GetType().Name));
		}

		void IEventDispatcher.Trigger(object argument1, object argument2, object argument3, object argument4)
		{
			Trigger(
				argument1 is T1 ? (T1)argument1 : default(T1),
				argument1 is T2 ? (T2)argument2 : default(T2),
				argument1 is T3 ? (T3)argument3 : default(T3));
		}
	}

	public class EventDispatcher<T1, T2, T3, T4> : IEventDispatcher
	{
		public event Action<T1, T2, T3, T4> Event = delegate { };

		public void Subscribe(Action<T1, T2, T3, T4> receiver)
		{
			Event += receiver;
		}

		public void Unsubscribe(Action<T1, T2, T3, T4> receiver)
		{
			Event -= receiver;
		}

		public void Trigger(T1 argument1, T2 argument2, T3 argument3, T4 argument4)
		{
			Event(argument1, argument2, argument3, argument4);
		}

		void IEventDispatcher.Subscribe(Delegate receiver)
		{
			if (receiver is Action<T1, T2, T3, T4>)
				Subscribe((Action<T1, T2, T3, T4>)receiver);
			else
				throw new ArgumentException(string.Format("Type {0} doesn't match event type {1}. Make sure that all subscribers have the same signature.", receiver.GetType().Name, Event.GetType().Name));
		}

		void IEventDispatcher.Unsubscribe(Delegate receiver)
		{
			if (receiver is Action<T1, T2, T3, T4>)
				Unsubscribe((Action<T1, T2, T3, T4>)receiver);
			else
				throw new ArgumentException(string.Format("Type {0} doesn't match event type {1}. Make sure that all subscribers have the same signature.", receiver.GetType().Name, Event.GetType().Name));
		}

		void IEventDispatcher.Trigger(object argument1, object argument2, object argument3, object argument4)
		{
			Trigger(
				argument1 is T1 ? (T1)argument1 : default(T1),
				argument1 is T2 ? (T2)argument2 : default(T2),
				argument1 is T3 ? (T3)argument3 : default(T3),
				argument1 is T4 ? (T4)argument4 : default(T4));
		}
	}
}
