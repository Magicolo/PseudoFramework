using UnityEngine;
using System.Collections;
using Pseudo;
using System;

namespace Pseudo.Internal.Audio
{
	public class AudioEnumeratorContainerItem : AudioContainerItem, ICopyable<AudioEnumeratorContainerItem>
	{
		AudioEnumeratorContainerSettings _originalSettings;
		AudioEnumeratorContainerSettings _settings;

		public override AudioTypes Type { get { return AudioTypes.EnumeratorContainer; } }
		public override AudioSettingsBase Settings { get { return _settings; } }

		public static AudioEnumeratorContainerItem Default = new AudioEnumeratorContainerItem();

		public void Initialize(AudioEnumeratorContainerSettings settings, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(settings.GetHashCode(), settings.Name, spatializer, parent);

			_originalSettings = settings;
			_settings = Pool<AudioEnumeratorContainerSettings>.Create(settings);

			InitializeModifiers(_originalSettings);
			InitializeSources();

			for (int i = 0; i < _originalSettings.Options.Count; i++)
				ApplyOption(_originalSettings.Options[i], false);
		}

		protected override void InitializeSources()
		{
			if (_originalSettings.CurrentRepeat >= _originalSettings.Repeats[_originalSettings.CurrentIndex])
			{
				_originalSettings.CurrentIndex = (_originalSettings.CurrentIndex + 1) % _originalSettings.Sources.Count;
				_originalSettings.CurrentRepeat = 0;
			}

			AddSource(_originalSettings.Sources[_originalSettings.CurrentIndex]);
			_originalSettings.CurrentRepeat++;
		}

		protected override void Recycle()
		{
			Pool<AudioEnumeratorContainerItem>.Recycle(this);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			Pool<AudioEnumeratorContainerSettings>.Recycle(ref _settings);
		}

		public void Copy(AudioEnumeratorContainerItem reference)
		{
			base.Copy(reference);

			_originalSettings = reference._originalSettings;
			_settings = reference._settings;
		}
	}
}