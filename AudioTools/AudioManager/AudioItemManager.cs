using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

namespace Pseudo.Internal.Audio
{
	public class AudioItemManager
	{
		Dictionary<int, List<IAudioItem>> idActiveItems = new Dictionary<int, List<IAudioItem>>();
		List<AudioItemBase> toUpdate = new List<AudioItemBase>();
		IAudioManager audioManager;

		public IAudioManager AudioManager
		{
			get { return audioManager; }
		}

		public AudioItemManager(IAudioManager audioManager)
		{
			this.audioManager = audioManager;
		}

		public void Update()
		{
			for (int i = toUpdate.Count; i-- > 0;)
				toUpdate[i].Update();
		}

		public void Activate(AudioItemBase item)
		{
			GetItems(item.Identifier).Add(item);
			toUpdate.Add(item);
		}

		public void Deactivate(AudioItemBase item)
		{
			GetItems(item.Identifier).Remove(item);
			toUpdate.Remove(item);
		}

		public void TrimInstances(AudioItemBase item, int maxInstances)
		{
			var items = GetItems(item.Identifier);

			if (maxInstances > 0)
			{
				while (items.Count >= maxInstances)
					items.Pop().StopImmediate();
			}
		}

		public IAudioItem CreateItem(AudioSettingsBase settings)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();

			return CreateItem(settings, spatializer, null);
		}

		public IAudioItem CreateItem(AudioSettingsBase settings, Vector3 position)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();
			spatializer.Initialize(position);

			return CreateItem(settings, spatializer, null);
		}

		public IAudioItem CreateItem(AudioSettingsBase settings, Transform follow)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();
			spatializer.Initialize(follow);

			return CreateItem(settings, spatializer, null);
		}

		public IAudioItem CreateItem(AudioSettingsBase settings, Func<Vector3> getPosition)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();
			spatializer.Initialize(getPosition);

			return CreateItem(settings, spatializer, null);
		}

		public AudioItemBase CreateItem(AudioSettingsBase settings, AudioSpatializer spatializer, IAudioItem parent)
		{
			if (settings == null)
				return null;

			switch (settings.Type)
			{
				default:
					var sourceItem = TypePoolManager.Create<AudioSourceItem>();
					var source = PrefabPoolManager.Create(audioManager.Reference);
					source.Copy(audioManager.Reference, audioManager.UseCustomCurves);
					sourceItem.Initialize((AudioSourceSettings)settings, this, source, spatializer, parent);
					return sourceItem;
				case AudioTypes.MixContainer:
					var mixContainerItem = TypePoolManager.Create<AudioMixContainerItem>();
					mixContainerItem.Initialize((AudioMixContainerSettings)settings, this, spatializer, parent);
					return mixContainerItem;
				case AudioTypes.RandomContainer:
					var randomContainerItem = TypePoolManager.Create<AudioRandomContainerItem>();
					randomContainerItem.Initialize((AudioRandomContainerSettings)settings, this, spatializer, parent);
					return randomContainerItem;
				case AudioTypes.EnumeratorContainer:
					var enumeratorContainerItem = TypePoolManager.Create<AudioEnumeratorContainerItem>();
					enumeratorContainerItem.Initialize((AudioEnumeratorContainerSettings)settings, this, spatializer, parent);
					return enumeratorContainerItem;
				case AudioTypes.SwitchContainer:
					var switchContainerItem = TypePoolManager.Create<AudioSwitchContainerItem>();
					switchContainerItem.Initialize((AudioSwitchContainerSettings)settings, this, spatializer, parent);
					return switchContainerItem;
				case AudioTypes.SequenceContainer:
					var sequenceContainerItem = TypePoolManager.Create<AudioSequenceContainerItem>();
					sequenceContainerItem.Initialize((AudioSequenceContainerSettings)settings, this, spatializer, parent);
					return sequenceContainerItem;
			}
		}

		public IAudioItem CreateDynamicItem(DynamicGetter getNextSettings)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public IAudioItem CreateDynamicItem(DynamicGetter getNextSettings, Vector3 position)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();
			spatializer.Initialize(position);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public IAudioItem CreateDynamicItem(DynamicGetter getNextSettings, Transform follow)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();
			spatializer.Initialize(follow);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public IAudioItem CreateDynamicItem(DynamicGetter getNextSettings, Func<Vector3> getPosition)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();
			spatializer.Initialize(getPosition);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioDynamicItem CreateDynamicItem(DynamicGetter getNextSettings, AudioSpatializer spatializer, IAudioItem parent)
		{
			var item = TypePoolManager.Create<AudioDynamicItem>();
			item.Initialize(getNextSettings, this, spatializer, parent);

			return item;
		}

		public void StopItemsWithId(int id)
		{
			var items = GetItems(id);

			for (int i = items.Count; i-- > 0;)
				items[i].Stop();
		}

		public void StopItemsWithIdImmediate(int id)
		{
			var items = GetItems(id);

			for (int i = items.Count; i-- > 0;)
				items[i].StopImmediate();
		}

		public void StopAllItems()
		{
			for (int i = toUpdate.Count; i-- > 0;)
				toUpdate[i].Stop();
		}

		public void StopAllItemsImmediate()
		{
			for (int i = toUpdate.Count; i-- > 0;)
				toUpdate[i].StopImmediate();
		}

		List<IAudioItem> GetItems(int id)
		{
			List<IAudioItem> items;

			if (!idActiveItems.TryGetValue(id, out items))
			{
				items = new List<IAudioItem>();
				idActiveItems[id] = items;
			}

			return items;
		}
	}
}