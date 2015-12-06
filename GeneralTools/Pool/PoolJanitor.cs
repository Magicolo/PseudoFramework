using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public class PoolJanitor : PMonoBehaviour
	{
		void OnDestroy()
		{
			PoolUtility.ClearAllPools();
		}
	}
}