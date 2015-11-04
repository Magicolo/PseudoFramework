using UnityEngine;
using System.Collections;
using Pseudo;
using System;

namespace Pseudo.Internal.Audio
{
	[Copy]
	public class AudioSwitchContainerItem : AudioContainerItem, ICopyable<AudioSwitchContainerItem>
	{
		public static readonly AudioSwitchContainerItem Default = new AudioSwitchContainerItem();

		AudioSwitchContainerSettings originalSettings;
		AudioSwitchContainerSettings settings;
		AudioValue<int> switchValue;

		public override AudioTypes Type { get { return AudioTypes.SwitchContainer; } }
		public override AudioSettingsBase Settings { get { return settings; } }

		public void Initialize(AudioSwitchContainerSettings settings, AudioSpatializer spatializer, AudioItem parent)
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
			switchValue = AudioManager.Instance.GetSwitchValue(settings.SwitchName);
			int stateValue = switchValue.Value;

			for (int i = 0; i < originalSettings.Sources.Count; i++)
			{
				if (originalSettings.SwitchValues[i] == stateValue)
					AddSource(originalSettings.Sources[i]);
			}
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			AudioSettingsBase.Pool.Recycle(settings);
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