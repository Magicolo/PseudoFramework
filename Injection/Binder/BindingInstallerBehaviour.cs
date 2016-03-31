using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection;

namespace Pseudo.Injection
{
	public abstract class BindingInstallerBehaviour : MonoBehaviour, IBindingInstaller
	{
		public abstract void Install(IBinder binder);
	}
}
