using UnityEngine;
using System.Collections;
using Pseudo;
using System;
using Pseudo.Pooling;

namespace Pseudo.Audio.Internal
{
	public class AudioEnumeratorContainerItem : AudioContainerItem, ICopyable<AudioEnumeratorContainerItem>
	{
		AudioEnumeratorContainerSettings originalSettings;
		AudioEnumeratorContainerSettings settings;

		public override AudioTypes Type { get { return AudioTypes.EnumeratorContainer; } }
		public override AudioSettingsBase Settings { get { return settings; } }

		public void Initialize(AudioEnumeratorContainerSettings settings, AudioItemManager itemManager, AudioSpatializer spatializer, IAudioItem parent)
		{
			base.Initialize(settings.Identifier, itemManager, spatializer, parent);

			originalSettings = settings;
			this.settings = PrefabPoolManager.Create(settings);

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

		public override void OnRecycle()
		{
			base.OnRecycle();

			PrefabPoolManager.Recycle(ref settings);
		}

		public void Copy(AudioEnumeratorContainerItem reference)
		{
			base.Copy(reference);

			originalSettings = reference.originalSettings;
			settings = reference.settings;
		}

		public void CopyTo(AudioEnumeratorContainerItem instance)
		{
			instance.Copy(this);
		}
	}
}