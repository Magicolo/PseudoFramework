using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public abstract class PrefabPool : Pool
	{
		protected PrefabPool(UnityEngine.Object reference, int startSize = 8) : base(reference, reference.GetType(), startSize) { }

		public override void Clear()
		{
			lock (instances)
			{
				while (instances.Count > 0)
					((UnityEngine.Object)instances.Dequeue()).Destroy();
			}

			lock (toInitialize)
			{
				while (toInitialize.Count > 0)
					((UnityEngine.Object)toInitialize.Dequeue()).Destroy();
			}
		}

		protected override object CreateInstance()
		{
			return UnityEngine.Object.Instantiate((UnityEngine.Object)reference);
		}
	}
}