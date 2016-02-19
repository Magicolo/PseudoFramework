using UnityEngine;
using System.Collections;
using Pseudo;
using System;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	public class AudioMixContainerItem : AudioContainerItem, ICopyable<AudioMixContainerItem>
	{
		AudioMixContainerSettings originalSettings;
		AudioMixContainerSettings settings;
		double deltaTime;
		double lastTime;

		readonly List<double> delays = new List<double>();

		public override AudioTypes Type { get { return AudioTypes.MixContainer; } }
		public override AudioSettingsBase Settings { get { return settings; } }

		public void Initialize(AudioMixContainerSettings settings, AudioItemManager itemManager, AudioSpatializer spatializer, IAudioItem parent)
		{
			base.Initialize(settings.Identifier, itemManager, spatializer, parent);

			originalSettings = settings;
			this.settings = PrefabPoolManager.Create(settings);

			InitializeModifiers(originalSettings);
			InitializeSources();

			for (int i = 0; i < originalSettings.Options.Count; i++)
				ApplyOption(originalSettings.Options[i], false);
		}

		protected override void InitializeSources()
		{
			for (int i = 0; i < originalSettings.Sources.Count; i++)
			{
				if (AddSource(originalSettings.Sources[i]) != null)
					delays.Add(originalSettings.Delays[i]);
			}
		}

		public override void Update()
		{
			base.Update();

			UpdateScheduledTime();
		}

		protected void UpdateScheduledTime()
		{
			if (state == AudioStates.Stopped)
				return;

			// Update delta time
			double dspTime = Math.Max(AudioSettings.dspTime, scheduledTime);

			deltaTime = dspTime - lastTime;
			lastTime = dspTime;

			// Decrease delay counters
			for (int i = 0; i < delays.Count; i++)
			{
				if (state != AudioStates.Paused)
					delays[i] = Math.Max(delays[i] - deltaTime, 0d);
			}

			// Schedule sources
			for (int i = 0; i < sources.Count; i++)
			{
				IAudioItem item = sources[i];
				double time = Math.Max(AudioSettings.dspTime, scheduledTime) + delays[i];

				if (state == AudioStates.Playing && item.State == AudioStates.Waiting)
					item.PlayScheduled(time);
				else
					item.SetScheduledTime(time);
			}
		}

		public override void Play()
		{
			if (state != AudioStates.Waiting)
				return;

			lastTime = Math.Max(AudioSettings.dspTime, scheduledTime);
			UpdateScheduledTime();

			base.Play();
		}

		public override void SetScheduledTime(double time)
		{
			if (state == AudioStates.Stopped || scheduleStarted)
				return;

			scheduledTime = time;
			lastTime = time;

			UpdateScheduledTime();
		}

		protected override void RemoveSource(int index)
		{
			base.RemoveSource(index);

			delays.RemoveAt(index);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			delays.Clear();
			PrefabPoolManager.Recycle(ref settings);
		}

		public void Copy(AudioMixContainerItem reference)
		{
			base.Copy(reference);

			originalSettings = reference.originalSettings;
			settings = reference.settings;
			deltaTime = reference.deltaTime;
			lastTime = reference.lastTime;
		}
	}
}