using Pseudo.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal
{
	public interface IPoolInitializable
	{
		void OnBeforePoolInitialize();
		void OnAfterPoolInitialize(List<IPoolSetter> setters);
	}
}
