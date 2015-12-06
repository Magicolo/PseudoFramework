namespace Pseudo
{
	public interface Bag<T>
	{
		T Next();

		void Reset();
	}
}