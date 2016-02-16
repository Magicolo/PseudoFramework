using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
{
	public interface IInjectableMember
	{
		MemberInfo Member { get; }

		void Inject(object instance, IResolver resolver);
		bool CanInject(IResolver resolver);
	}
}
