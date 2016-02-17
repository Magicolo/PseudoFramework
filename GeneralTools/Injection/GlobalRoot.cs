using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Injection;

namespace Pseudo
{
	public class GlobalRoot : RootBase<GlobalRoot>
	{
		protected override void Initialize()
		{
			base.Initialize();

			DontDestroyOnLoad(gameObject);
		}

		protected override IBinder CreateBinder()
		{
			return new Binder();
		}

		void Reset()
		{
			this.SetExecutionOrder(-9999);
		}
	}
}
