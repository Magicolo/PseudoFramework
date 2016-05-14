using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Injection.Internal
{
	public abstract class InjectableMemberBase<TMember> : InjectableElementBase<TMember>, IInjectableMember<TMember> where TMember : MemberInfo
	{
		public TMember Member
		{
			get { return provider; }
		}

		protected InjectableMemberBase(TMember member) : base(member) { }

		protected override void SetupContext(ref InjectionContext context)
		{
			base.SetupContext(ref context);

			context.DeclaringType = provider.DeclaringType;
		}
	}
}