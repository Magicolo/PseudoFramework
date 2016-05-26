using System;

namespace Pseudo.Communication
{
	public interface IMessageManager
	{
		void SubscribeAll(IMessageable receiver);
		void Subscribe<TId>(IMessageable<TId> receiver);
		void UnsubscribeAll(IMessageable receiver);
		void Unsubscribe<TId>(IMessageable<TId> receiver);
		void Send<TId>(object target, TId identifier);
		void Send<TId, TArg>(object target, TId identifier, TArg argument);
	}
}