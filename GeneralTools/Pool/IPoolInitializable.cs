using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Pool
{
	public interface IPoolInitializable
	{
		void OnPrePoolInitialize();
		void OnPostPoolInitialize();
	}
}
