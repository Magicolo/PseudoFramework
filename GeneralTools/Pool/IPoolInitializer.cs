using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public interface IPoolInitializer
	{
		void Initialize(object instance);
	}
}
