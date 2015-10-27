using UnityEngine;
using System.Collections;
using Pseudo;
using System;

namespace Pseudo.Internal.Audio
{
	public class AudioRandomContainerItem : AudioContainerItem, ICopyable<AudioRandomContainerItem>
	{
		AudioRandomContainerSettings _originalSettings;
		AudioRandomContainerSettings _settings;

		public override AudioTypes Type { get { return AudioTypes.RandomContainer; } }
		public override AudioSettingsBase Settings { get { return _settings; } }

		public static AudioRandomContainerItem Default = new AudioRandomContainerItem();

		public void Initialize(AudioRandomContainerSettings settings, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(settings.GetHashCode(), settings.Name, spatializer, parent);

			_originalSettings = settings;
			_settings = Pool<AudioRandomContainerSettings>.Create(settings);

			InitializeModifiers(_originalSettings);
			InitializeSources();

			for (int i = 0; i < _originalSettings.Options.Count; i++)
				ApplyOption(_originalSettings.Options[i], false);
		}

		protected override void InitializeSources()
		{
			AddSource(PRandom.WeightedRandom(_originalSettings.Sources, _originalSettings.Weights));
		}

		protected override void Recycle()
		{
			Pool<AudioRandomContainerItem>.Recycle(this);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			Pool<AudioRandomContainerSettings>.Recycle(ref _settings);
		}

		public void Copy(AudioRandomContainerItem reference)
		{
			base.Copy(reference);

			_originalSettings = reference._originalSettings;
			_settings = reference._settings;
		}
	}
}