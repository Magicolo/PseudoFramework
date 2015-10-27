using UnityEngine;
using System.Collections;
using Pseudo;
using System;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	public abstract class AudioContainerItem : AudioItem, ICopyable<AudioContainerItem>
	{
		protected readonly List<AudioItem> _sources = new List<AudioItem>();

		protected readonly Action<AudioModifier> _setVolumeScale;
		protected readonly Action<AudioModifier> _setPitchScale;

		protected AudioContainerItem()
		{
			_setVolumeScale = modifer =>
			{
				for (int i = 0; i < _sources.Count; i++)
					_sources[i].SetVolumeScale(modifer.Value);
			};

			_setPitchScale = modifer =>
			{
				for (int i = 0; i < _sources.Count; i++)
					_sources[i].SetPitchScale(modifer.Value);
			};
		}

		protected virtual void InitializeModifiers(AudioSettingsBase settings)
		{
			_volumeModifier.OnValueChanged += _setVolumeScale;
			_volumeModifier.InitialValue = settings.VolumeScale;
			_volumeModifier.RandomModifier = 1f + UnityEngine.Random.Range(-settings.RandomVolume, settings.RandomVolume);

			_pitchModifier.OnValueChanged += _setPitchScale;
			_pitchModifier.InitialValue = settings.PitchScale;
			_pitchModifier.RandomModifier = 1f + UnityEngine.Random.Range(-settings.RandomPitch, settings.RandomPitch);
		}

		protected abstract void InitializeSources();

		protected virtual void UpdateSources()
		{
			for (int i = _sources.Count; i-- > 0;)
			{
				AudioItem source = _sources[i];

				source.Update();

				if (source.State == AudioStates.Stopped || (_state == AudioStates.Stopping && source.State == AudioStates.Waiting))
					RemoveSource(i);
			}
		}

		public override void Update()
		{
			if (_state == AudioStates.Stopped)
				return;

			UpdateSources();

			base.Update();

			if (_sources.Count == 0)
			{
				if (!_break && Settings.Loop)
					Reset();
				else
					StopImmediate();
			}
		}

		public override void Play()
		{
			if (_state != AudioStates.Waiting)
				return;

			if (Settings.FadeIn > 0f)
				FadeIn();

			if (_scheduledTime > 0d)
			{
				for (int i = 0; i < _sources.Count; i++)
					_sources[i].PlayScheduled(_scheduledTime);
			}
			else
			{
				for (int i = 0; i < _sources.Count; i++)
					_sources[i].Play();
			}

			SetState(AudioStates.Playing);
			RaisePlayEvent();
		}

		public override void PlayScheduled(double time)
		{
			if (_state != AudioStates.Waiting)
				return;

			SetScheduledTime(time);
			Play();
		}

		public override void Pause()
		{
			if (_state != AudioStates.Playing && _state != AudioStates.Stopping)
				return;

			for (int i = 0; i < _sources.Count; i++)
				_sources[i].Pause();

			_pausedState = _state;
			SetState(AudioStates.Paused);
			RaisePauseEvent();
		}

		public override void Resume()
		{
			if (_state != AudioStates.Paused)
				return;

			for (int i = 0; i < _sources.Count; i++)
				_sources[i].Resume();

			SetState(_pausedState);
			RaiseResumeEvent();
		}

		public override void Stop()
		{
			if (_state == AudioStates.Stopping || _state == AudioStates.Stopped)
				return;

			_fadeTweener.Stop();

			if (Settings.FadeOut > 0f && _state != AudioStates.Paused)
				FadeOut();
			else
			{
				for (int i = 0; i < _sources.Count; i++)
					_sources[i].Stop();
			}

			SetState(AudioStates.Stopping);
			RaiseStoppingEvent();
		}

		public override void StopImmediate()
		{
			if (_state == AudioStates.Stopped)
				return;

			for (int i = 0; i < _sources.Count; i++)
				_sources[i].StopImmediate();

			SetState(AudioStates.Stopped);
			RaiseStopEvent();

			if (_parent == null)
				PAudio.Instance.ItemManager.Deactivate(this);

			Recycle();
		}

		protected override void ApplyOptionNow(AudioOption option, bool recycle)
		{
			if (_state != AudioStates.Stopped)
			{
				switch (option.Type)
				{
					case AudioOption.Types.VolumeScale:
						float[] volumeData = option.GetValue<float[]>();
						SetVolumeScale(volumeData[0], volumeData[1], (Tweening.Ease)volumeData[2], true);
						break;
					case AudioOption.Types.PitchScale:
						float[] pitchData = option.GetValue<float[]>();
						SetPitchScale(pitchData[0], pitchData[1], (Tweening.Ease)pitchData[2], true);
						break;
					case AudioOption.Types.RandomVolume:
						float randomVolume = option.GetValue<float>();
						Settings.RandomVolume = randomVolume;
						_volumeModifier.RandomModifier = 1f + UnityEngine.Random.Range(-randomVolume, randomVolume);
						break;
					case AudioOption.Types.RandomPitch:
						float randomPitch = option.GetValue<float>();
						Settings.RandomPitch = randomPitch;
						_pitchModifier.RandomModifier = 1f + UnityEngine.Random.Range(-randomPitch, randomPitch);
						break;
					case AudioOption.Types.FadeIn:
						float[] fadeInData = option.GetValue<float[]>();
						Settings.FadeIn = fadeInData[0];
						Settings.FadeInEase = (Tweening.Ease)fadeInData[1];
						break;
					case AudioOption.Types.FadeOut:
						float[] fadeOutData = option.GetValue<float[]>();
						Settings.FadeIn = fadeOutData[0];
						Settings.FadeInEase = (Tweening.Ease)fadeOutData[1];
						break;
					case AudioOption.Types.Loop:
						Settings.Loop = option.GetValue<bool>();
						break;
					default:
						for (int i = 0; i < _sources.Count; i++)
							_sources[i].ApplyOption(option, false);
						break;
				}
			}

			if (recycle)
				Pool<AudioOption>.Recycle(option);
		}

		public override void SetScheduledTime(double time)
		{
			if (_state == AudioStates.Stopped || _scheduleStarted)
				return;

			_scheduledTime = time;

			for (int i = 0; i < _sources.Count; i++)
				_sources[i].SetScheduledTime(_scheduledTime);
		}

		public override double RemainingTime()
		{
			if (_state == AudioStates.Stopped)
				return 0d;

			double remainingTime = 0d;

			for (int i = 0; i < _sources.Count; i++)
				remainingTime = Math.Max(remainingTime, _sources[i].RemainingTime());

			return remainingTime;
		}

		public override void Break()
		{
			for (int i = 0; i < _sources.Count; i++)
			{
				AudioItem source = _sources[i];

				if (source.State != AudioStates.Waiting && source.GetScheduledTime() <= AudioSettings.dspTime)
					source.Break();
			}

			_break = true;
		}

		public override void SetRTPCValue(string name, float value)
		{
			if (State == AudioStates.Stopped)
				return;

			AudioRTPC rtpc = Settings.GetRTPC(name);

			if (rtpc != null)
				rtpc.SetValue(value);

			for (int i = 0; i < _sources.Count; i++)
				_sources[i].SetRTPCValue(name, value);
		}

		public override List<AudioItem> GetChildren()
		{
			if (_state == AudioStates.Stopped)
				return null;

			return _sources;
		}

		protected virtual AudioItem AddSource(AudioContainerSourceData data)
		{
			AudioItem item = null;

			if (data != null)
				item = AddSource(data.Settings, data.Options);

			return item;
		}

		protected virtual AudioItem AddSource(AudioSettingsBase settings, List<AudioOption> options)
		{
			AudioItem item = null;

			if (settings != null)
			{
				item = PAudio.Instance.ItemManager.CreateItem(settings, _spatializer, this);

				if (options != null)
				{
					for (int i = 0; i < options.Count; i++)
						item.ApplyOption(options[i], false);
				}

				_sources.Add(item);
				_volumeModifier.SimulateChange();
				_pitchModifier.SimulateChange();
			}

			return item;
		}

		protected virtual void RemoveSource(int index)
		{
			_sources.RemoveAt(index);
		}

		protected virtual void Reset()
		{
			SetState(AudioStates.Waiting);
			InitializeSources();
			Play();
		}

		protected abstract void Recycle();

		public override void OnRecycle()
		{
			base.OnRecycle();

			_sources.Clear();
		}

		public void Copy(AudioContainerItem reference)
		{
			base.Copy(reference);

		}
	}
}