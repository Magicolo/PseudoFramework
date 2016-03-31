using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public interface IInjectable
	{
		void OnPreInject(IBinder binder);
		void OnPostInject(IBinder binder);
	}
}
