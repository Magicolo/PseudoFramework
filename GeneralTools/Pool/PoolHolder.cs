using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public static class PoolHolder<T> where T : class
	{
		public readonly static Pool Pool = TypePoolManager.GetPool(typeof(T));
	}
}
