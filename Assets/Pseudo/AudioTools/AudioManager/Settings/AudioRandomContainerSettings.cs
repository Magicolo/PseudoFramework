
using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	/// <summary>
	/// Container that will play a random source based on the Weights array.
	/// </summary>
	[Copy]
	public class AudioRandomContainerSettings : AudioContainerSettings, ICopyable<AudioRandomContainerSettings>
	{
		public List<float> Weights = new List<float>();

		public override AudioItem.AudioTypes Type { get { return AudioItem.AudioTypes.RandomContainer; } }

		public void Copy(AudioRandomContainerSettings reference)
		{
			base.Copy(reference);

			CopyUtility.CopyTo(reference.Weights, ref Weights);
		}
	}
}