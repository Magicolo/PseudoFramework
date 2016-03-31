using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using Pseudo.Audio.Internal;
using System.Collections.Generic;

namespace Pseudo.Audio.Internal
{
	/// <summary>
	/// Container that will only play the sources that correspond to the value stored in AudioManager.Instance.States[StateName].
	/// </summary>
	public class AudioSwitchContainerSettings : AudioContainerSettings
	{
		public string SwitchName;
		public List<int> SwitchValues = new List<int>();

		public override AudioTypes Type { get { return AudioTypes.SwitchContainer; } }
	}
}