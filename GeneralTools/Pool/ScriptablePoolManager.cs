using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class ScriptablePoolManager<T> : TypePoolManager<T, ScriptablePoolz<T>> where T : ScriptableObject, IPoolable
	{
		protected override ScriptablePoolz<T> CreatePool(Type identifier)
		{
			return new ScriptablePoolz<T>(identifier);
		}
	}
}