using Pseudo.Internal;
using System;

namespace Pseudo
{
	public interface IEventManager
	{
		void SubscribeAll<TId>(Action<TId> receiver) where TId : IEquatable<TId>;
		void SubscribeAll<TId, TArg>(Action<TId, TArg> receiver) where TId : IEquatable<TId>;
		void SubscribeAll<TId, TArg1, TArg2>(Action<TId, TArg1, TArg2> receiver) where TId : IEquatable<TId>;
		void SubscribeAll<TId, TArg1, TArg2, TArg3>(Action<TId, TArg1, TArg2, TArg3> receiver) where TId : IEquatable<TId>;
		void Subscribe<TId>(TId identifier, Action receiver) where TId : IEquatable<TId>;
		void Subscribe<TId, TArg>(TId identifier, Action<TArg> receiver) where TId : IEquatable<TId>;
		void Subscribe<TId, TArg1, TArg2>(TId identifier, Action<TArg1, TArg2> receiver) where TId : IEquatable<TId>;
		void Subscribe<TId, TArg1, TArg2, TArg3>(TId identifier, Action<TArg1, TArg2, TArg3> receiver) where TId : IEquatable<TId>;
		void UnsubscribeAll<TId>(Action<TId> receiver) where TId : IEquatable<TId>;
		void UnsubscribeAll<TId, TArg>(Action<TId, TArg> receiver) where TId : IEquatable<TId>;
		void UnsubscribeAll<TId, TArg1, TArg2>(Action<TId, TArg1, TArg2> receiver) where TId : IEquatable<TId>;
		void UnsubscribeAll<TId, TArg1, TArg2, TArg3>(Action<TId, TArg1, TArg2, TArg3> receiver) where TId : IEquatable<TId>;
		void Unsubscribe<TId>(TId identifier, Action receiver) where TId : IEquatable<TId>;
		void Unsubscribe<TId, TArg>(TId identifier, Action<TArg> receiver) where TId : IEquatable<TId>;
		void Unsubscribe<TId, TArg1, TArg2>(TId identifier, Action<TArg1, TArg2> receiver) where TId : IEquatable<TId>;
		void Unsubscribe<TId, TArg1, TArg2, TArg3>(TId identifier, Action<TArg1, TArg2, TArg3> receiver) where TId : IEquatable<TId>;
		void Trigger<TId>(TId identifier) where TId : IEquatable<TId>;
		void Trigger<TId, TArg>(TId identifier, TArg argument) where TId : IEquatable<TId>;
		void Trigger<TId, TArg1, TArg2>(TId identifier, TArg1 argument1, TArg2 argument2) where TId : IEquatable<TId>;
		void Trigger<TId, TArg1, TArg2, TArg3>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3) where TId : IEquatable<TId>;
		void ResolveEvents();
		void EnqueueEvent(IEvent eventData);
	}
}