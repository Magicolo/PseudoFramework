using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Internal.Audio;
using Pseudo.Internal.Pool;
using UnityEngine.Assertions;

namespace Pseudo
{
	// TODO Show container types better in container inspectors
	// TODO Uniformize RTPCValues and SwitchValues
	// TODO Find a clean way to limit instances of multiple Settings together (using the same ids)
	// TODO AudioSettings editors should all have unique colors/icons
	// TODO Add random selection types in AudioRandomContainerSettings
	// FIXME Reordering AudioOption doesn't work
	// FIXME Minor editor issue: when scrollbar is visible, AudioOption and AudioRTPC are partially under it
	public class AudioManager : MonoBehaviour, IAudioManager
	{
		[SerializeField]
		AudioSource reference;
		[SerializeField]
		bool useCustomCurves = true;
		AudioItemManager itemManager;

		Dictionary<string, AudioValue<int>> switchValues = new Dictionary<string, AudioValue<int>>();

		/// <summary>
		/// If you use custom curves in the Reference AudioSource, set this to true otherwise, leave it to false to save useless memory allocations.
		/// </summary>
		public bool UseCustomCurves
		{
			get { return useCustomCurves; }
			set { useCustomCurves = value; }
		}

		/// <summary>
		/// Default setup for AudioSources.
		/// </summary>
		public AudioSource Reference
		{
			get
			{
				if (reference == null)
					Initialize();

				return reference;
			}
		}

		public AudioManager()
		{
			itemManager = new AudioItemManager(this);
		}

		void Awake()
		{
			Initialize();
		}

		void OnDestroy()
		{
			StopAllItemsImmediate();
		}

		void Reset()
		{
			Initialize();
		}

		void Initialize()
		{
			if (this == null)
				return;

			reference = gameObject.FindOrAddChild("Reference").GetOrAddComponent<AudioSource>();
			reference.gameObject.SetActive(false);
			reference.playOnAwake = false;
			reference.spatialBlend = 1f;
		}

		public void Update()
		{
			itemManager.Update();
		}

		/// <summary>
		/// Creates a non spatialized AudioItem that corresponds to the type of the <paramref name="settings"/>.
		/// </summary>
		/// <param name="settings">Settings that will define the behaviour of the AudioItem.</param>
		/// <returns></returns>
		public IAudioItem CreateItem(AudioSettingsBase settings)
		{
			Assert.IsNotNull(settings);
			return itemManager.CreateItem(settings);
		}

		/// <summary>
		/// Creates an AudioItem spatialized at the provided <paramref name="position"/> that corresponds to the type of the <paramref name="settings"/>.
		/// </summary>
		/// <param name="settings">Settings that will define the behaviour of the AudioItem. </param>
		/// <param name="position">Position at which to place the AudioSource.</param>
		/// <returns></returns>
		public IAudioItem CreateItem(AudioSettingsBase settings, Vector3 position)
		{
			Assert.IsNotNull(settings);
			return itemManager.CreateItem(settings, position);
		}

		/// <summary>
		/// Creates an AudioItem dynamicaly spatialized around the provided Transform that corresponds to the type of the <paramref name="settings"/>.
		/// If the Transform ever becomes <code>null</code>, the AudioItem will simply stop moving.
		/// </summary>
		/// <param name="settings">Settings that will define the behaviour of the AudioItem. </param>
		/// <param name="follow">Transform the the AudioSource will follow.</param>
		/// <returns></returns>
		public IAudioItem CreateItem(AudioSettingsBase settings, Transform follow)
		{
			Assert.IsNotNull(settings);
			Assert.IsNotNull(follow);
			return itemManager.CreateItem(settings, follow);
		}

		/// <summary>
		/// Creates an AudioItem dynamicaly spatialized using the <paramref name="getPosition"/> callback to set its position that corresponds to the type of the <paramref name="settings"/>.
		/// </summary>
		/// <param name="settings">Settings that will define the behaviour of the AudioItem. </param>
		/// <param name="getPosition">Callback that will be used to update the AudioSource's position.</param>
		/// <returns></returns>
		public IAudioItem CreateItem(AudioSettingsBase settings, Func<Vector3> getPosition)
		{
			Assert.IsNotNull(settings);
			Assert.IsNotNull(getPosition);
			return itemManager.CreateItem(settings, getPosition);
		}

