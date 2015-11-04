using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Internal.Audio;

namespace Pseudo
{
	// TODO Show container types better in container inspectors
	// TODO Uniformize RTPCValues and SwitchValues
	// TODO Find a clean way to limit instances of multiple Settings together
	// TODO AudioSettings editors should all have unique colors/icons
	// TODO Add random selection types in AudioRandomContainerSettings
	// FIXME Reordering AudioOption doesn't work
	// FIXME Minor editor issue: when scrollbar is visible, AudioOption and AudioRTPC are partially under it
	public class AudioManager : Singleton<AudioManager>
	{
		[SerializeField]
		AudioSource reference;
		ComponentPool<AudioSource> audioSourcePool;
		AudioItemManager itemManager = new AudioItemManager();

		Dictionary<string, AudioValue<int>> switchValues = new Dictionary<string, AudioValue<int>>();

		/// <summary>
		/// If you use custom curves in the Reference AudioSource, set this to true.
		/// </summary>
		public bool UseCustomCurves = true;

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

		/// <summary>
		/// Used internally to pool AudioSources
		/// </summary>
		public ComponentPool<AudioSource> AudioSourcePool
		{
			get
			{
				if (audioSourcePool == null)
					Initialize();

				return audioSourcePool;
			}
		}

		/// <summary>
		/// Used internaly to manager AudioItems
		/// </summary>
		public AudioItemManager ItemManager { get { return itemManager; } }

		protected override void Awake()
		{
			base.Awake();

			Initialize();
		}

		void Reset()
		{
			Initialize();
		}

		void Update()
		{
			itemManager.Update();
		}

		void Initialize()
		{
			reference = CachedGameObject.FindOrAddChild("Reference").GetOrAddComponent<AudioSource>();
			reference.gameObject.SetActive(false);
			reference.playOnAwake = false;
			reference.spatialBlend = 1f;
			audioSourcePool = new ComponentPool<AudioSource>(reference, 0);

			if (Application.isPlaying)
			{
				audioSourcePool.GameObject.name = "Sources";
				audioSourcePool.Transform.parent = CachedTransform;
			}
		}

		/// <summary>
		/// Creates a non spatialized AudioItem that corresponds to the type of the <paramref name="settings"/>.
		/// </summary>
		/// <param name="settings">Settings that will define the behaviour of the AudioItem.</param>
		/// <returns></returns>
		public AudioItem CreateItem(AudioSettingsBase settings)
		{
			return itemManager.CreateItem(settings);
		}

		/// <summary>
		/// Creates an AudioItem spatialized at the provided <paramref name="position"/> that corresponds to the type of the <paramref name="settings"/>.
		/// </summary>
		/// <param name="settings"> Settings that will define the behaviour of the AudioItem. </param>
		/// <returns></returns>
		public AudioItem CreateItem(AudioSettingsBase settings, Vector3 position)
		{
			return itemManager.CreateItem(settings, position);
		}

		/// <summary>
		/// Creates an AudioItem dynamicaly spatialized around the provided Transform that corresponds to the type of the <paramref name="settings"/>.
		/// If the Transform ever becomes <code>null</code>, the AudioItem will simply stop moving.
		/// </summary>
		/// <param name="settings"> Settings that will define the behaviour of the AudioItem. </param>
		/// <returns></returns>
		public AudioItem CreateItem(AudioSettingsBase settings, Transform follow)
		{
			return itemManager.CreateItem(settings, follow);
		}

		/// <summary>
		/// Creates an AudioItem dynamicaly spatialized using the <paramref name="getPosition"/> callback to set its position that corresponds to the type of the <paramref name="settings"/>.
		/// </summary>
		/// <param name="settings"> Settings that will define the behaviour of the AudioItem. </param>
		/// <returns></returns>
		public AudioItem CreateItem(AudioSettingsBase settings, Func<Vector3> getPosition)
		{
			return itemManager.CreateItem(settings, getPosition);
		}

		/// <summary>
		/// Creates a non spatialized AudioDynamicItem that corresponds to the type of the <paramref name="settings"/>.
		/// The AudioDynamicItem will require new AudioSettingsBase when appropriate.
		/// The play behaviour of the AudioSettingsBase can be defined via the provided AudioDynamicData object.
		/// </summary>
		/// <param name="getNextSettings">Delegate that will be called when the AudioItem requires its next AudioSettingsBase.</param>
		/// <returns></returns>
		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings)
		{
			return ItemManager.CreateDynamicItem(getNextSettings);
		}

		/// <summary>
		/// Creates an AudioItem spatialized at the provided <paramref name="position"/> that corresponds to the type of the <paramref name="settings"/>.
		/// The AudioDynamicItem will require new AudioSettingsBase when appropriate.
		/// The play behaviour of the AudioSettingsBase can be defined via the provided AudioDynamicData object.
		/// </summary>
		/// <param name="getNextSettings">Delegate that will be called when the AudioItem requires its next AudioSettingsBase.</param>
		/// <returns></returns>
		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Vector3 position)
		{
			return ItemManager.CreateDynamicItem(getNextSettings, position);
		}

		/// <summary>
		/// Creates an AudioItem dynamicaly spatialized around the provided Transform that corresponds to the type of the <paramref name="settings"/>.
		/// If the Transform ever becomes <code>null</code>, the AudioItem will simply stop moving.
		/// The AudioDynamicItem will require new AudioSettingsBase when appropriate.
		/// The play behaviour of the AudioSettingsBase can be defined via the provided AudioDynamicData object.
		/// </summary>
		/// <param name="getNextSettings">Delegate that will be called when the AudioItem requires its next AudioSettingsBase.</param>
		/// <returns></returns>
		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Transform follow)
		{
			return ItemManager.CreateDynamicItem(getNextSettings, follow);
		}

		/// <summary>
		/// Creates an AudioItem dynamicaly spatialized using the <paramref name="getPosition"/> callback to set its position that corresponds to the type of the <paramref name="settings"/>.
		/// The AudioDynamicItem will require new AudioSettingsBase when appropriate.
		/// The play behaviour of the AudioSettingsBase can be defined via the provided AudioDynamicData object.
		/// </summary>
		/// <param name="getNextSettings">Delegate that will be called when the AudioItem requires its next AudioSettingsBase.</param>
		/// <returns></returns>
		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Func<Vector3> getPosition)
		{
			return ItemManager.CreateDynamicItem(getNextSettings, getPosition);
		}

		/// <summary>
		/// Stops all active AudioItems
		/// </summary>
		public void StopAll()
		{
			ItemManager.StopAll();
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
				value = AudioValue<int>.Pool.Create();
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