using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Internal.Physics;

namespace Pseudo
{
	public class GravityManager
	{
		public enum GravityChannels : byte
		{
			Unity,
			Game,
			UI,
			World,
			Player,
			Enemy
		}

		public static readonly IGravityChannel Unity = new UnityGravityChannel();
		public static readonly IGravityChannel Game = new GlobalGravityChannel(GravityChannels.Game);
		public static readonly IGravityChannel UI = new GlobalGravityChannel(GravityChannels.UI);
		public static readonly IGravityChannel World = new GlobalGravityChannel(GravityChannels.World);
		public static readonly IGravityChannel Player = new GlobalGravityChannel(GravityChannels.Player);
		public static readonly IGravityChannel Enemy = new GlobalGravityChannel(GravityChannels.Enemy);

		static IGravityChannel[] channels =
		{
			Unity,
			Game,
			UI,
			World,
			Player,
			Enemy
		};

		public static IGravityChannel GetChannel(GravityChannels channel)
		{
			return channels[(byte)channel];
		}
	}
}