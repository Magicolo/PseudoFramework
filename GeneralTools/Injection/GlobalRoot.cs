using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Injection;

namespace Pseudo
{
	public class GlobalRoot : RootBase
	{
		public static GlobalRoot Instance { get { return instance; } }

		static GlobalRoot instance;

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

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void OnLoad()
		{
			if (Instance == null)
			{
				var root = Resources.Load<GlobalRoot>("GlobalRoot");

				if (root != null)
					instance = Instantiate(root);
			}
		}
	}
}
