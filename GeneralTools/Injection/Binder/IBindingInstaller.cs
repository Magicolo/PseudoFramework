using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface IBindingInstaller
	{
		void Install(IBinder binder);
	}
}
