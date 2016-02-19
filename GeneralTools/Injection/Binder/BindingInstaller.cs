using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public abstract class BindingInstaller : MonoBehaviour, IBindingInstaller
	{
		public abstract void Install(IBinder binder);
	}
}
