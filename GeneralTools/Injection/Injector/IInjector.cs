using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface IInjector
	{
		IBinder Binder { get; }

		void Inject(object instance);
		void Inject(InjectionContext context);
		void Inject(GameObject instance, bool recursive = false);
	}
}
