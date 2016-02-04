namespace Pseudo
{
	public interface IMessageManager
	{
		void Send<TId>(object target, TId identifier);
		void Send<TId, TArg>(object target, TId identifier, TArg argument);
		void Send<TId, TArg1, TArg2>(object target, TId identifier, TArg1 argument1, TArg2 argument2);
		void Send<TId, TArg1, TArg2, TArg3>(object target, TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3);
	}
}