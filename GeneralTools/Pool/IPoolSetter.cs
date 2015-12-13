using System;

namespace Pseudo.Internal.Pool
{
	public interface IPoolSetter
	{
		void SetValue(object instance);
	}
}