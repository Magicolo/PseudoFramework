using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using Pseudo.Internal.Audio;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	/// <summary>
	/// Container that plays its sources one after the other with delays between each of them.
	/// Note that AudioItem.RemainingTime() will not return the correct value.
	/// </summary>
	public class AudioSequenceContainerSettings : AudioContainerSettings, ICopyable<AudioSequenceContainerSettings>
	{
		public List<double> Delays = new List<double>();

		public override AudioItem.AudioTypes Type { get { return AudioItem.AudioTypes.SequenceContainer; } }

		public override void Recycle()
		{
			Pool<AudioSequenceContainerSettings>.Recycle(this);
		}

		public override AudioSettingsBase Clone()
		{
			return Pool<AudioSequenceContainerSettings>.Create(this);
		}

		public void Copy(AudioSequenceContainerSettings reference)
		{
			base.Copy(reference);

			CopyHelper.CopyTo(reference.Delays, ref Delays);
		}
	}
}