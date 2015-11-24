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
			toUpdate.Add(item);
		}

		public void Deactivate(AudioItem item)
		{
			List<AudioItem> items;

			if (idActiveItems.TryGetValue(item.Id, out items))
				items.Remove(item);

			toUpdate.Remove(item);
		}

		public void TrimInstances(AudioItem item, int maxInstances)
		{
			List<AudioItem> items;

			if (!idActiveItems.TryGetValue(item.Id, out items))
			{
				items = new List<AudioItem>();
				idActiveItems[item.Id] = items;
			}

			if (maxInstances > 0)
				while (items.Count >= maxInstances)
					items.Pop().StopImmediate();

			items.Add(item);
		}

		public AudioItem CreateItem(AudioSettingsBase settings)
		{
			var spatializer = AudioSpatializer.Pool.Create();

			return CreateItem(settings, spatializer, null);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Vector3 position)
		{
			var spatializer = AudioSpatializer.Pool.Create();
			spatializer.Initialize(position);

			return CreateItem(settings, spatializer, null);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Transform follow)
		{
			var spatializer = AudioSpatializer.Pool.Create();
			spatializer.Initialize(follow);

			return CreateItem(settings, spatializer, null);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Func<Vector3> getPosition)
		{
			var spatializer = AudioSpatializer.Pool.Create();
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
					var sourceItem = PoolManager.Create(AudioSourceItem.Default);
					var source = AudioManager.Instance.AudioSourcePool.Create();
					source.Copy(AudioManager.Instance.Reference, AudioManager.Instance.UseCustomCurves);
					sourceItem.Initialize((AudioSourceSettings)settings, source, spatializer, parent);
					return sourceItem;
				case AudioItem.AudioTypes.MixContainer:
					var mixContainerItem = PoolManager.Create(AudioMixContainerItem.Default);
					mixContainerItem.Initialize((AudioMixContainerSettings)settings, spatializer, parent);
					return mixContainerItem;
				case AudioItem.AudioTypes.RandomContainer:
					var randomContainerItem = PoolManager.Create(AudioRandomContainerItem.Default);
					randomContainerItem.Initialize((AudioRandomContainerSettings)settings, spatializer, parent);
					return randomContainerItem;
				case AudioItem.AudioTypes.EnumeratorContainer:
					var enumeratorContainerItem = PoolManager.Create(AudioEnumeratorContainerItem.Default);
					enumeratorContainerItem.Initialize((AudioEnumeratorContainerSettings)settings, spatializer, parent);
					return enumeratorContainerItem;
				case AudioItem.AudioTypes.SwitchContainer:
					var switchContainerItem = PoolManager.Create(AudioSwitchContainerItem.Default);
					switchContainerItem.Initialize((AudioSwitchContainerSettings)settings, spatializer, parent);
					return switchContainerItem;
				case AudioItem.AudioTypes.SequenceContainer:
					var sequenceContainerItem = PoolManager.Create(AudioSequenceContainerItem.Default);
					sequenceContainerItem.Initialize((AudioSequenceContainerSettings)settings, spatializer, parent);
					return sequenceContainerItem;
			}
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings)
		{
			var spatializer = AudioSpatializer.Pool.Create();

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Vector3 position)
		{
			var spatializer = AudioSpatializer.Pool.Create();
			spatializer.Initialize(position);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Transform follow)
		{
			var spatializer = AudioSpatializer.Pool.Create();
			spatializer.Initialize(follow);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Func<Vector3> getPosition)
		{
			var spatializer = AudioSpatializer.Pool.Create();
			spatializer.Initialize(getPosition);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioDynamicItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, AudioSpatializer spatializer, AudioItem parent)
		{
			var item = PoolManager.Create(AudioDynamicItem.Default);
			item.Initialize(getNextSettings, spatializer, parent);

			return item;
		}

		public void StopAll()
		{
			for (int i = toUpdate.Count; i-- > 0;)
				toUpdate[i].StopImmediate();
		}
	}
}