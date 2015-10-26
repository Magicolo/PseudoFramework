using UnityEngine;
using System.Collections;
using Pseudo;
using System;
using System.Collections.Generic;

namespace Pseudo.Internal.Audio
{
	public class AudioMixContainerItem : AudioContainerItem, ICopyable<AudioMixContainerItem>
	{
		AudioMixContainerSettings _originalSettings;
		AudioMixContainerSettings _settings;
		double _deltaTime;
		double _lastTime;

		readonly List<double> _delays = new List<double>();

		public override AudioTypes Type { get { return AudioTypes.MixContainer; } }
		public override AudioSettingsBase Settings { get { return _settings; } }

		public static AudioMixContainerItem Default = new AudioMixContainerItem();

		public void Initialize(AudioMixContainerSettings settings, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(settings.GetHashCode(), settings.Name, spatializer, parent);

			_originalSettings = settings;
			_settings = Pool<AudioMixContainerSettings>.Create(settings);

			InitializeModifiers(_originalSettings);
			InitializeSources();

			for (int i = 0; i < _originalSettings.Options.Count; i++)
				ApplyOption(_originalSettings.Options[i], false);
		}

		protected override void InitializeSources()
		{
			for (int i = 0; i < _originalSettings.Sources.Count; i++)
			{
				if (AddSource(_originalSettings.Sources[i]) != null)
					_delays.Add(_originalSettings.Delays[i]);
			}
		}

		public override void Update()
		{
			base.Update();

			UpdateScheduledTime();
		}

		protected void UpdateScheduledTime()
		{
			if (_state == AudioStates.Stopped)
				return;

			// Update delta time
			double dspTime = Math.Max(AudioSettings.dspTime, _scheduledTime);

			_deltaTime = dspTime - _lastTime;
			_lastTime = dspTime;

			// Decrease delay counters
			for (int i = 0; i < _delays.Count; i++)
			{
				if (_state != AudioStates.Paused)
					_delays[i] = Math.Max(_delays[i] - _deltaTime, 0d);
			}

			// Schedule sources
			for (int i = 0; i < _sources.Count; i++)
			{
				AudioItem item = _sources[i];
				double time = Math.Max(AudioSettings.dspTime, _scheduledTime) + _delays[i];

				if (_state == AudioStates.Playing && item.State == AudioStates.Waiting)
					item.PlayScheduled(time);
				else
					item.SetScheduledTime(time);
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

		protected override void RemoveSource(int index)
		{
			base.RemoveSource(index);

			_delays.RemoveAt(index);
		}

		protected override void Recycle()
		{
			Pool<AudioMixContainerItem>.Recycle(this);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			_delays.Clear();
			Pool<AudioMixContainerSettings>.Recycle(ref _settings);
		}

		public void Copy(AudioMixContainerItem reference)
		{
			base.Copy(reference);

			_originalSettings = reference._originalSettings;
			_settings = reference._settings;
			_deltaTime = reference._deltaTime;
			_lastTime = reference._lastTime;
		}
	}
}