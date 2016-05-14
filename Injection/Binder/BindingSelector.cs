using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class BindingSelector : IBindingSelector
	{
		public IBinding Select(InjectionContext context, List<IBinding> bindings)
		{
			for (int i = 0; i < bindings.Count; i++)
			{
				var binding = bindings[i];

				if (binding.Condition(context))
					return binding;
			}

			return null;
		}

		public IEnumerable<IBinding> SelectAll(InjectionContext context, List<IBinding> bindings)
		{
			return bindings.Where(b => b.Condition(context));
		}
	}
}
