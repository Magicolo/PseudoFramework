using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	/// <summary>
	/// Container that will play all its sources after their delay.
	/// </summary>
	public class AudioMixContainerSettings : AudioContainerSettings, ICopyable<AudioMixContainerSettings>
	{
		[Min]
		public List<double> Delays = new List<double>();

		public override AudioItem.AudioTypes Type { get { return AudioItem.AudioTypes.MixContainer; } }

		public override void Recycle()
		{
			Pool<AudioMixContainerSettings>.Recycle(this);
		}

		public override AudioSettingsBase Clone()
		{
			return Pool<AudioMixContainerSettings>.Create(this);
		}

		public void Copy(AudioMixContainerSettings reference)
		{
			base.Copy(reference);

			CopyHelper.CopyTo(reference.Delays, ref Delays);
		}
	}
}