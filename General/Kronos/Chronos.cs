using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public static class Chronos
	{
		public enum TimeChannels : byte
		{
			Unity,
			Game,
			UI,
			World,
			Player,
			Enemy,
		}

		public static readonly ITimeChannel Unity = new UnityTimeChannel();
		public static readonly ITimeChannel Game = new GlobalTimeChannel(TimeChannels.Game);
		public static readonly ITimeChannel UI = new GlobalTimeChannel(TimeChannels.UI);
		public static readonly ITimeChannel World = new GlobalTimeChannel(TimeChannels.World);
		public static readonly ITimeChannel Player = new GlobalTimeChannel(TimeChannels.Player);
		public static readonly ITimeChannel Enemy = new GlobalTimeChannel(TimeChannels.Enemy);

		static readonly ITimeChannel[] channels =
		{
			Unity,
			Game,
			UI,
			World,
			Player,
			Enemy
		};

		public static ITimeChannel GetChannel(TimeChannels channel)
		{
			return channels[(byte)channel];
		}
	}
}