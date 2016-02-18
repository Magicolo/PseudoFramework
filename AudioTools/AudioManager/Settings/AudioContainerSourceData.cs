using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	[Serializable]
	public class AudioContainerSourceData : IPoolable
	{
		[InitializeValue]
		public AudioSettingsBase Settings;
		[InitializeContent]
		public List<AudioOption> Options = new List<AudioOption>();

		public virtual void OnCreate() { }

		public virtual void OnRecycle()
		{
			TypePoolManager.RecycleElements(Options);
		}
	}
}