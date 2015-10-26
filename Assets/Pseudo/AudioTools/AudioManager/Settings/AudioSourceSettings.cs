using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Pseudo;

namespace Pseudo.Internal.Audio
{
	public class AudioSourceSettings : AudioSettingsBase, ICopyable<AudioSourceSettings>
	{
		public override AudioItem.AudioTypes Type { get { return AudioItem.AudioTypes.Source; } }

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

		public override void Recycle()
		{
			Pool<AudioSourceSettings>.Recycle(this);
		}

		public override AudioSettingsBase Clone()
		{
			return Pool<AudioSourceSettings>.Create(this);
		}

		public void Copy(AudioSourceSettings reference)
		{
			base.Copy(reference);

			Clip = reference.Clip;
			Output = reference.Output;
			PlayRangeStart = reference.PlayRangeStart;
			PlayRangeEnd = reference.PlayRangeEnd;
			MaxInstances = reference.MaxInstances;
		}
	}
}