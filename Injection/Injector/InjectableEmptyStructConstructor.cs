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
	public class InjectableEmptyStructConstructor : InjectableMemberBase<ConstructorInfo>, IInjectableConstructor
	{
		static readonly IInjectableParameter[] emptyParameters = new IInjectableParameter[0];

		public ConstructorInfo Constructor
		{
			get { return null; }
		}
		public IInjectableParameter[] Parameters
		{
			get { return emptyParameters; }
		}

		readonly Type concreteType;

		public InjectableEmptyStructConstructor(Type concreteType) : base(null)
		{
			this.concreteType = concreteType;
		}

		public override bool CanInject(InjectionContext context)
		{
			return true;
		}

		protected override void SetupContext(ref InjectionContext context) { }

		protected override object Inject(ref InjectionContext context)
		{
			return FormatterServices.GetUninitializedObject(concreteType);
		}
	}
}
