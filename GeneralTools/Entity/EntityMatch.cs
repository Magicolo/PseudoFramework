using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	[Serializable]
	public struct EntityMatch
	{
		static readonly EntityAllGroupMatcher allMatcher = new EntityAllGroupMatcher();
		static readonly EntityAnyGroupMatcher anyMatcher = new EntityAnyGroupMatcher();
		static readonly EntityNoneGroupMatcher noneMatcher = new EntityNoneGroupMatcher();
		static readonly EntityExactGroupMatcher exactMatcher = new EntityExactGroupMatcher();

		public EntityGroup.Groups Group
		{
			get { return (EntityGroup.Groups)group; }
		}
		public EntityGroup.Matches Match
		{
			get { return match; }
		}

		[SerializeField, EnumFlags(typeof(EntityGroup.Groups))]
		ulong group;
		[SerializeField]
		EntityGroup.Matches match;

		public static IEntityGroupMatcher GetMatcher(EntityGroup.Matches match)
		{
			IEntityGroupMatcher matcher = null;

			switch (match)
			{
				case EntityGroup.Matches.All:
					matcher = allMatcher;
					break;
				case EntityGroup.Matches.Any:
					matcher = anyMatcher;
					break;
				case EntityGroup.Matches.None:
					matcher = noneMatcher;
					break;
				case EntityGroup.Matches.Exact:
					matcher = exactMatcher;
					break;
			}

			return matcher;
		}

		public EntityMatch(EntityGroup.Groups group, EntityGroup.Matches match = EntityGroup.Matches.All)
		{
			this.group = (ulong)group;
			this.match = match;
		}
	}
}