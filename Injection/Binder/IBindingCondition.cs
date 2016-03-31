using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection.Internal;

namespace Pseudo.Injection
{
	public interface IBindingCondition
	{
		void When(Predicate<InjectionContext> condition);
		void When(string identifier);
		void When(InjectionContext.ContextTypes contextType);
		void WhenInjectedInto(Type declaringType);
		void WhenInjectedInto(object instance);
	}
}
