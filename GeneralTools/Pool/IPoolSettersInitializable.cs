using Pseudo.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Pool
{
	public interface IPoolSettersInitializable
	{
		void OnPrePoolSettersInitialize();
		void OnPostPoolSettersInitialize(List<IPoolSetter> setters);
	}
}
