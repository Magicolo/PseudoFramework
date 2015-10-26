using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using System.Runtime.Serialization;
using Pseudo.Internal.Audio;

namespace Pseudo.Internal.Audio
{
	public class AudioDynamicSettings : AudioContainerSettings, ICopyable<AudioDynamicSettings>
	{
		public override AudioItem.AudioTypes Type { get { return AudioItem.AudioTypes.Dynamic; } }

		public static AudioDynamicSettings Default = CreateInstance<AudioDynamicSettings>();

		public override void Recycle()
		{
			Pool<AudioDynamicSettings>.Recycle(this);
		}

		public override AudioSettingsBase Clone()
		{
			return Pool<AudioDynamicSettings>.Create(this);
		}

		public void Copy(AudioDynamicSettings reference)
		{
			base.Copy(reference);

		}
	}
}