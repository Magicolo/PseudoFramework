using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public class PoolJanitor : Singleton<PoolJanitor>
	{
		void LateUpdate()
		{
			for (int i = PoolUtility.ToUpdate.Count - 1; i >= 0; i--)
				PoolUtility.ToUpdate[i]();
		}

		void OnDestroy()
		{
			PoolUtility.ClearAllPools();
		}
	}
}