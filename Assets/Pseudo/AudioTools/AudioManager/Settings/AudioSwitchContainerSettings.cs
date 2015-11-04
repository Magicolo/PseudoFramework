using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using Pseudo.Internal.Audio;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	/// <summary>
	/// Container that will only play the sources that correspond to the value stored in AudioManager.Instance.States[StateName].
	/// </summary>
	[Copy]
	public class AudioSwitchContainerSettings : AudioContainerSettings, ICopyable<AudioSwitchContainerSettings>
	{
		public string SwitchName;
		public List<int> SwitchValues = new List<int>();

		public override AudioItem.AudioTypes Type { get { return AudioItem.AudioTypes.SwitchContainer; } }

		public void Copy(AudioSwitchContainerSettings reference)
		{
			base.Copy(reference);

			SwitchName = reference.SwitchName;
			CopyUtility.CopyTo(reference.SwitchValues, ref SwitchValues);
		}
	}
}