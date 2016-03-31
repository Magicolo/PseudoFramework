using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Injection.Internal
{
	public interface IInjectableMember
	{
		MemberInfo Member { get; }

		void Inject(InjectionContext context);
	}
}
