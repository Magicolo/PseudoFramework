using UnityEngine;
using System.Collections;
using Pseudo;
using System;

namespace Pseudo.Internal.Audio
{
	public class AudioEnumeratorContainerItem : AudioContainerItem, ICopyable<AudioEnumeratorContainerItem>
	{
		AudioEnumeratorContainerSettings originalSettings;
		AudioEnumeratorContainerSettings settings;

		public override AudioTypes Type { get { return AudioTypes.EnumeratorContainer; } }
		public override AudioSettingsBase Settings { get { return settings; } }

		public static AudioEnumeratorContainerItem Default = new AudioEnumeratorContainerItem();

		public void Initialize(AudioEnumeratorContainerSettings settings, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(settings.GetHashCode(), settings.Name, spatializer, parent);

			originalSettings = settings;
			this.settings = Pool<AudioEnumeratorContainerSettings>.Create(settings);

			InitializeModifiers(originalSettings);
			InitializeSources();

			for (int i = 0; i < originalSettings.Options.Count; i++)
				ApplyOption(originalSettings.Options[i], false);
		}

		protected override void InitializeSources()
		{
			if (originalSettings.CurrentRepeat >= originalSettings.Repeats[originalSettings.CurrentIndex])
			{
				originalSettings.CurrentIndex = (originalSettings.CurrentIndex + 1) % originalSettings.Sources.Count;
				originalSettings.CurrentRepeat = 0;
			}

			AddSource(originalSettings.Sources[originalSettings.CurrentIndex]);
			originalSettings.CurrentRepeat++;
		}

		protected override void Recycle()
		{
			Pool<AudioEnumeratorContainerItem>.Recycle(this);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			Pool<AudioEnumeratorContainerSettings>.Recycle(ref settings);
		}

		public void Copy(AudioEnumeratorContainerItem reference)
		{
			base.Copy(reference);

			originalSettings = reference.originalSettings;
			settings = reference.settings;
		}
	}
}