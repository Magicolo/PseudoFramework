using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class GravityManager : Singleton<GravityManager>
	{
		public enum Dimensions
		{
			_2D,
			_3D
		}

		public enum GravityChannels
		{
			Unity,
			World,
			Player,
			Enemy
		}

		public class GravityChannel
		{
			public GravityChannels Channel { get { return channel; } }
			public Vector3 Gravity
			{
				get
				{
					UpdateGravity();
					return gravity;
				}
			}
			public Vector3 GravityScale
			{
				get { return gravityScale; }
				set
				{
					if (gravityScale != value)
					{
						gravityScale = value;
						hasChanged = true;
					}
				}
			}
			public Vector3 Rotation
			{
				get { return rotation; }
				set
				{
					if (rotation != value)
					{
						rotation = value;
						rotationQuaternion.eulerAngles = rotation;
						hasChanged = true;
					}
				}
			}

			GravityChannels channel;
			Vector3 gravity;
			Vector3 gravityScale = new Vector3(1f, 1f, 1f);
			Vector3 rotation;
			Quaternion rotationQuaternion = Quaternion.identity;
			Vector3 lastGravity;
			bool hasChanged = true;

			public GravityChannel(GravityChannels channel)
			{
				this.channel = channel;
			}

			void UpdateGravity()
			{
				Vector3 currentGravity = GetDefaultGravity();

				if (!hasChanged && lastGravity == currentGravity)
					return;

				hasChanged = false;
				lastGravity = currentGravity;
				gravity = rotationQuaternion * lastGravity.Mult(gravityScale);
			}
		}

		public readonly static GravityChannel Unity = new GravityChannel(GravityChannels.Unity);
		public readonly static GravityChannel World = new GravityChannel(GravityChannels.World);
		public readonly static GravityChannel Player = new GravityChannel(GravityChannels.Player);
		public readonly static GravityChannel Enemy = new GravityChannel(GravityChannels.Enemy);
		protected readonly static List<GravityChannel> channels = new List<GravityChannel> { Unity, World, Player, Enemy };
		protected static Vector3 lastGravity;
		public static Dimensions Mode { get; set; }

		[SerializeField]
		protected Dimensions mode;

		protected override void Awake()
		{
			base.Awake();

			Mode = mode;
		}

		static Vector3 GetDefaultGravity()
		{
			switch (Mode)
			{
				default:
					return Physics2D.gravity;
				case Dimensions._3D:
					return Physics.gravity;
			}
		}

		public static GravityChannel GetChannel(GravityChannels channel)
		{
			return channels[(int)channel];
		}

		public static Vector3 GetGravity(GravityChannels channel)
		{
			return GetChannel(channel).Gravity;
		}

		public static Vector3 GetGravityScale(GravityChannels channel)
		{
			return GetChannel(channel).GravityScale;
		}

		public static void SetGravityScale(GravityChannels channel, Vector3 gravityScale)
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