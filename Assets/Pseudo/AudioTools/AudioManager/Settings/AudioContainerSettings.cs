using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	public abstract class AudioContainerSettings : AudioSettingsBase, ICopyable<AudioContainerSettings>
	{
		public List<AudioContainerSourceData> Sources = new List<AudioContainerSourceData>();

		public override void OnCreate()
		{
			base.OnCreate();

			Pool<AudioContainerSourceData>.CreateElements(Sources);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			Pool<AudioContainerSourceData>.RecycleElements(Sources);
		}

		public void Copy(AudioContainerSettings reference)
		{
			base.Copy(reference);

			CopyUtility.CopyTo(reference.Sources, ref Sources);
		}
	}
}