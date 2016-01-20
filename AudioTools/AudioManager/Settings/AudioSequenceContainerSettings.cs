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
	public class AudioSequenceContainerSettings : AudioContainerSettings
	{
		public List<double> Delays = new List<double>();

		public override AudioTypes Type { get { return AudioTypes.SequenceContainer; } }
	}
}