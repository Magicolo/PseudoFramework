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

		public void Copy(AudioMixContainerSettings reference)
		{
			base.Copy(reference);

			CopyUtility.CopyTo(reference.Delays, ref Delays);
		}
	}
}