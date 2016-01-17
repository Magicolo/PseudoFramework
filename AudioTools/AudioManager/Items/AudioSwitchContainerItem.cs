using UnityEngine;
using System.Collections;
using Pseudo;
using System;

namespace Pseudo.Internal.Audio
{
	public class AudioSwitchContainerItem : AudioContainerItem
	{
		AudioSwitchContainerSettings originalSettings;
		AudioSwitchContainerSettings settings;
		AudioValue<int> switchValue;

		public override AudioTypes Type { get { return AudioTypes.SwitchContainer; } }
		public override AudioSettingsBase Settings { get { return settings; } }


		public void Initialize(AudioSwitchContainerSettings settings, AudioItemManager itemManager, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(settings.Id, settings.Name, itemManager, spatializer, parent);

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

		public override void Copy(object reference)
		{
			base.Copy(reference);

			var castedReference = (AudioSwitchContainerItem)reference;
			originalSettings = castedReference.originalSettings;
			settings = castedReference.settings;
			switchValue = castedReference.switchValue;
		}
	}
}