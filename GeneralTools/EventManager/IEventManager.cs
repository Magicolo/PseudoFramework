using Pseudo.Internal;
using System;

namespace Pseudo
{
	public interface IEventManager
	{
		void SubscribeAll<TId>(Action<TId> receiver);
		void SubscribeAll<TId, TArg>(Action<TId, TArg> receiver);
		void SubscribeAll<TId, TArg1, TArg2>(Action<TId, TArg1, TArg2> receiver);
		void SubscribeAll<TId, TArg1, TArg2, TArg3>(Action<TId, TArg1, TArg2, TArg3> receiver);
		void Subscribe<TId>(TId identifier, Action receiver);
		void Subscribe<TId, TArg>(TId identifier, Action<TArg> receiver);
		void Subscribe<TId, TArg1, TArg2>(TId identifier, Action<TArg1, TArg2> receiver);
		void Subscribe<TId, TArg1, TArg2, TArg3>(TId identifier, Action<TArg1, TArg2, TArg3> receiver);
		void UnsubscribeAll<TId>(Action<TId> receiver);
		void UnsubscribeAll<TId, TArg>(Action<TId, TArg> receiver);
		void UnsubscribeAll<TId, TArg1, TArg2>(Action<TId, TArg1, TArg2> receiver);
		void UnsubscribeAll<TId, TArg1, TArg2, TArg3>(Action<TId, TArg1, TArg2, TArg3> receiver);
		void Unsubscribe<TId>(TId identifier, Action receiver);
		void Unsubscribe<TId, TArg>(TId identifier, Action<TArg> receiver);
		void Unsubscribe<TId, TArg1, TArg2>(TId identifier, Action<TArg1, TArg2> receiver);
		void Unsubscribe<TId, TArg1, TArg2, TArg3>(TId identifier, Action<TArg1, TArg2, TArg3> receiver);
		void Trigger<TId>(TId identifier);
		void Trigger<TId, TArg>(TId identifier, TArg argument);
		void Trigger<TId, TArg1, TArg2>(TId identifier, TArg1 argument1, TArg2 argument2);
		void Trigger<TId, TArg1, TArg2, TArg3>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3);
		void ResolveEvents();
		void Trigger(IEvent eventData);
	}
}