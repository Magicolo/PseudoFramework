using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Internal;

namespace Pseudo.Injection.Internal
{
	public abstract class InjectableMemberBase<TMember> : IInjectableMember where TMember : MemberInfo
	{
		public MemberInfo Member
		{
			get { return member; }
		}
		public InjectAttribute Attribute
		{
			get { return attribute; }
		}

		protected readonly TMember member;
		protected readonly InjectAttribute attribute;

		protected InjectableMemberBase(TMember member)
		{
			this.member = member;

			if (this.member == null)
				attribute = new InjectAttribute();
			else
				attribute = member.GetAttribute<InjectAttribute>(true) ?? new InjectAttribute();
		}

		public object Inject(InjectionContext context)
		{
			SetupContext(ref context);

			if (!attribute.Optional || CanInject(context))
				return Inject(ref context);

			return null;
		}

		public abstract bool CanInject(InjectionContext context);

		protected abstract void SetupContext(ref InjectionContext context);
		protected abstract object Inject(ref InjectionContext context);
	}
}