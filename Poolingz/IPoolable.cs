using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Pooling
{
	public interface IPoolable
	{
		void OnCreate();
		void OnRecycle();
	}
}
