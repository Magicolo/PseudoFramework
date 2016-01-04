using UnityEngine;
using System.Collections;
using Pseudo;
using System;

namespace Pseudo.Internal.Audio
{
	public class AudioRandomContainerItem : AudioContainerItem
	{
		AudioRandomContainerSettings originalSettings;
		AudioRandomContainerSettings settings;

		public override AudioTypes Type { get { return AudioTypes.RandomContainer; } }
		public override AudioSettingsBase Settings { get { return settings; } }

		public void Initialize(AudioRandomContainerSettings settings, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(settings.GetHashCode(), settings.Name, spatializer, parent);

			originalSettings = settings;
			this.settings = PrefabPoolManager.Create(settings);

			InitializeModifiers(originalSettings);
			InitializeSources();

			for (int i = 0; i < originalSettings.Options.Count; i++)
				ApplyOption(originalSettings.Options[i], false);
		}

		protected override void InitializeSources()
		{
			AddSource(PRandom.WeightedRandom(originalSettings.Sources, originalSettings.Weights));
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			PrefabPoolManager.Recycle(ref settings);
		}

		public override void Copy(object reference)
		{
			base.Copy(reference);

			var castedReference = (AudioRandomContainerItem)reference;
			originalSettings = castedReference.originalSettings;
			settings = castedReference.settings;
		}
	}
}