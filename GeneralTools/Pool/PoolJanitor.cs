using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public class PoolJanitor : Singleton<PoolJanitor>
	{
		void OnDestroy()
		{
			PoolUtility.ClearAllPools();
		}
	}
}