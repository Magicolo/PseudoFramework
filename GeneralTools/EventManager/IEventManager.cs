using System;

namespace Pseudo
{
	public interface IEventManager
	{
		void Subscribe<T>(Action<T> receiver);
		void Subscribe<T>(T identifier, Action receiver);
		void Subscribe<T, TArg>(T identifier, Action<TArg> receiver);
		void Subscribe<T, TArg1, TArg2>(T identifier, Action<TArg1, TArg2> receiver);
		void Subscribe<T, TArg1, TArg2, TArg3>(T identifier, Action<TArg1, TArg2, TArg3> receiver);
		void Subscribe<T, TArg1, TArg2, TArg3, TArg4>(T identifier, Action<TArg1, TArg2, TArg3, TArg4> receiver);
		void Trigger<T>(T identifier);
		void Trigger<T, TArg>(T identifier, TArg argument);
		void Trigger<T, TArg1, TArg2>(T identifier, TArg1 argument1, TArg2 argument2);
		void Trigger<T, TArg1, TArg2, TArg3>(T identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3);
		void Trigger<T, TArg1, TArg2, TArg3, TArg4>(T identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3, TArg4 argument4);
		void Unsubscribe<T>(Action<T> receiver);
		void Unsubscribe<T>(T identifier, Action receiver);
		void Unsubscribe<T, TArg>(T identifier, Action<TArg> receiver);
		void Unsubscribe<T, TArg1, TArg2>(T identifier, Action<TArg1, TArg2> receiver);
		void Unsubscribe<T, TArg1, TArg2, TArg3>(T identifier, Action<TArg1, TArg2, TArg3> receiver);
		void Unsubscribe<T, TArg1, TArg2, TArg3, TArg4>(T identifier, Action<TArg1, TArg2, TArg3, TArg4> receiver);
	}
}