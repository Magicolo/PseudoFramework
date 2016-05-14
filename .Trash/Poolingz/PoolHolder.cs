using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public static class PoolHolder<T> where T : class
	{
		public readonly static IPool<T> Pool = (IPool<T>)TypePoolManager.GetPool(typeof(T));
	}
}
