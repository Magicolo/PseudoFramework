using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using System.Runtime.Serialization;
using Pseudo.Internal.Audio;

namespace Pseudo.Internal.Audio
{
	public class AudioDynamicSettings : AudioContainerSettings
	{
		public override AudioItem.AudioTypes Type { get { return AudioItem.AudioTypes.Dynamic; } }
	}
}