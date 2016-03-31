using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using System.Collections.Generic;

namespace Pseudo.Audio.Internal
{
	/// <summary>
	/// Container that will play a random source based on the Weights array.
	/// </summary>
	public class AudioRandomContainerSettings : AudioContainerSettings
	{
		public List<float> Weights = new List<float>();

		public override AudioTypes Type { get { return AudioTypes.RandomContainer; } }
	}
}