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
	public abstract class InjectableMemberBase<TMember> : InjectableElementBase, IInjectableMember<TMember> where TMember : MemberInfo
	{
		public TMember Member
		{
			get { return member; }
		}

		protected readonly TMember member;

		protected InjectableMemberBase(TMember member) : base(member)
		{
			this.member = member;
		}

		protected override void SetupContext(ref InjectionContext context)
		{
			base.SetupContext(ref context);

			context.DeclaringType = member == null ? null : member.DeclaringType;
		}
	}
}