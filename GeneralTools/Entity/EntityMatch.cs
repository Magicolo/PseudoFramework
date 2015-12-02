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
		public ByteFlag<EntityGroups> Groups
		{
			get { return groups; }
		}
		public EntityMatches Match
		{
			get { return match; }
		}

		[SerializeField, EnumFlags(typeof(EntityGroups))]
		ByteFlag groups;
		[SerializeField]
		EntityMatches match;

		public EntityMatch(ByteFlag<EntityGroups> group, EntityMatches match = EntityMatches.All)
		{
			this.groups = group;
			this.match = match;
		}

		public static bool Matches(ByteFlag groups1, ByteFlag groups2, EntityMatches match)
		{
			bool matches = false;

			switch (match)
			{
				case EntityMatches.All:
					matches = (~groups1 & groups2) == ByteFlag.Nothing;
					break;
				case EntityMatches.Any:
					matches = (groups1 & ~groups2) != groups1;
					break;
				case EntityMatches.None:
					matches = (groups1 & ~groups2) == groups1;
					break;
				case EntityMatches.Exact:
					matches = groups1 == groups2;
					break;
			}

			return matches;
		}

		public static bool Matches(PEntity entity, ByteFlag components, EntityMatches match)
		{
			bool matches = false;

			switch (match)
			{
				case EntityMatches.All:
					matches = MatchesAll(entity, components);
					break;
				case EntityMatches.Any:
					matches = MatchesAny(entity, components);
					break;
				case EntityMatches.None:
					matches = MatchesNone(entity, components);
					break;
				case EntityMatches.Exact:
					matches = MatchesExact(entity, components);
					break;
			}

			return matches;
		}

		static bool MatchesAll(PEntity entity, ByteFlag components)
		{
			for (byte i = 0; i < EntityUtility.IdCount; i++)
			{
				if (components[i] && !entity.HasComponent(EntityUtility.GetComponentType(i)))
					return false;
			}

			return true;
		}

		static bool MatchesAny(PEntity entity, ByteFlag components)
		{
			for (byte i = 0; i < EntityUtility.IdCount; i++)
			{
				if (components[i] && entity.HasComponent(EntityUtility.GetComponentType(i)))
					return true;
			}

			return false;
		}

		static bool MatchesNone(PEntity entity, ByteFlag components)
		{
			for (byte i = 0; i < EntityUtility.IdCount; i++)
			{
				if (components[i] && entity.HasComponent(EntityUtility.GetComponentType(i)))
					return false;
			}

			return true;
		}

		static bool MatchesExact(PEntity entity, ByteFlag components)
		{
			for (byte i = 0; i < EntityUtility.IdCount; i++)
			{
				if (components[i] != entity.HasComponent(EntityUtility.GetComponentType(i)))
					return false;
			}

			return true;
		}
	}
}