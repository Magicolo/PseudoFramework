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
		public enum GravityChannels
		{
			Unity,
			World,
			Player,
			Enemy
		}

		public enum Dimensions
		{
			_2D,
			_3D
		}

		public static GravityChannel Unity { get { return GetChannel(GravityChannels.Unity); } }
		public static GravityChannel World { get { return GetChannel(GravityChannels.World); } }
		public static GravityChannel Player { get { return GetChannel(GravityChannels.Player); } }
		public static GravityChannel Enemy { get { return GetChannel(GravityChannels.Enemy); } }
		protected static List<GravityChannel> channels;
		protected static Vector3 lastGravity;
		public static Dimensions Mode { get; set; }

		[SerializeField]
		protected Dimensions mode;

		protected override void Awake()
		{
			base.Awake();

			Mode = mode;

			GravityChannels[] channelValues = (GravityChannels[])Enum.GetValues(typeof(GravityChannels));
			channels = new List<GravityChannel>(channelValues.Length);

			for (int i = 0; i < channelValues.Length; i++)
			{
				GravityChannels channelValue = channelValues[i];
				GravityChannel channel = CreateChannel(channelValue);
				channels.Add(channel);
			}
		}

		public static GravityChannel GetChannel(GravityChannels channel)
		{
			if (channels == null)
			{
				Debug.LogError("No instance of the GravityManager has been found.");
				return null;
			}

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

		static GravityChannel CreateChannel(GravityChannels channel)
		{
			return instance.CachedGameObject.AddChild(channel.ToString()).AddComponent<GravityChannel>();
		}
	}
}