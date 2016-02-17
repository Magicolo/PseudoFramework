﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Injection;

namespace Pseudo
{
	public interface IBindingCondition
	{
		void When(Predicate<InjectionContext> condition);
		void WhenIs(string identifier);
		void WhenIs(InjectionContext.Types contextType);
		void WhenInjectedInto(Type declaringType);
		void WhenInjectedInto(object instance);
	}
}
