using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public class AudioSourceCopyer : Copyer<AudioSource>
	{
		public override void CopyTo(AudioSource source, AudioSource target)
		{
			target.Copy(source);
		}
	}
}
