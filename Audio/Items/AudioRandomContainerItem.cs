using UnityEngine;
using System.Collections;
using Pseudo;
using System;
using Pseudo.Pooling;

namespace Pseudo.Audio.Internal
{
	public class AudioRandomContainerItem : AudioContainerItem, ICopyable<AudioRandomContainerItem>
	{
		AudioRandomContainerSettings originalSettings;
		AudioRandomContainerSettings settings;

		public override AudioTypes Type { get { return AudioTypes.RandomContainer; } }
		public override AudioSettingsBase Settings { get { return settings; } }

		public void Initialize(AudioRandomContainerSettings settings, AudioItemManager itemManager, AudioSpatializer spatializer, IAudioItem parent)
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
			AddSource(PRandom.WeightedRandom(originalSettings.Sources, originalSettings.Weights));
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			PrefabPoolManager.Recycle(ref settings);
		}

		public void Copy(AudioRandomContainerItem reference)
		{
			base.Copy(reference);

			originalSettings = reference.originalSettings;
			settings = reference.settings;
		}
	}
}