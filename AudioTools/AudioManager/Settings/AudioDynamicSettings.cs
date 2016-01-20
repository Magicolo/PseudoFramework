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
		public override AudioTypes Type { get { return AudioTypes.Dynamic; } }
	}
}