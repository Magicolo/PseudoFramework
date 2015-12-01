﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Internal.Pool
{
	public class ScriptablePool<T> : ScriptablePool where T : ScriptableObject
	{
		public ScriptablePool(int startSize = 4) : base(typeof(T), startSize) { }

		public ScriptablePool(T reference, int startSize = 4) : base(reference, startSize) { }

		new public T Create()
		{
			return (T)base.Create();
		}
	}

	public class ScriptablePool : PrefabPool
	{
		public ScriptablePool(Type type, int startSize = 4) : base(ScriptableObject.CreateInstance(type), startSize) { }

		public ScriptablePool(ScriptableObject reference, int startSize = 4) : base(reference, startSize) { }
	}
}