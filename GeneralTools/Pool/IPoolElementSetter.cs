using System.Collections;

namespace Pseudo.Internal.Pool
{
	public interface IPoolElementSetter
	{
		void SetValue(IList array, int index);
	}
}