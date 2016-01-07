using UnityEngine;
using System.Collections;
using Pseudo;
using System;

namespace Pseudo.Internal.Audio
{
	public class AudioEnumeratorContainerItem : AudioContainerItem
	{
		AudioEnumeratorContainerSettings originalSettings;
		AudioEnumeratorContainerSettings settings;

		public override AudioTypes Type { get { return AudioTypes.EnumeratorContainer; } }
		public override AudioSettingsBase Settings { get { return settings; } }

		public void Initialize(AudioEnumeratorContainerSettings settings, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(settings.Id, settings.Name, spatializer, parent);

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

		public override void Copy(object reference)
		{
			base.Copy(reference);

			var castedReference = (AudioEnumeratorContainerItem)reference;
			originalSettings = castedReference.originalSettings;
			settings = castedReference.settings;
		}
	}
}