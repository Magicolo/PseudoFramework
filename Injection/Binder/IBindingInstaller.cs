using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public interface IBindingInstaller
	{
		void Install(IContainer container);
	}
}
