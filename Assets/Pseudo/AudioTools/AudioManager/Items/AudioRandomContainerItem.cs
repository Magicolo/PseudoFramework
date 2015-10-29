using UnityEngine;
using System.Collections;
using Pseudo;
using System;

namespace Pseudo.Internal.Audio
{
	public class AudioRandomContainerItem : AudioContainerItem, ICopyable<AudioRandomContainerItem>
	{
		AudioRandomContainerSettings originalSettings;
		AudioRandomContainerSettings settings;

		public override AudioTypes Type { get { return AudioTypes.RandomContainer; } }
		public override AudioSettingsBase Settings { get { return settings; } }

		public static AudioRandomContainerItem Default = new AudioRandomContainerItem();

		public void Initialize(AudioRandomContainerSettings settings, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(settings.GetHashCode(), settings.Name, spatializer, parent);

			originalSettings = settings;
			this.settings = Pool<AudioRandomContainerSettings>.Create(settings);

			InitializeModifiers(originalSettings);
			InitializeSources();

			for (int i = 0; i < originalSettings.Options.Count; i++)
				ApplyOption(originalSettings.Options[i], false);
		}

		protected override void InitializeSources()
		{
			AddSource(PRandom.WeightedRandom(originalSettings.Sources, originalSettings.Weights));
		}

		protected override void Recycle()
		{
			Pool<AudioRandomContainerItem>.Recycle(this);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			Pool<AudioRandomContainerSettings>.Recycle(ref settings);
		}

		public void Copy(AudioRandomContainerItem reference)
		{
			base.Copy(reference);

			originalSettings = reference.originalSettings;
			settings = reference.settings;
		}
	}
}