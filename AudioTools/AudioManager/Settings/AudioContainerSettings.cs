using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	public abstract class AudioContainerSettings : AudioSettingsBase
	{
		[InitializeContent]
		public List<AudioContainerSourceData> Sources = new List<AudioContainerSourceData>();

		public override void OnRecycle()
		{
			base.OnRecycle();

			TypePoolManager.RecycleElements(Sources);
		}
	}
}