using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class ScriptablePoolManager<T> : TypePoolManager<T, ScriptablePool<T>> where T : ScriptableObject, IPoolable
	{
		protected override ScriptablePool<T> CreatePool(Type identifier)
		{
			return new ScriptablePool<T>(identifier);
		}
	}
}