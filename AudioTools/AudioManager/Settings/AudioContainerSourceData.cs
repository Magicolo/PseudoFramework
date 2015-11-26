using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	[Serializable]
	public class AudioContainerSourceData : IPoolable, ICopyable
	{
		public AudioSettingsBase Settings;
		public List<AudioOption> Options = new List<AudioOption>();

		public virtual void OnCreate() { }

		public virtual void OnRecycle()
		{
			PrefabPoolManager.Recycle(Settings);
			TypePoolManager.RecycleElements(Options);
		}

		public virtual void Copy(object reference)
		{
			var castedReference = (AudioContainerSourceData)reference;
			Settings = castedReference.Settings;
			TypePoolManager.CreateCopies(Options, castedReference.Options);
		}
	}
}