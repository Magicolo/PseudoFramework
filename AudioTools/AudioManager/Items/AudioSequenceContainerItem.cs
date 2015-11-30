using UnityEngine;
using System.Collections;
using Pseudo;
using System;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	public class AudioSequenceContainerItem : AudioContainerItem
	{
		AudioSequenceContainerSettings originalSettings;
		AudioSequenceContainerSettings settings;
		double deltaTime;
		double lastTime;
		double delay;
		int sourcesIndex;

		public override AudioTypes Type { get { return AudioTypes.SequenceContainer; } }
		public override AudioSettingsBase Settings { get { return settings; } }

		public void Initialize(AudioSequenceContainerSettings settings, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(settings.GetHashCode(), settings.Name, spatializer, parent);

			originalSettings = settings;
			this.settings = PrefabPoolManager.Create(settings);

			InitializeModifiers(originalSettings);
			InitializeSources();

			for (int i = 0; i < originalSettings.Options.Count; i++)
				ApplyOption(originalSettings.Options[i], false);
		}

		protected override void InitializeSources()
		{
			sourcesIndex = 0;
			CopyUtility.CopyTo(originalSettings.Delays, ref settings.Delays);

			if (originalSettings.Sources.Count > 0)
				AddSource(originalSettings.Sources[sourcesIndex++]);
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
			if (!IsPlaying || state == AudioStates.Stopping)
				return;

			if (sources.Count < 2 && sourcesIndex < originalSettings.Sources.Count)
				AddSource(originalSettings.Sources[sourcesIndex++]);
		}

		protected void UpdateDeltaTime()
		{
			double dspTime = AudioSettings.dspTime;
			deltaTime = Math.Max(dspTime - lastTime, 0d);
			lastTime = dspTime;
		}

		protected void UpdateDelays()
		{
			if (state == AudioStates.Stopped)
				return;

			UpdateDeltaTime();

			delay = 0d;

			for (int i = 0; i < sourcesIndex - sources.Count; i++)
			{
				double currentDelay = settings.Delays[i];

				if (state != AudioStates.Paused)
					currentDelay = Math.Max(currentDelay - deltaTime, 0d);

				delay += currentDelay;
				settings.Delays[i] = currentDelay;
			}
		}

		protected void UpdateScheduledTime()
		{
			if (state == AudioStates.Stopped)
				return;

			UpdateDelays();

			// Schedule sources
			int delayIndex = sourcesIndex - sources.Count;
			double remainingTime = 0d;

			for (int i = 0; i < sources.Count; i++)
			{
				AudioItem item = sources[i];
				double time;

				if (i == 0)
					time = Math.Max(AudioSettings.dspTime, scheduledTime) + delay;
				else
					time = AudioSettings.dspTime + remainingTime;

				if (state == AudioStates.Playing && item.State == AudioStates.Waiting)
					item.PlayScheduled(time);
				else
					item.SetScheduledTime(time);

				if (delayIndex < settings.Delays.Count)
					remainingTime = item.RemainingTime() + settings.Delays[delayIndex++];
				else
					remainingTime = item.RemainingTime();
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

		public override double RemainingTime()
		{
			if (state == AudioStates.Stopped || sources.Count == 0)
				return 0d;

			return sources.Last().RemainingTime();
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			PrefabPoolManager.Recycle(ref settings);
		}

		public override void Copy(object reference)
		{
			base.Copy(reference);

			var castedReference = (AudioSequenceContainerItem)reference;
			originalSettings = castedReference.originalSettings;
			settings = castedReference.settings;
			deltaTime = castedReference.deltaTime;
			lastTime = castedReference.lastTime;
			delay = castedReference.delay;
			sourcesIndex = castedReference.sourcesIndex;
		}
	}
}