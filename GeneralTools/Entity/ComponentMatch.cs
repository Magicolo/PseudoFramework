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
	public struct ComponentMatch
	{
		static readonly ComponentAllGroupMatcher allMatcher = new ComponentAllGroupMatcher();

		public BitArray TypeId;
		public EntityGroup.Matches Match;

		public static IComponentGroupMatcher GetMatcher(EntityGroup.Matches match)
		{
			IComponentGroupMatcher matcher = null;

			switch (match)
			{
				case EntityGroup.Matches.All:
					matcher = allMatcher;
					break;
				case EntityGroup.Matches.Any:
					break;
				case EntityGroup.Matches.None:
					break;
				case EntityGroup.Matches.Exact:
					break;
			}

			return matcher;
		}
	}
}