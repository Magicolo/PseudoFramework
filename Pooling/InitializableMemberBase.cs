using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.PoolingNOOOO.Internal
{
	public abstract class InitializableMemberBase<TMember> : IInitializableMember<TMember> where TMember : MemberInfo
	{
		public TMember Member
		{
			get { return member; }
		}

		protected readonly TMember member;

		protected InitializableMemberBase(TMember member)
		{
			this.member = member;
		}

		public abstract void Initialize(object instance, object value);
	}
}
