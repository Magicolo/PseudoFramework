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
		public enum GravityChannels
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

		static List<IGravityChannel> channels = new List<IGravityChannel>
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
			return channels[(int)channel];
		}

		public static Vector3 GetGravity(GravityChannels channel)
		{
			return GetChannel(channel).Gravity;
		}

		public static Vector2 GetGravity2D(GravityChannels channel)
		{
			return GetChannel(channel).Gravity2D;
		}

		public static float GetGravityScale(GravityChannels channel)
		{
			return GetChannel(channel).GravityScale;
		}

		public static void SetGravityScale(GravityChannels channel, float gravityScale)
		{
			GetChannel(channel).GravityScale = gravityScale;
		}

		public static Vector3 GetRotation(GravityChannels channel)
		{
			return GetChannel(channel).Rotation;
		}

		public static void SetRotation(GravityChannels channel, Vector3 rotation)
		{
			GetChannel(channel).Rotation = rotation;
		}
	}
}