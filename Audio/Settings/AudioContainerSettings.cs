using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Pseudo.Pooling;

namespace Pseudo.Audio.Internal
{
	public abstract class AudioContainerSettings : AudioSettingsBase
	{
		public List<AudioContainerSourceData> Sources = new List<AudioContainerSourceData>();

		public override void OnRecycle()
		{
			base.OnRecycle();

			//TypePoolManager.RecycleElements(Sources);
		}
	}
}