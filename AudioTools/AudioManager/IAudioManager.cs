using Pseudo.Internal.Audio;
using System;
using UnityEngine;

namespace Pseudo
{
	public interface IAudioManager
	{
		AudioSource Reference { get; set; }
		bool UseCustomCurves { get; set; }

		IAudioItem CreateItem(AudioSettingsBase settings);
		IAudioItem CreateItem(AudioSettingsBase settings, Vector3 position);
		IAudioItem CreateItem(AudioSettingsBase settings, Transform follow);
		IAudioItem CreateItem(AudioSettingsBase settings, Func<Vector3> getPosition);
		IAudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings);
		IAudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Vector3 position);
		IAudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Transform follow);
		IAudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Func<Vector3> getPosition);
		AudioValue<int> GetSwitchValue(string name);
		void SetSwitchValue(string name, int value);
		void StopItems(int id);
		void StopItems(AudioSettingsBase settings);
		void StopAllItems();
		void StopAllItemsImmediate();
		void StopItemsImmediate(int id);
		void StopItemsImmediate(AudioSettingsBase settings);
	}
}