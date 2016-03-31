using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Pseudo;

namespace Pseudo.Audio.Internal
{
	public class AudioSourceSettings : AudioSettingsBase
	{
		public override AudioTypes Type { get { return AudioTypes.Source; } }

		public AudioClip Clip;
		public AudioMixerGroup Output;
		[Clamp]
		public float PlayRangeStart;
		[Clamp]
		public float PlayRangeEnd = 1f;
		[Min]
		public int MaxInstances;

		public float GetLength()
		{
			if (Clip == null)
				return 0f;
			else
				return Clip.length * (PlayRangeEnd - PlayRangeStart);
		}
	}
}