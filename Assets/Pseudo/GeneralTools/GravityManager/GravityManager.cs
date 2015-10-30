using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class GravityManager : Singleton<GravityManager>
	{
		public enum Dimensions
		{
			_2D,
			_3D
		}

		public readonly static GlobalGravityChannel Unity = new GlobalGravityChannel(GravityChannels.Unity);
		public readonly static GlobalGravityChannel World = new GlobalGravityChannel(GravityChannels.World);
		public readonly static GlobalGravityChannel Player = new GlobalGravityChannel(GravityChannels.Player);
		public readonly static GlobalGravityChannel Enemy = new GlobalGravityChannel(GravityChannels.Enemy);
		protected readonly static List<GlobalGravityChannel> channels = new List<GlobalGravityChannel> { Unity, World, Player, Enemy };
		protected static Vector3 lastGravity;
		public static Dimensions Mode { get; set; }

		[SerializeField]
		protected Dimensions mode;

		protected override void Awake()
		{
			base.Awake();

			Mode = mode;
		}

		public static GlobalGravityChannel GetChannel(GravityChannels channel)
		{
			return channels[(int)channel];
		}

		public static Vector3 GetGravity(GravityChannels channel)
		{
			return GetChannel(channel).Gravity;
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