		/// <summary>
		/// Creates a non spatialized AudioDynamicItem that corresponds to the type of the <paramref name="settings"/>.
		/// The AudioDynamicItem will require new AudioSettingsBase when appropriate.
		/// The play behaviour of the AudioSettingsBase can be defined via the provided AudioDynamicData object.
		/// </summary>
		/// <param name="getNextSettings">Delegate that will be called when the AudioItem requires its next AudioSettingsBase.</param>
		/// <returns></returns>
		public IAudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings)
		{
			Assert.IsNotNull(getNextSettings);
			return itemManager.CreateDynamicItem(getNextSettings);
		}

		/// <summary>
		/// Creates an AudioItem spatialized at the provided <paramref name="position"/> that corresponds to the type of the <paramref name="settings"/>.
		/// The AudioDynamicItem will require new AudioSettingsBase when appropriate.
		/// The play behaviour of the AudioSettingsBase can be defined via the provided AudioDynamicData object.
		/// </summary>
		/// <param name="getNextSettings">Delegate that will be called when the AudioItem requires its next AudioSettingsBase.</param>
		/// <param name="position">Position at which to place the AudioSource.</param>
		/// <returns></returns>
		public IAudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Vector3 position)
		{
			Assert.IsNotNull(getNextSettings);
			return itemManager.CreateDynamicItem(getNextSettings, position);
		}

		/// <summary>
		/// Creates an AudioItem dynamicaly spatialized around the provided Transform that corresponds to the type of the <paramref name="settings"/>.
		/// If the Transform ever becomes <code>null</code>, the AudioItem will simply stop moving.
		/// The AudioDynamicItem will require new AudioSettingsBase when appropriate.
		/// The play behaviour of the AudioSettingsBase can be defined via the provided AudioDynamicData object.
		/// </summary>
		/// <param name="getNextSettings">Delegate that will be called when the AudioItem requires its next AudioSettingsBase.</param>
		/// <param name="follow">Transform the the AudioSource will follow.</param>
		/// <returns></returns>
		public IAudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Transform follow)
		{
			Assert.IsNotNull(getNextSettings);
			Assert.IsNotNull(follow);
			return itemManager.CreateDynamicItem(getNextSettings, follow);
		}

		/// <summary>
		/// Creates an AudioItem dynamicaly spatialized using the <paramref name="getPosition"/> callback to set its position that corresponds to the type of the <paramref name="settings"/>.
		/// The AudioDynamicItem will require new AudioSettingsBase when appropriate.
		/// The play behaviour of the AudioSettingsBase can be defined via the provided AudioDynamicData object.
		/// </summary>
		/// <param name="getNextSettings">Delegate that will be called when the AudioItem requires its next AudioSettingsBase.</param>
		/// <param name="getPosition">Callback that will be used to update the AudioSource's position.</param>
		/// <returns></returns>
		public IAudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Func<Vector3> getPosition)
		{
			Assert.IsNotNull(getNextSettings);
			Assert.IsNotNull(getPosition);
			return itemManager.CreateDynamicItem(getNextSettings, getPosition);
		}

		public void StopItems(AudioSettingsBase settings)
		{
			Assert.IsNotNull(settings);
			itemManager.StopItemsWithId(settings.Id);
		}

		public void StopItemsImmediate(AudioSettingsBase settings)
		{
			Assert.IsNotNull(settings);
			itemManager.StopItemsWithIdImmediate(settings.Id);
		}

		public void StopItems(int id)
		{
			itemManager.StopItemsWithId(id);
		}

		public void StopItemsImmediate(int id)
		{
			itemManager.StopItemsWithIdImmediate(id);
		}

		/// <summary>
		/// Stops all active AudioItems
		/// </summary>
		public void StopAllItems()
		{
			itemManager.StopAllItems();
		}

		public void StopAllItemsImmediate()
		{
			itemManager.StopAllItemsImmediate();
		}

		/// <summary>
		/// Gets an AudioValue containing the current switch value.
		/// </summary>
		/// <param name="name">The name of the switch.</param>
		/// <returns>The AudioValue.</returns>
		public AudioValue<int> GetSwitchValue(string name)
		{
			AudioValue<int> value;

			if (!switchValues.TryGetValue(name, out value))
			{
				value = TypePoolManager.Create<AudioValue<int>>();
				switchValues[name] = value;
			}

			return value;
		}

		/// <summary>
		/// Sets a switch value.
		/// </summary>
		/// <param name="name">The name of the switch.</param>
		/// <param name="value">The value to which the switch will be set to.</param>
		public void SetSwitchValue(string name, int value)
		{
			GetSwitchValue(name).Value = value;
		}
	}
}