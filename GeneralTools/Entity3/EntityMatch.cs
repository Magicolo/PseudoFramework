using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;
using Pseudo.Internal;

namespace Pseudo.Internal.Entity3
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
		public ByteFlag Groups
		{
			get { return groups; }
		}
		public EntityMatches Match
		{
			get { return match; }
		}

		[SerializeField, EntityGroups]
		ByteFlag groups;
		[SerializeField]
		EntityMatches match;

		public EntityMatch(ByteFlag groups, EntityMatches match = EntityMatches.All)
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
					matches = MatchesAll(groups1, groups2);
					break;
				case EntityMatches.Any:
					matches = MatchesAny(groups1, groups2);
					break;
				case EntityMatches.None:
					matches = !MatchesAny(groups1, groups2);
					break;
				case EntityMatches.Exact:
					matches = MatchesExact(groups1, groups2);
					break;
			}

			return matches;
		}

		public static bool Matches(IList<int> groups1, int[] groups2, EntityMatches match = EntityMatches.All)
		{
			bool matches = false;

			switch (match)
			{
				case EntityMatches.All:
					matches = MatchesAll(groups1, groups2);
					break;
				case EntityMatches.Any:
					matches = MatchesAny(groups1, groups2);
					break;
				case EntityMatches.None:
					matches = !MatchesAny(groups1, groups2);
					break;
				case EntityMatches.Exact:
					matches = MatchesExact(groups1, groups2);
					break;
			}

			return matches;
		}

		public static bool Matches(IEntity entity, int[] componentIndices, EntityMatches match = EntityMatches.All)
		{
			return Matches(entity.GetComponentIndices(), componentIndices, match);
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

		static bool MatchesAll(IList<int> groups1, int[] groups2)
		{
			if (groups2.Length == 0)
				return true;
			if (groups1.Count < groups2.Length)
				return false;
			else if (groups1.Count == 1 && groups2.Length == 1)
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

				for (int j = lastIndex; j < groups1.Count; j++)
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

		static bool MatchesAny(IList<int> groups1, int[] groups2)
		{
			if (groups2.Length == 0)
				return true;
			else if (groups1.Count == 0)
				return false;
			else if (groups1.Count == 1 && groups2.Length == 1)
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

				for (int j = lastIndex; j < groups1.Count; j++)
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

		static bool MatchesExact(IList<int> groups1, int[] groups2)
		{
			if (groups1.Count != groups2.Length)
				return false;
			else if (groups1.Count == 0 && groups2.Length == 0)
				return true;
			else if (groups1.Count == 1 && groups2.Length == 1)
				return groups1[0] == groups2[0];

			for (int i = 0; i < groups1.Count; i++)
			{
				if (groups1[i] != groups2[i])
					return false;
			}

			return true;
		}
	}
}