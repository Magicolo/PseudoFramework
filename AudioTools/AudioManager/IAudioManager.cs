using Pseudo.Internal.Audio;
using System;
using UnityEngine;

namespace Pseudo
{
	public delegate AudioSettingsBase DynamicGetter(AudioDynamicItem item, AudioDynamicData data);

	public interface IAudioManager
	{
		AudioSource Reference { get; set; }
		bool UseCustomCurves { get; set; }

		IAudioItem CreateItem(AudioSettingsBase settings);
		IAudioItem CreateItem(AudioSettingsBase settings, Vector3 position);
		IAudioItem CreateItem(AudioSettingsBase settings, Transform follow);
		IAudioItem CreateItem(AudioSettingsBase settings, Func<Vector3> getPosition);
		IAudioItem CreateDynamicItem(DynamicGetter getNextSettings);
		IAudioItem CreateDynamicItem(DynamicGetter getNextSettings, Vector3 position);
		IAudioItem CreateDynamicItem(DynamicGetter getNextSettings, Transform follow);
		IAudioItem CreateDynamicItem(DynamicGetter getNextSettings, Func<Vector3> getPosition);
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