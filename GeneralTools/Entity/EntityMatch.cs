using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public struct EntityMatch
	{
		[Flags]
		public enum Groups : ulong
		{
			Player = 1 << 0,
			Enemy = 1 << 1,
		}

		public enum Matches
		{
			All,
			Any,
			None,
			Exact
		}

		public Groups Group
		{
			get { return (Groups)group; }
		}
		public Matches Match
		{
			get { return match; }
		}

		[SerializeField, Flag(typeof(Groups))]
		ulong group;
		[SerializeField]
		Matches match;

		public EntityMatch(Groups group, Matches match = Matches.All)
		{
			this.group = (ulong)group;
			this.match = match;
		}
	}
}