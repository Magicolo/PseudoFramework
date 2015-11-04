using UnityEngine;
using System.Collections;
using Pseudo;
using System;

namespace Pseudo.Internal.Audio
{
	[Copy]
	public class AudioRandomContainerItem : AudioContainerItem, ICopyable<AudioRandomContainerItem>
	{
		public static readonly AudioRandomContainerItem Default = new AudioRandomContainerItem();

		AudioRandomContainerSettings originalSettings;
		AudioRandomContainerSettings settings;

		public override AudioTypes Type { get { return AudioTypes.RandomContainer; } }
		public override AudioSettingsBase Settings { get { return settings; } }

		public void Initialize(AudioRandomContainerSettings settings, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(settings.GetHashCode(), settings.Name, spatializer, parent);

			originalSettings = settings;
			this.settings = AudioSettingsBase.Pool.CreateCopy(settings);

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

			AudioSettingsBase.Pool.Recycle(settings);
		}

		public void Copy(AudioRandomContainerItem reference)
		{
			base.Copy(reference);

			originalSettings = reference.originalSettings;
			settings = reference.settings;
		}
	}
}