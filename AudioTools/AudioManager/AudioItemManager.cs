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
		Dictionary<int, List<AudioItem>> idActiveItems = new Dictionary<int, List<AudioItem>>();
		List<AudioItem> toUpdate = new List<AudioItem>();

		public void Update()
		{
			for (int i = toUpdate.Count; i-- > 0;)
				toUpdate[i].Update();
		}

		public void Activate(AudioItem item)
		{
			GetItems(item.Id).Add(item);
			toUpdate.Add(item);
		}

		public void Deactivate(AudioItem item)
		{
			GetItems(item.Id).Remove(item);
			toUpdate.Remove(item);
		}

		public void TrimInstances(AudioItem item, int maxInstances)
		{
			var items = GetItems(item.Id);

			if (maxInstances > 0)
			{
				while (items.Count >= maxInstances)
					items.Pop().StopImmediate();
			}
		}

		public AudioItem CreateItem(AudioSettingsBase settings)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();

			return CreateItem(settings, spatializer, null);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Vector3 position)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();
			spatializer.Initialize(position);

			return CreateItem(settings, spatializer, null);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Transform follow)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();
			spatializer.Initialize(follow);

			return CreateItem(settings, spatializer, null);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Func<Vector3> getPosition)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();
			spatializer.Initialize(getPosition);

			return CreateItem(settings, spatializer, null);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, AudioSpatializer spatializer, AudioItem parent)
		{
			if (settings == null)
				return null;

			switch (settings.Type)
			{
				default:
					var sourceItem = TypePoolManager.Create<AudioSourceItem>();
					var source = PrefabPoolManager.Create(AudioManager.Instance.Reference);
					source.Copy(AudioManager.Instance.Reference, AudioManager.Instance.UseCustomCurves);
					sourceItem.Initialize((AudioSourceSettings)settings, source, spatializer, parent);
					return sourceItem;
				case AudioItem.AudioTypes.MixContainer:
					var mixContainerItem = TypePoolManager.Create<AudioMixContainerItem>();
					mixContainerItem.Initialize((AudioMixContainerSettings)settings, spatializer, parent);
					return mixContainerItem;
				case AudioItem.AudioTypes.RandomContainer:
					var randomContainerItem = TypePoolManager.Create<AudioRandomContainerItem>();
					randomContainerItem.Initialize((AudioRandomContainerSettings)settings, spatializer, parent);
					return randomContainerItem;
				case AudioItem.AudioTypes.EnumeratorContainer:
					var enumeratorContainerItem = TypePoolManager.Create<AudioEnumeratorContainerItem>();
					enumeratorContainerItem.Initialize((AudioEnumeratorContainerSettings)settings, spatializer, parent);
					return enumeratorContainerItem;
				case AudioItem.AudioTypes.SwitchContainer:
					var switchContainerItem = TypePoolManager.Create<AudioSwitchContainerItem>();
					switchContainerItem.Initialize((AudioSwitchContainerSettings)settings, spatializer, parent);
					return switchContainerItem;
				case AudioItem.AudioTypes.SequenceContainer:
					var sequenceContainerItem = TypePoolManager.Create<AudioSequenceContainerItem>();
					sequenceContainerItem.Initialize((AudioSequenceContainerSettings)settings, spatializer, parent);
					return sequenceContainerItem;
			}
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Vector3 position)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();
			spatializer.Initialize(position);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Transform follow)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();
			spatializer.Initialize(follow);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Func<Vector3> getPosition)
		{
			var spatializer = TypePoolManager.Create<AudioSpatializer>();
			spatializer.Initialize(getPosition);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioDynamicItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, AudioSpatializer spatializer, AudioItem parent)
		{
			var item = TypePoolManager.Create<AudioDynamicItem>();
			item.Initialize(getNextSettings, spatializer, parent);

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

		List<AudioItem> GetItems(int id)
		{
			List<AudioItem> items;

			if (!idActiveItems.TryGetValue(id, out items))
			{
				items = new List<AudioItem>();
				idActiveItems[id] = items;
			}

			return items;
		}
	}
}