using UnityEngine;
using System.Collections;
using Pseudo;
using System;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	public abstract class AudioContainerItem : AudioItemBase
	{
		protected readonly List<AudioItemBase> sources = new List<AudioItemBase>();
		protected readonly Action<AudioModifier> setVolumeScale;
		protected readonly Action<AudioModifier> setPitchScale;

		protected AudioContainerItem()
		{
			setVolumeScale = modifer =>
			{
				for (int i = 0; i < sources.Count; i++)
					sources[i].SetVolumeScale(modifer.Value);
			};

			setPitchScale = modifer =>
			{
				for (int i = 0; i < sources.Count; i++)
					sources[i].SetPitchScale(modifer.Value);
			};
		}

		protected virtual void InitializeModifiers(AudioSettingsBase settings)
		{
			volumeModifier.OnValueChanged += setVolumeScale;
			volumeModifier.InitialValue = settings.VolumeScale;
			volumeModifier.RandomModifier = 1f + UnityEngine.Random.Range(-settings.RandomVolume, settings.RandomVolume);

			pitchModifier.OnValueChanged += setPitchScale;
			pitchModifier.InitialValue = settings.PitchScale;
			pitchModifier.RandomModifier = 1f + UnityEngine.Random.Range(-settings.RandomPitch, settings.RandomPitch);
		}

		protected abstract void InitializeSources();

		protected virtual void UpdateSources()
		{
			for (int i = sources.Count; i-- > 0;)
			{
				var source = sources[i];
				source.Update();

				if (source.State == AudioStates.Stopped || (state == AudioStates.Stopping && source.State == AudioStates.Waiting))
					RemoveSource(i);
			}
		}

		public override void Update()
		{
			if (state == AudioStates.Stopped)
				return;

			UpdateSources();

			base.Update();

			if (sources.Count == 0)
			{
				if (!hasBreak && Settings.Loop)
					Reset();
				else
					StopImmediate();
			}
		}

		public override void Play()
		{
			if (state != AudioStates.Waiting)
				return;

			if (Settings.FadeIn > 0f)
				FadeIn();

			if (scheduledTime > 0d)
			{
				for (int i = 0; i < sources.Count; i++)
					sources[i].PlayScheduled(scheduledTime);
			}
			else
			{
				for (int i = 0; i < sources.Count; i++)
					sources[i].Play();
			}

			SetState(AudioStates.Playing);
			RaisePlayEvent();
		}

		public override void PlayScheduled(double time)
		{
			if (state != AudioStates.Waiting)
				return;

			SetScheduledTime(time);
			Play();
		}

		public override void Pause()
		{
			if (state != AudioStates.Playing && state != AudioStates.Stopping)
				return;

			for (int i = 0; i < sources.Count; i++)
				sources[i].Pause();

			pausedState = state;
			SetState(AudioStates.Paused);
			RaisePauseEvent();
		}

		public override void Resume()
		{
			if (state != AudioStates.Paused)
				return;

			for (int i = 0; i < sources.Count; i++)
				sources[i].Resume();

			SetState(pausedState);
			RaiseResumeEvent();
		}

		public override void Stop()
		{
			if (state == AudioStates.Stopping || state == AudioStates.Stopped)
				return;

			fadeTweener.Stop();

			if (Settings.FadeOut > 0f && state != AudioStates.Paused)
				FadeOut();
			else
			{
				for (int i = 0; i < sources.Count; i++)
					sources[i].Stop();
			}

			SetState(AudioStates.Stopping);
			RaiseStoppingEvent();
		}

		public override void StopImmediate()
		{
			if (state == AudioStates.Stopped)
				return;

			for (int i = 0; i < sources.Count; i++)
				sources[i].StopImmediate();

			SetState(AudioStates.Stopped);
			RaiseStopEvent();

			if (parent == null)
				itemManager.Deactivate(this);

			TypePoolManager.Recycle(this);
		}

		protected override void ApplyOptionNow(AudioOption option, bool recycle)
		{
			if (state != AudioStates.Stopped)
			{
				switch (option.Type)
				{
					case AudioOption.Types.VolumeScale:
						float[] volumeData = option.GetValue<float[]>();
						SetVolumeScale(volumeData[0], volumeData[1], (TweenUtility.Ease)volumeData[2], true);
						break;
					case AudioOption.Types.PitchScale:
						float[] pitchData = option.GetValue<float[]>();
						SetPitchScale(pitchData[0], pitchData[1], (TweenUtility.Ease)pitchData[2], true);
						break;
					case AudioOption.Types.RandomVolume:
						float randomVolume = option.GetValue<float>();
						Settings.RandomVolume = randomVolume;
						volumeModifier.RandomModifier = 1f + UnityEngine.Random.Range(-randomVolume, randomVolume);
						break;
					case AudioOption.Types.RandomPitch:
						float randomPitch = option.GetValue<float>();
						Settings.RandomPitch = randomPitch;
						pitchModifier.RandomModifier = 1f + UnityEngine.Random.Range(-randomPitch, randomPitch);
						break;
					case AudioOption.Types.FadeIn:
						float[] fadeInData = option.GetValue<float[]>();
						Settings.FadeIn = fadeInData[0];
						Settings.FadeInEase = (TweenUtility.Ease)fadeInData[1];
						break;
					case AudioOption.Types.FadeOut:
						float[] fadeOutData = option.GetValue<float[]>();
						Settings.FadeIn = fadeOutData[0];
						Settings.FadeInEase = (TweenUtility.Ease)fadeOutData[1];
						break;
					case AudioOption.Types.Loop:
						Settings.Loop = option.GetValue<bool>();
						break;
					default:
						for (int i = 0; i < sources.Count; i++)
							sources[i].ApplyOption(option, false);
						break;
				}
			}

			if (recycle)
				TypePoolManager.Recycle(ref option);
		}

		public override void SetScheduledTime(double time)
		{
			if (state == AudioStates.Stopped || scheduleStarted)
				return;

			scheduledTime = time;

			for (int i = 0; i < sources.Count; i++)
				sources[i].SetScheduledTime(scheduledTime);
		}

		public override double RemainingTime()
		{
			if (state == AudioStates.Stopped)
				return 0d;

			double remainingTime = 0d;

			for (int i = 0; i < sources.Count; i++)
				remainingTime = Math.Max(remainingTime, sources[i].RemainingTime());

			return remainingTime;
		}

		public override void Break()
		{
			for (int i = 0; i < sources.Count; i++)
			{
				IAudioItem source = sources[i];

				if (source.State != AudioStates.Waiting && source.GetScheduledTime() <= AudioSettings.dspTime)
					source.Break();
			}

			hasBreak = true;
		}

		public override void SetRTPCValue(string name, float value)
		{
			if (State == AudioStates.Stopped)
				return;

			var rtpc = Settings.GetRTPC(name);

			if (rtpc != null)
				rtpc.SetValue(value);

			for (int i = 0; i < sources.Count; i++)
				sources[i].SetRTPCValue(name, value);
		}

		protected virtual IAudioItem AddSource(AudioContainerSourceData data)
		{
			IAudioItem item = null;

			if (data != null)
				item = AddSource(data.Settings, data.Options);

			return item;
		}

		protected virtual IAudioItem AddSource(AudioSettingsBase settings, List<AudioOption> options)
		{
			AudioItemBase item = null;

			if (settings != null)
			{
				item = itemManager.CreateItem(settings, spatializer, this);

				if (options != null)
				{
					for (int i = 0; i < options.Count; i++)
						item.ApplyOption(options[i], false);
				}

				sources.Add(item);
				volumeModifier.SimulateChange();
				pitchModifier.SimulateChange();
			}

			return item;
		}

		protected virtual void RemoveSource(int index)
		{
			sources.RemoveAt(index);
		}

		protected virtual void Reset()
		{
			SetState(AudioStates.Waiting);
			InitializeSources();
			Play();
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			sources.Clear();
		}
	}
}