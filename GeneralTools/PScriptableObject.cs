using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class PScriptableObject : ScriptableObject, IPoolable
	{
		public virtual void OnCreate() { }

		public virtual void OnRecycle() { }
	}
}