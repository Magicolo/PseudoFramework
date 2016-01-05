using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;
using Pseudo.Internal;

namespace Pseudo
{
	public enum EntityMatches
	{
		All,
		Any,
		None,
		Exact
	}

	[Serializable]
	public struct EntityMatch
	{
		public EntityGroupDefinition Groups
		{
			get { return groups; }
		}
		public EntityMatches Match
		{
			get { return match; }
		}

		[SerializeField]
		EntityGroupDefinition groups;
		[SerializeField]
		EntityMatches match;

		public EntityMatch(EntityGroupDefinition groups, EntityMatches match = EntityMatches.All)
		{
			this.groups = groups;
			this.match = match;
		}

		public static bool Matches(ByteFlag groups1, ByteFlag groups2, EntityMatches match = EntityMatches.All)
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

		public static bool Matches(IEntity entity, ByteFlag components, EntityMatches match = EntityMatches.All)
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
					matches = !MatchesAny(entity, components);
					break;
				case EntityMatches.Exact:
					matches = MatchesExact(entity, components);
					break;
			}

			return matches;
		}

		public static bool Matches(EntityGroupDefinition groups1, EntityGroupDefinition groups2, EntityMatches match = EntityMatches.All)
		{
			bool matches = false;

			switch (match)
			{
				case EntityMatches.All:
					matches = MatchesAll(groups1.Groups, groups2.Groups);
					break;
				case EntityMatches.Any:
					matches = MatchesAny(groups1.Groups, groups2.Groups);
					break;
				case EntityMatches.None:
					matches = !MatchesAny(groups1.Groups, groups2.Groups);
					break;
				case EntityMatches.Exact:
					matches = MatchesExact(groups1.Groups, groups2.Groups);
					break;
			}

			return matches;
		}

		static bool MatchesAll(ByteFlag groups1, ByteFlag groups2)
		{
			return (~groups1 & groups2) == ByteFlag.Nothing;
		}

		static bool MatchesAny(ByteFlag groups1, ByteFlag groups2)
		{
			return (groups1 & ~groups2) != groups1;
		}

		static bool MatchesExact(ByteFlag groups1, ByteFlag groups2)
		{
			return groups1 == groups2;
		}

		static bool MatchesAll(int[] groups1, int[] groups2)
		{
			if (groups1.Length == 0)
				return false;
			else if (groups2.Length == 0)
				return true;
			else if (groups1.Length < groups2.Length)
				return false;
			else if (groups1.Length == 1 && groups2.Length == 1)
				return groups1[0] == groups2[0];
			else if (groups1[0] > groups2.Last() || groups1.Last() < groups2[0])
				return false;

			int lastId = groups1.Last();
			int lastIndex = 0;
			for (int i = 0; i < groups2.Length; i++)
			{
				int id2 = groups2[i];
				bool contains = false;

				if (id2 > lastId)
					return false;

				for (int j = lastIndex; j < groups1.Length; j++)
				{
					int id1 = groups1[j];

					if (id1 == id2)
					{
						contains = true;
						lastIndex = j + 1;
						break;
					}
				}

				if (!contains)
					return false;
			}

			return true;
		}

		static bool MatchesAny(int[] groups1, int[] groups2)
		{
			if (groups2.Length == 0)
				return true;
			else if (groups1.Length == 0)
				return false;
			else if (groups1.Length == 1 && groups2.Length == 1)
				return groups1[0] == groups2[0];
			else if (groups1[0] == groups2[0])
				return true;
			else if (groups1[0] > groups2.Last() || groups1.Last() < groups2[0])
				return false;

			int lastId = groups1.Last();
			int lastIndex = 0;
			for (int i = 0; i < groups2.Length; i++)
			{
				int id2 = groups2[i];

				if (id2 > lastId)
					return false;

				for (int j = lastIndex; j < groups1.Length; j++)
				{
					int id1 = groups1[j];

					if (id1 == id2)
						return true;
					else if (id2 > id1)
						lastIndex = j + 1;
				}
			}

			return false;
		}

		static bool MatchesExact(int[] groups1, int[] groups2)
		{
			if (groups1.Length != groups2.Length)
				return false;
			else if (groups1.Length == 0 && groups2.Length == 0)
				return true;
			else if (groups1.Length == 1 && groups2.Length == 1)
				return groups1[0] == groups2[0];

			for (int i = 0; i < groups1.Length; i++)
			{
				if (groups1[i] != groups2[i])
					return false;
			}

			return true;
		}

		static bool MatchesAll(IEntity entity, ByteFlag components)
		{
			for (byte i = 0; i < EntityUtility.IdCount; i++)
			{
				if (components[i] && !entity.HasComponent(EntityUtility.GetComponentType(i)))
					return false;
			}

			return true;
		}

		static bool MatchesAny(IEntity entity, ByteFlag components)
		{
			for (byte i = 0; i < EntityUtility.IdCount; i++)
			{
				if (components[i] && entity.HasComponent(EntityUtility.GetComponentType(i)))
					return true;
			}

			return false;
		}

		static bool MatchesExact(IEntity entity, ByteFlag components)
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