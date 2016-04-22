using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using System.Runtime.Serialization;
using Pseudo.Reflection;

namespace Pseudo.Injection.Internal
{
	public class InjectableEmptyConstructor : InjectableElementBase<EmptyAttributeProvider>, IInjectableConstructor
	{
		static readonly EmptyAttributeProvider emptyAttributeProvider = new EmptyAttributeProvider();

		public ConstructorInfo Member
		{
			get { return null; }
		}
		public IInjectableParameter[] Parameters
		{
			get { return InjectionUtility.EmptyParameters; }
		}

		readonly IConstructorWrapper wrapper;

		public InjectableEmptyConstructor(Type concreteType) : base(emptyAttributeProvider)
		{
			wrapper = ReflectionUtility.CreateConstructorWrapper(concreteType);
		}

		protected override bool CanInject(ref InjectionContext context)
		{
			return true;
		}

		protected override object Inject(ref InjectionContext context)
		{
			return wrapper.Invoke();
		}
	}
}
