using UnityEngine;
using System.Collections;
using Pseudo;
using System;
using Pseudo.Pooling;

namespace Pseudo.Audio.Internal
{
	public class AudioSwitchContainerItem : AudioContainerItem, ICopyable<AudioSwitchContainerItem>
	{
		AudioSwitchContainerSettings originalSettings;
		AudioSwitchContainerSettings settings;
		AudioValue<int> switchValue;

		public override AudioTypes Type { get { return AudioTypes.SwitchContainer; } }
		public override AudioSettingsBase Settings { get { return settings; } }


		public void Initialize(AudioSwitchContainerSettings settings, AudioItemManager itemManager, AudioSpatializer spatializer, IAudioItem parent)
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
			switchValue = itemManager.AudioManager.GetSwitchValue(settings.SwitchName);
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

			PrefabPoolManager.Recycle(ref settings);
		}

		public void Copy(AudioSwitchContainerItem reference)
		{
			base.Copy(reference);

			originalSettings = reference.originalSettings;
			settings = reference.settings;
			switchValue = reference.switchValue;
		}

		public void CopyTo(AudioSwitchContainerItem instance)
		{
			instance.Copy(this);
		}
	}
}