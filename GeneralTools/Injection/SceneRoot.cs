using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Injection;
using UnityEngine.SceneManagement;

namespace Pseudo
{
	public class SceneRoot : RootBase
	{
		protected override IBinder CreateBinder()
		{
			var globalRoot = GlobalRoot.Instance;

			return new Binder(globalRoot == null ? null : globalRoot.Binder);
		}

		void Reset()
		{
			this.SetExecutionOrder(-9998);
		}
	}
}
