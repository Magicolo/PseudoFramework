using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface IPoolable
	{
		void OnCreate();
		void OnRecycle();
	}
}
