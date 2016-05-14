using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public interface IBindingSelector
	{
		IBinding Select(InjectionContext context, List<IBinding> bindings);
		IEnumerable<IBinding> SelectAll(InjectionContext context, List<IBinding> bindings);
	}
}
