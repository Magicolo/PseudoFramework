using UnityEngine;
using System.Collections;
using Pseudo;
using System;

namespace Pseudo.Internal.Audio
{
	public class AudioSwitchContainerItem : AudioContainerItem, ICopyable<AudioSwitchContainerItem>
	{
		AudioSwitchContainerSettings originalSettings;
		AudioSwitchContainerSettings settings;
		AudioValue<int> switchValue;

		public override AudioTypes Type { get { return AudioTypes.SwitchContainer; } }
		public override AudioSettingsBase Settings { get { return settings; } }

		public static AudioSwitchContainerItem Default = new AudioSwitchContainerItem();

		public void Initialize(AudioSwitchContainerSettings settings, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(settings.GetHashCode(), settings.Name, spatializer, parent);

			originalSettings = settings;
			this.settings = Pool<AudioSwitchContainerSettings>.Create(settings);

			InitializeModifiers(originalSettings);
			InitializeSources();

			for (int i = 0; i < originalSettings.Options.Count; i++)
				ApplyOption(originalSettings.Options[i], false);
		}

		protected override void InitializeSources()
		{
			switchValue = AudioManager.Instance.GetSwitchValue(settings.SwitchName);
			int stateValue = switchValue.Value;

			for (int i = 0; i < originalSettings.Sources.Count; i++)
			{
				if (originalSettings.SwitchValues[i] == stateValue)
					AddSource(originalSettings.Sources[i]);
			}
		}

		protected override void Recycle()
		{
			Pool<AudioSwitchContainerItem>.Recycle(this);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			Pool<AudioSwitchContainerSettings>.Recycle(ref settings);
		}

		public void Copy(AudioSwitchContainerItem reference)
		{
			base.Copy(reference);

			originalSettings = reference.originalSettings;
			settings = reference.settings;
			switchValue = reference.switchValue;
		}
	}
}