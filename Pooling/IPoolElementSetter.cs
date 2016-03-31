using System.Collections;

namespace Pseudo.Pooling.Internal
{
	public interface IPoolElementSetter
	{
		void SetValue(IList array, int index);
	}
}