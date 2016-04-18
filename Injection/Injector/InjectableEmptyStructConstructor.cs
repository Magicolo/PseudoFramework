using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using System.Runtime.Serialization;

namespace Pseudo.Injection.Internal
{
	public class InjectableEmptyStructConstructor : InjectableElementBase, IInjectableConstructor
	{
		static readonly ICustomAttributeProvider emptyAttributeProvider = new EmptyAttributeProvider();

		public ConstructorInfo Member
		{
			get { return null; }
		}
		public IInjectableParameter[] Parameters
		{
			get { return InjectionUtility.EmptyParameters; }
		}

		readonly Type concreteType;

		public InjectableEmptyStructConstructor(Type concreteType) : base(emptyAttributeProvider)
		{
			this.concreteType = concreteType;
		}

		protected override bool CanInject(ref InjectionContext context)
		{
			return true;
		}

		protected override object Inject(ref InjectionContext context)
		{
			return FormatterServices.GetUninitializedObject(concreteType);
		}
	}
}
