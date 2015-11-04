using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	[Serializable,Copy]
	public class AudioContainerSourceData : IPoolable, ICopyable<AudioContainerSourceData>
	{
		public static readonly Pool<AudioContainerSourceData> Pool = new Pool<AudioContainerSourceData>(() => new AudioContainerSourceData());

		public AudioSettingsBase Settings;
		public List<AudioOption> Options = new List<AudioOption>();

		public void OnCreate()
		{
			Settings = AudioSettingsBase.Pool.CreateCopy(Settings);
			AudioOption.Pool.CreateElements(Options);
		}

		public void OnRecycle()
		{
			AudioSettingsBase.Pool.Recycle(Settings);
			AudioOption.Pool.RecycleElements(Options);
		}

		public void Copy(AudioContainerSourceData reference)
		{
			Settings = reference.Settings;
			CopyUtility.CopyTo(reference.Options, ref Options);
		}
	}
}