using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Pseudo.Pooling;

namespace Pseudo.Audio.Internal
{
	[Serializable]
	public class AudioContainerSourceData : IPoolable
	{
		public AudioSettingsBase Settings;
		public List<AudioOption> Options = new List<AudioOption>();

		public virtual void OnCreate() { }

		public virtual void OnRecycle()
		{
			//TypePoolManager.RecycleElements(Options);
		}
	}
}