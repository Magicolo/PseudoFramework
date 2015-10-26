using UnityEngine;
using System.Collections;
using Pseudo;
using System;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	public class AudioSequenceContainerItem : AudioContainerItem, ICopyable<AudioSequenceContainerItem>
	{
		AudioSequenceContainerSettings _originalSettings;
		AudioSequenceContainerSettings _settings;
		double _deltaTime;
		double _lastTime;
		double _delay;
		int _sourcesIndex;

		public override AudioTypes Type { get { return AudioTypes.SequenceContainer; } }
		public override AudioSettingsBase Settings { get { return _settings; } }

		public static AudioSequenceContainerItem Default = new AudioSequenceContainerItem();

		public void Initialize(AudioSequenceContainerSettings settings, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(settings.GetHashCode(), settings.Name, spatializer, parent);

			_originalSettings = settings;
			_settings = Pool<AudioSequenceContainerSettings>.Create(settings);

			InitializeModifiers(_originalSettings);
			InitializeSources();

			for (int i = 0; i < _originalSettings.Options.Count; i++)
				ApplyOption(_originalSettings.Options[i], false);
		}

		protected override void InitializeSources()
		{
			_sourcesIndex = 0;
			CopyHelper.CopyTo(_originalSettings.Delays, ref _settings.Delays);

			if (_originalSettings.Sources.Count > 0)
				AddSource(_originalSettings.Sources[_sourcesIndex++]);
		}

		public override void Update()
		{
			if (_state == AudioStates.Stopped)
				return;

			UpdateSequence();
			base.Update();
			UpdateScheduledTime();
		}

		protected void UpdateSequence()
		{
			if (!IsPlaying || _state == AudioStates.Stopping)
				return;

			if (_sources.Count < 2 && _sourcesIndex < _originalSettings.Sources.Count)
				AddSource(_originalSettings.Sources[_sourcesIndex++]);
		}

		protected void UpdateDeltaTime()
		{
			double dspTime = AudioSettings.dspTime;
			_deltaTime = Math.Max(dspTime - _lastTime, 0d);
			_lastTime = dspTime;
		}

		protected void UpdateDelays()
		{
			if (_state == AudioStates.Stopped)
				return;

			UpdateDeltaTime();

			_delay = 0d;

			for (int i = 0; i < _sourcesIndex - _sources.Count; i++)
			{
				double currentDelay = _settings.Delays[i];

				if (_state != AudioStates.Paused)
					currentDelay = Math.Max(currentDelay - _deltaTime, 0d);

				_delay += currentDelay;
				_settings.Delays[i] = currentDelay;
			}
		}

		protected void UpdateScheduledTime()
		{
			if (_state == AudioStates.Stopped)
				return;

			UpdateDelays();

			// Schedule sources
			int delayIndex = _sourcesIndex - _sources.Count;
			double remainingTime = 0d;

			for (int i = 0; i < _sources.Count; i++)
			{
				AudioItem item = _sources[i];
				double time;

				if (i == 0)
					time = Math.Max(AudioSettings.dspTime, _scheduledTime) + _delay;
				else
					time = AudioSettings.dspTime + remainingTime;

				if (_state == AudioStates.Playing && item.State == AudioStates.Waiting)
					item.PlayScheduled(time);
				else
					item.SetScheduledTime(time);

				if (delayIndex < _settings.Delays.Count)
					remainingTime = item.RemainingTime() + _settings.Delays[delayIndex++];
				else
					remainingTime = item.RemainingTime();
			}
		}

		public override void Play()
		{
			if (_state != AudioStates.Waiting)
				return;

			_lastTime = Math.Max(AudioSettings.dspTime, _scheduledTime);
			UpdateScheduledTime();

			base.Play();
		}

		public override void SetScheduledTime(double time)
		{
			if (_state == AudioStates.Stopped || _scheduleStarted)
				return;

			_scheduledTime = time;
			_lastTime = time;

			UpdateScheduledTime();
		}

		public override double RemainingTime()
		{
			if (_state == AudioStates.Stopped || _sources.Count == 0)
				return 0d;

			return _sources.Last().RemainingTime();
		}

		protected override void Recycle()
		{
			Pool<AudioSequenceContainerItem>.Recycle(this);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			Pool<AudioSequenceContainerSettings>.Recycle(ref _settings);
		}

		public void Copy(AudioSequenceContainerItem reference)
		{
			base.Copy(reference);

			_originalSettings = reference._originalSettings;
			_settings = reference._settings;
			_deltaTime = reference._deltaTime;
			_lastTime = reference._lastTime;
			_delay = reference._delay;
			_sourcesIndex = reference._sourcesIndex;
		}
	}
}