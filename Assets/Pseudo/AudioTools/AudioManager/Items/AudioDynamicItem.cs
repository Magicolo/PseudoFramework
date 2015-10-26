using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using UnityEngine.Audio;
using Pseudo.Internal;
using Pseudo.Internal.Audio;

namespace Pseudo
{
	public class AudioDynamicItem : AudioContainerItem, ICopyable<AudioDynamicItem>
	{
		Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> _getNextSettings;
		AudioDynamicSettings _settings;
		int _currentStep;
		bool _requestNextSettings = true;
		bool _breakSequence;
		double _deltaTime;
		double _lastTime;

		protected readonly List<AudioDynamicData> _data = new List<AudioDynamicData>();

		public override AudioTypes Type { get { return AudioTypes.Dynamic; } }
		public override AudioSettingsBase Settings { get { return _settings; } }
		public int CurrentStep { get { return _currentStep; } }

		public static AudioDynamicItem Default = new AudioDynamicItem();

		public void Initialize(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(getNextSettings.GetHashCode(), getNextSettings.Method.Name, spatializer, parent);

			_getNextSettings = getNextSettings;
			_settings = Pool<AudioDynamicSettings>.Create(AudioDynamicSettings.Default);

			InitializeModifiers(_settings);
			InitializeSources();
		}

		protected override void InitializeSources()
		{
			UpdateSequence();
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
			if (_breakSequence || (_sources.Count > 0 && !_requestNextSettings))
				return;

			AudioDynamicData data = Pool<AudioDynamicData>.Create(AudioDynamicData.Default);
			AudioSettingsBase settings = _getNextSettings(this, data);

			_currentStep++;

			if (settings == null || _state == AudioStates.Stopped)
				_breakSequence = true;
			else
				AddSource(settings, data);
		}

		protected void UpdateDeltaTime()
		{
			double dspTime = Math.Max(AudioSettings.dspTime, _scheduledTime);
			_deltaTime = dspTime - _lastTime;
			_lastTime = dspTime;
		}

		protected void UpdateScheduledTime()
		{
			if (_state == AudioStates.Stopped)
				return;

			UpdateDeltaTime();
			double remainingTime = 0d;

			_requestNextSettings = _state != AudioStates.Paused;

			for (int i = 0; i < _sources.Count; i++)
			{
				AudioItem source = _sources[i];
				AudioDynamicData data = _data[i];

				// Decrease delay
				if (_state != AudioStates.Paused)
				{
					switch (data.PlayMode)
					{
						case AudioDynamicData.PlayModes.Now:
							data.Delay = Math.Max(data.Delay - _deltaTime, 0d);
							break;
						case AudioDynamicData.PlayModes.After:
							if (i == 0)
								data.Delay = Math.Max(data.Delay - _deltaTime, 0d);
							else
								_requestNextSettings = false;
							break;
					}

					if (data.Delay > 0d)
						_requestNextSettings = false;
				}

				// Schedule source
				double time = Math.Max(AudioSettings.dspTime, _scheduledTime) + remainingTime + data.Delay;

				if (_state == AudioStates.Playing && source.State == AudioStates.Waiting)
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
			if (_state != AudioStates.Waiting)
				return;

			_lastTime = Math.Max(AudioSettings.dspTime, _scheduledTime);

			base.Play();
		}

		public override void Stop()
		{
			if (_state == AudioStates.Stopped || _state == AudioStates.Stopping)
				return;

			_breakSequence = true;

			base.Stop();
		}

		public override void StopImmediate()
		{
			if (_state == AudioStates.Stopped)
				return;

			_breakSequence = true;

			base.StopImmediate();
		}

		protected void AddSource(AudioSettingsBase settings, AudioDynamicData data)
		{
			AudioItem item = base.AddSource(settings, null);

			if (item == null)
				return;

			if (data.OnInitialize != null)
			{
				data.OnInitialize(item);
				data.OnInitialize = null;
			}

			_data.Add(data);
		}

		protected override void RemoveSource(int index)
		{
			base.RemoveSource(index);

			Pool<AudioDynamicData>.Recycle(_data.Pop(index));
			UpdateSequence();
		}

		protected override void Recycle()
		{
			Pool<AudioDynamicItem>.Recycle(this);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			Pool<AudioDynamicSettings>.Recycle(ref _settings);
			Pool<AudioDynamicData>.RecycleElements(_data);
			_data.Clear();
		}

		public void Copy(AudioDynamicItem reference)
		{
			base.Copy(reference);

			_getNextSettings = reference._getNextSettings;
			_settings = reference._settings;
			_currentStep = reference._currentStep;
			_requestNextSettings = reference._requestNextSettings;
			_breakSequence = reference._breakSequence;
			_deltaTime = reference._deltaTime;
			_lastTime = reference._lastTime;
		}
	}
}