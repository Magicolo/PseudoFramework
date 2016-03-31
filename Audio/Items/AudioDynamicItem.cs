using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using UnityEngine.Audio;
using Pseudo.Internal;
using Pseudo.Audio.Internal;
using Pseudo.Pooling;

namespace Pseudo.Audio
{
	public class AudioDynamicItem : AudioContainerItem
	{
		DynamicGetter getNextSettings;
		AudioDynamicSettings settings;
		int currentStep;
		bool requestNextSettings = true;
		bool breakSequence;
		double deltaTime;
		double lastTime;

		readonly List<AudioDynamicData> dynamicData = new List<AudioDynamicData>();

		public override AudioTypes Type { get { return AudioTypes.Dynamic; } }
		public override AudioSettingsBase Settings { get { return settings; } }
		public int CurrentStep { get { return currentStep; } }

		public void Initialize(DynamicGetter getNextSettings, AudioItemManager itemManager, AudioSpatializer spatializer, IAudioItem parent)
		{
			settings = TypePoolManager.Create<AudioDynamicSettings>();

			base.Initialize(settings.Identifier, itemManager, spatializer, parent);

			this.getNextSettings = getNextSettings ?? delegate { return null; };

			InitializeModifiers(settings);
			InitializeSources();
		}

		protected override void InitializeSources()
		{
			UpdateSequence();
		}

		public override void Update()
		{
			if (state == AudioStates.Stopped)
				return;

			UpdateSequence();
			base.Update();
			UpdateScheduledTime();
		}

		protected void UpdateSequence()
		{
			if (breakSequence || (sources.Count > 0 && !requestNextSettings))
				return;

			var data = TypePoolManager.Create<AudioDynamicData>();
			var settings = getNextSettings(this, data);

			currentStep++;

			if (settings == null || state == AudioStates.Stopped)
				breakSequence = true;
			else
				AddSource(settings, data);
		}

		protected void UpdateDeltaTime()
		{
			double dspTime = Math.Max(AudioSettings.dspTime, scheduledTime);
			deltaTime = dspTime - lastTime;
			lastTime = dspTime;
		}

		protected void UpdateScheduledTime()
		{
			if (state == AudioStates.Stopped)
				return;

			UpdateDeltaTime();
			double remainingTime = 0d;

			requestNextSettings = state != AudioStates.Paused;

			for (int i = 0; i < sources.Count; i++)
			{
				IAudioItem source = sources[i];
				AudioDynamicData data = dynamicData[i];

				// Decrease delay
				if (state != AudioStates.Paused)
				{
					switch (data.PlayMode)
					{
						case AudioDynamicData.PlayModes.Now:
							data.Delay = Math.Max(data.Delay - deltaTime, 0d);
							break;
						case AudioDynamicData.PlayModes.After:
							if (i == 0)
								data.Delay = Math.Max(data.Delay - deltaTime, 0d);
							else
								requestNextSettings = false;
							break;
					}

					if (data.Delay > 0d)
						requestNextSettings = false;
				}

				// Schedule source
				double time = Math.Max(AudioSettings.dspTime, scheduledTime) + remainingTime + data.Delay;

				if (state == AudioStates.Playing && source.State == AudioStates.Waiting)
					source.PlayScheduled(time);
				else
					source.SetScheduledTime(time);

				// Set remaining time
				switch (data.PlayMode)
				{
					case AudioDynamicData.PlayModes.Now:
						remainingTime = data.Delay;
						break;
					case AudioDynamicData.PlayModes.After:
						remainingTime = source.RemainingTime();
						break;
				}
			}
		}

		public override void Play()
		{
			if (state != AudioStates.Waiting)
				return;

			lastTime = Math.Max(AudioSettings.dspTime, scheduledTime);

			base.Play();
		}

		public override void Stop()
		{
			if (state == AudioStates.Stopped || state == AudioStates.Stopping)
				return;

			breakSequence = true;

			base.Stop();
		}

		public override void StopImmediate()
		{
			if (state == AudioStates.Stopped)
				return;

			breakSequence = true;

			base.StopImmediate();
		}

		protected void AddSource(AudioSettingsBase settings, AudioDynamicData data)
		{
			IAudioItem item = base.AddSource(settings, null);

			if (item == null)
				return;

			if (data.OnInitialize != null)
			{
				data.OnInitialize(item);
				data.OnInitialize = null;
			}

			dynamicData.Add(data);
		}

		protected override void RemoveSource(int index)
		{
			base.RemoveSource(index);

			TypePoolManager.Recycle(dynamicData.Pop(index));
			UpdateSequence();
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			PrefabPoolManager.Recycle(settings);
			TypePoolManager.RecycleElements(dynamicData);
		}
	}
}