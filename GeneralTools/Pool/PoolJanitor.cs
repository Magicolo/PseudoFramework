<<<<<<< HEAD
﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public class PoolJanitor : Singleton<PoolJanitor>
	{
		protected override void Awake()
		{
			base.Awake();

			PoolUtility.IsPlaying = true;
		}

		void OnDestroy()
		{
			PoolUtility.ClearAllPools();
		}

		void OnApplicationQuit()
		{
			PoolUtility.IsPlaying = false;
		}
	}
=======
﻿using UnityEngine;
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
>>>>>>> Entity2
}