using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Groupingz
{
	public struct Filter : IFilter
	{
		public MatchType Match
		{
			get { return match; }
		}
		public IList<int> Identifiers
		{
			get { return identifiers; }
		}

		readonly MatchType match;
		readonly int[] identifiers;

		public Filter(MatchType match, int[] identifiers)
		{
			this.match = match;
			this.identifiers = identifiers;
		}

		public bool Equals(IFilter other)
		{
			return match == other.Match && identifiers.SequenceEqual(other.Identifiers);
		}

		public override bool Equals(object obj)
		{
			if (obj is IFilter)
				return Equals((IFilter)obj);

			return false;
		}

		public override int GetHashCode()
		{
			var hash = 1 << (int)Match;

			for (int i = 0; i < identifiers.Length; i++)
				hash ^= identifiers[i] * 397;

			return hash;
		}
	}
}
