using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Internal.Entity;

namespace Pseudo.Internal
{
	[Serializable]
	public abstract class GroupDefinition : IEquatable<GroupDefinition>
	{
		public ByteFlag Groups { get { return groups; } }

		[SerializeField]
		protected ByteFlag groups = ByteFlag.Nothing;

		protected GroupDefinition(ByteFlag groups)
		{
			this.groups = groups;
		}

		protected GroupDefinition(params byte[] groupIds) : this(new ByteFlag(groupIds)) { }

		public bool Equals(GroupDefinition other)
		{
			return groups == other.groups;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is GroupDefinition))
				return false;

			return Equals((GroupDefinition)obj);
		}

		public override int GetHashCode()
		{
			return groups.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, groups);
		}
	}
}

namespace Pseudo
{
	[Serializable]
	public class EntityGroupDefinition : GroupDefinition, IEquatable<EntityGroupDefinition>
	{
		public static readonly EntityGroupDefinition Empty = new EntityGroupDefinition();

		public EntityGroupDefinition(params byte[] groupIds) : base(groupIds) { }

		public bool Equals(EntityGroupDefinition other)
		{
			return base.Equals(other);
		}
	}

	public class ComponentGroupDefinition : GroupDefinition, IEquatable<ComponentGroupDefinition>
	{
		public ComponentGroupDefinition(params Type[] componentTypes) : base(EntityUtility.GetComponentFlags(componentTypes)) { }

		public bool Equals(ComponentGroupDefinition other)
		{
			return base.Equals(other);
		}
	}
}