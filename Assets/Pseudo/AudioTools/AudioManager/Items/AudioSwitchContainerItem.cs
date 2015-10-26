using UnityEngine;
using System.Collections;
using Pseudo;
using System;

namespace Pseudo.Internal.Audio
{
	public class AudioSwitchContainerItem : AudioContainerItem, ICopyable<AudioSwitchContainerItem>
	{
		AudioSwitchContainerSettings _originalSettings;
		AudioSwitchContainerSettings _settings;
		AudioValue<int> _switchValue;

		public override AudioTypes Type { get { return AudioTypes.SwitchContainer; } }
		public override AudioSettingsBase Settings { get { return _settings; } }

		public static AudioSwitchContainerItem Default = new AudioSwitchContainerItem();

		public void Initialize(AudioSwitchContainerSettings settings, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(settings.GetHashCode(), settings.Name, spatializer, parent);

			_originalSettings = settings;
			_settings = Pool<AudioSwitchContainerSettings>.Create(settings);

			InitializeModifiers(_originalSettings);
			InitializeSources();

			for (int i = 0; i < _originalSettings.Options.Count; i++)
				ApplyOption(_originalSettings.Options[i], false);
		}

		protected override void InitializeSources()
		{
			_switchValue = PAudio.Instance.GetSwitchValue(_settings.SwitchName);
			int stateValue = _switchValue.Value;

			for (int i = 0; i < _originalSettings.Sources.Count; i++)
			{
				if (_originalSettings.SwitchValues[i] == stateValue)
					AddSource(_originalSettings.Sources[i]);
			}
		}

		protected override void Recycle()
		{
			Pool<AudioSwitchContainerItem>.Recycle(this);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			Pool<AudioSwitchContainerSettings>.Recycle(ref _settings);
		}

		public void Copy(AudioSwitchContainerItem reference)
		{
			base.Copy(reference);

			_originalSettings = reference._originalSettings;
			_settings = reference._settings;
			_switchValue = reference._switchValue;
		}
	}
}