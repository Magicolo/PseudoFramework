using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using System.Runtime.Serialization;
using Pseudo.Audio.Internal;

namespace Pseudo.Audio.Internal
{
	public class AudioDynamicSettings : AudioContainerSettings
	{
		public override AudioTypes Type { get { return AudioTypes.Dynamic; } }
	}
}