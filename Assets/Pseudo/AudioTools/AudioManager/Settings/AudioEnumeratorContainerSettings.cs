using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	/// <summary>
	/// Container that steps through its sources each time it is played. 
	/// The number of time each source should be repeated is defined by the Repeats array.
	/// The CurrentIndex value and the CurrentRepeat value can be set manually (note that these values will be resetted on each play session).
	/// </summary>
	[Copy]
	public class AudioEnumeratorContainerSettings : AudioContainerSettings, ICopyable<AudioEnumeratorContainerSettings>
	{
		[Min(1)]
		public List<int> Repeats = new List<int>();
		public int CurrentIndex { get; set; }
		public int CurrentRepeat { get; set; }

		public override AudioItem.AudioTypes Type { get { return AudioItem.AudioTypes.EnumeratorContainer; } }

		public void Copy(AudioEnumeratorContainerSettings reference)
		{
			base.Copy(reference);

			CopyUtility.CopyTo(reference.Repeats, ref Repeats);
			CurrentIndex = reference.CurrentIndex;
			CurrentRepeat = reference.CurrentRepeat;
		}

		void OnEnable()
		{
			CurrentIndex = 0;
			CurrentRepeat = 0;
		}
	}
}