using System;

namespace Pseudo.Internal
{
	public interface IMessageDispatcherGroup
	{
		void Send<TArg1, TArg2, TArg3>(object target, TArg1 argument1, TArg2 argument2, TArg3 argument3);
	}
}