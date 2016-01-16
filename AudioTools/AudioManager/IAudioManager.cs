using Pseudo.Internal.Audio;
using System;
using UnityEngine;

namespace Pseudo
{
	public interface IAudioManager
	{
		AudioSource Reference { get; }
		bool UseCustomCurves { get; set; }
		AudioItemManager ItemManager { get; }

		AudioItem CreateItem(AudioSettingsBase settings);
		AudioItem CreateItem(AudioSettingsBase settings, Vector3 position);
		AudioItem CreateItem(AudioSettingsBase settings, Transform follow);
		AudioItem CreateItem(AudioSettingsBase settings, Func<Vector3> getPosition);

		AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings);
		AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Vector3 position);
		AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Transform follow);
		AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Func<Vector3> getPosition);

		AudioValue<int> GetSwitchValue(string name);
		void SetSwitchValue(string name, int value);
		void Stop(int id);
		void Stop(AudioSettingsBase settings);
		void StopAll();
		void StopAllImmediate();
		void StopImmediate(int id);
		void StopImmediate(AudioSettingsBase settings);
	}
}