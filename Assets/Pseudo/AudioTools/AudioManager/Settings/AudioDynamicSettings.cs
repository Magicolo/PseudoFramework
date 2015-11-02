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
		public static readonly AudioDynamicSettings Default = CreateInstance<AudioDynamicSettings>();

		public override AudioItem.AudioTypes Type { get { return AudioItem.AudioTypes.Dynamic; } }

		public void Copy(AudioDynamicSettings reference)
		{
			base.Copy(reference);

		}
	}
}