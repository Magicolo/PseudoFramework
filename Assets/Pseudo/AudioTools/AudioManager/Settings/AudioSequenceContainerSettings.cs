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

		public void Copy(AudioSequenceContainerSettings reference)
		{
			base.Copy(reference);

			CopyUtility.CopyTo(reference.Delays, ref Delays);
		}
	}
}