using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public abstract class InstallerBehaviourBase : MonoBehaviour, IBindingInstaller
	{
		public abstract void Install(IContainer container);
	}
}
