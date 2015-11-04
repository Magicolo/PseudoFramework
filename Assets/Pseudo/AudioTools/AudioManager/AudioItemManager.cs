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
			AudioSpatializer spatializer = AudioSpatializer.Pool.CreateCopy(AudioSpatializer.Default);

			return CreateItem(settings, spatializer, null);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Vector3 position)
		{
			AudioSpatializer spatializer = AudioSpatializer.Pool.CreateCopy(AudioSpatializer.Default);
			spatializer.Initialize(position);

			return CreateItem(settings, spatializer, null);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Transform follow)
		{
			AudioSpatializer spatializer = AudioSpatializer.Pool.CreateCopy(AudioSpatializer.Default);
			spatializer.Initialize(follow);

			return CreateItem(settings, spatializer, null);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Func<Vector3> getPosition)
		{
			AudioSpatializer spatializer = AudioSpatializer.Pool.CreateCopy(AudioSpatializer.Default);
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
					AudioSourceItem sourceItem = AudioItem.Pool.CreateCopy(AudioSourceItem.Default);
					AudioSource source = AudioManager.Instance.AudioSourcePool.Create();
					source.Copy(AudioManager.Instance.Reference, AudioManager.Instance.UseCustomCurves);
					sourceItem.Initialize((AudioSourceSettings)settings, source, spatializer, parent);
					return sourceItem;
				case AudioItem.AudioTypes.MixContainer:
					AudioMixContainerItem mixContainerItem = AudioItem.Pool.CreateCopy(AudioMixContainerItem.Default);
					mixContainerItem.Initialize((AudioMixContainerSettings)settings, spatializer, parent);
					return mixContainerItem;
				case AudioItem.AudioTypes.RandomContainer:
					AudioRandomContainerItem randomContainerItem = AudioItem.Pool.CreateCopy(AudioRandomContainerItem.Default);
					randomContainerItem.Initialize((AudioRandomContainerSettings)settings, spatializer, parent);
					return randomContainerItem;
				case AudioItem.AudioTypes.EnumeratorContainer:
					AudioEnumeratorContainerItem enumeratorContainerItem = AudioItem.Pool.CreateCopy(AudioEnumeratorContainerItem.Default);
					enumeratorContainerItem.Initialize((AudioEnumeratorContainerSettings)settings, spatializer, parent);
					return enumeratorContainerItem;
				case AudioItem.AudioTypes.SwitchContainer:
					AudioSwitchContainerItem switchContainerItem = AudioItem.Pool.CreateCopy(AudioSwitchContainerItem.Default);
					switchContainerItem.Initialize((AudioSwitchContainerSettings)settings, spatializer, parent);
					return switchContainerItem;
				case AudioItem.AudioTypes.SequenceContainer:
					AudioSequenceContainerItem sequenceContainerItem = AudioItem.Pool.CreateCopy(AudioSequenceContainerItem.Default);
					sequenceContainerItem.Initialize((AudioSequenceContainerSettings)settings, spatializer, parent);
					return sequenceContainerItem;
			}
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings)
		{
			AudioSpatializer spatializer = AudioSpatializer.Pool.CreateCopy(AudioSpatializer.Default);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Vector3 position)
		{
			AudioSpatializer spatializer = AudioSpatializer.Pool.CreateCopy(AudioSpatializer.Default);
			spatializer.Initialize(position);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Transform follow)
		{
			AudioSpatializer spatializer = AudioSpatializer.Pool.CreateCopy(AudioSpatializer.Default);
			spatializer.Initialize(follow);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Func<Vector3> getPosition)
		{
			AudioSpatializer spatializer = AudioSpatializer.Pool.CreateCopy(AudioSpatializer.Default);
			spatializer.Initialize(getPosition);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioDynamicItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, AudioSpatializer spatializer, AudioItem parent)
		{
			AudioDynamicItem item = AudioDynamicItem.Pool.CreateCopy(AudioDynamicItem.Default);
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