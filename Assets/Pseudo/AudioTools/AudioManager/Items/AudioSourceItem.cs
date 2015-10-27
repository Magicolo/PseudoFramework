using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using UnityEngine.Audio;
using Pseudo.Internal;
using Pseudo.Internal.Audio;

namespace Pseudo.Internal.Audio
{
	public class AudioSourceItem : AudioItem, ICopyable<AudioSourceItem>
	{
		AudioSourceSettings _originalSettings;
		AudioSourceSettings _settings;
		AudioSource _source;
		float _deltaTime;
		float _lastTime;

		readonly Action<AudioModifier> _setVolumeScale;
		readonly Action<AudioModifier> _setPitchScale;

		public AudioSource Source { get { return _source; } }
		public override AudioTypes Type { get { return AudioTypes.Source; } }
		public override AudioSettingsBase Settings { get { return _settings; } }

		public static readonly AudioSourceItem Default = new AudioSourceItem();

		public AudioSourceItem()
		{
			_setVolumeScale = modifier => _source.volume = modifier.Value;
			_setPitchScale = modifier => _source.pitch = modifier.Value;
		}

		public void Initialize(AudioSourceSettings settings, AudioSource audioSource, AudioSpatializer spatializer, AudioItem parent)
		{
			base.Initialize(settings.GetHashCode(), settings.Name, spatializer, parent);

			// General Setup
			_originalSettings = settings;
			_settings = Pool<AudioSourceSettings>.Create(settings);
			_source = audioSource;
			_source.transform.parent = PAudio.Instance.CachedTransform;
			_spatializer.AddSource(_source.transform);

			// Setup the AudioSource
			_source.Stop();
			_source.clip = _settings.Clip;
			_source.name = settings.Name;
			_source.outputAudioMixerGroup = _settings.Output;
			_source.loop = _settings.Loop;
			_source.spatialBlend = _spatializer.SpatializeMode == AudioSpatializer.SpatializeModes.None ? 0f : _source.spatialBlend;

			InitializeModifiers(_originalSettings);

			for (int i = 0; i < _originalSettings.Options.Count; i++)
				ApplyOption(_originalSettings.Options[i], false);

			if (_settings.MaxInstances > 0)
				PAudio.Instance.ItemManager.TrimInstances(this, _settings.MaxInstances);
		}

		protected void InitializeModifiers(AudioSettingsBase settings)
		{
			_volumeModifier.OnValueChanged += _setVolumeScale;
			_volumeModifier.InitialValue = settings.VolumeScale;
			_volumeModifier.RandomModifier = 1f + UnityEngine.Random.Range(-settings.RandomVolume, settings.RandomVolume);

			_pitchModifier.OnValueChanged += _setPitchScale;
			_pitchModifier.InitialValue = settings.PitchScale;
			_pitchModifier.RandomModifier = 1f + UnityEngine.Random.Range(-settings.RandomPitch, settings.RandomPitch);
		}

		public override void Update()
		{
			if (_state == AudioStates.Stopped)
				return;

			UpdateDeltaTime();
			UpdatePlayback();

			base.Update();
		}

		protected void UpdateDeltaTime()
		{
			float time = _source.time;
			_deltaTime = time > _lastTime ? Mathf.Abs(time - _lastTime) : 0f;
			_lastTime = time;
		}

		protected void UpdatePlayback()
		{
			if (_state == AudioStates.Paused)
				return;

			// Stops AudioSources that have no AudioClip
			if (_state == AudioStates.Playing && !_source.isPlaying)
			{
				StopImmediate();
				return;
			}

			// Clamps the playback position of the AudioSource to the AudioSourceSettings's PlayRange
			float startTime = _source.clip.length * _settings.PlayRangeStart;
			float endTime = _source.clip.length * _settings.PlayRangeEnd;

			if (_source.loop && !_break)
			{
				if ((_settings.PlayRangeStart > 0f && _source.time < startTime) || (_settings.PlayRangeEnd < 1f && _source.time >= endTime))
				{
					_source.time = startTime;
					_lastTime = _source.time;
				}
			}
			else if (_source.time >= endTime - _settings.FadeOut)
				Stop();
		}

		public override void Play()
		{
			if (_state != AudioStates.Waiting)
				return;

			Spatialize();
			UpdateRTPCs();

			if (_source.clip != null && _settings.PlayRangeEnd - _settings.PlayRangeStart > 0f)
			{
				_source.time = _source.clip.length * _settings.PlayRangeStart;
				_lastTime = _source.time;

				if (_settings.FadeIn > 0f)
					FadeIn();

				if (_scheduledTime > 0d)
					_source.PlayScheduled(_scheduledTime);
				else
					_source.Play();
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

			_pausedState = _state;
			_source.Pause();
			SetState(AudioStates.Paused);
			RaisePauseEvent();
		}

		public override void Resume()
		{
			if (_state != AudioStates.Paused)
				return;

			_source.UnPause();
			SetState(_pausedState);
			RaiseResumeEvent();
		}

		public override void Stop()
		{
			if (_state == AudioStates.Stopping || _state == AudioStates.Stopped)
				return;

			_fadeTweener.Stop();

			if (_settings.FadeOut > 0f && _state != AudioStates.Paused)
			{
				FadeOut();
				SetState(AudioStates.Stopping);
				RaiseStoppingEvent();
			}
			else
				StopImmediate();
		}

		public override void StopImmediate()
		{
			if (_state == AudioStates.Stopped)
				return;

			_source.Stop();
			SetState(AudioStates.Stopped);
			RaiseStopEvent();

			if (_parent == null)
				PAudio.Instance.ItemManager.Deactivate(this);

			_spatializer.RemoveSource(_source.transform);

			Pool<AudioSourceItem>.Recycle(this);
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
						_settings.RandomVolume = randomVolume;
						_volumeModifier.RandomModifier = 1f + UnityEngine.Random.Range(-randomVolume, randomVolume);
						break;
					case AudioOption.Types.RandomPitch:
						float randomPitch = option.GetValue<float>();
						_settings.RandomPitch = randomPitch;
						_pitchModifier.RandomModifier = 1f + UnityEngine.Random.Range(-randomPitch, randomPitch);
						break;
					case AudioOption.Types.FadeIn:
						float[] fadeInData = option.GetValue<float[]>();
						_settings.FadeIn = fadeInData[0];
						_settings.FadeInEase = (Tweening.Ease)fadeInData[1];
						break;
					case AudioOption.Types.FadeOut:
						float[] fadeOutData = option.GetValue<float[]>();
						_settings.FadeIn = fadeOutData[0];
						_settings.FadeInEase = (Tweening.Ease)fadeOutData[1];
						break;
					case AudioOption.Types.Loop:
						bool loop = option.GetValue<bool>();
						_settings.Loop = loop;
						_source.loop = loop && !_break;
						break;
					case AudioOption.Types.Clip:
						AudioClip clip = option.GetValue<AudioClip>();
						_settings.Clip = clip;
						_source.clip = clip;
						break;
					case AudioOption.Types.Output:
						AudioMixerGroup output = option.GetValue<AudioMixerGroup>();
						_settings.Output = output;
						_source.outputAudioMixerGroup = output;
						break;
					case AudioOption.Types.DopplerLevel:
						_source.dopplerLevel = option.GetValue<float>();
						break;
					case AudioOption.Types.RolloffMode:
						if (option.HasCurve())
						{
							_source.rolloffMode = AudioRolloffMode.Custom;
							_source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, option.GetValue<AnimationCurve>());
						}
						else
							_source.rolloffMode = option.GetValue<AudioRolloffMode>();
						break;
					case AudioOption.Types.MinDistance:
						_source.minDistance = option.GetValue<float>();
						break;
					case AudioOption.Types.MaxDistance:
						_source.maxDistance = option.GetValue<float>();
						break;
					case AudioOption.Types.Spread:
						if (option.HasCurve())
							_source.SetCustomCurve(AudioSourceCurveType.Spread, option.GetValue<AnimationCurve>());
						else
							_source.spread = option.GetValue<float>();
						break;
					case AudioOption.Types.Mute:
						_source.mute = option.GetValue<bool>();
						break;
					case AudioOption.Types.BypassEffects:
						_source.bypassEffects = option.GetValue<bool>();
						break;
					case AudioOption.Types.BypassListenerEffects:
						_source.bypassListenerEffects = option.GetValue<bool>();
						break;
					case AudioOption.Types.BypassReverbZones:
						_source.bypassReverbZones = option.GetValue<bool>();
						break;
					case AudioOption.Types.Priority:
						_source.priority = option.GetValue<int>();
						break;
					case AudioOption.Types.StereoPan:
						_source.panStereo = option.GetValue<float>();
						break;
					case AudioOption.Types.SpatialBlend:
						if (option.HasCurve())
							_source.SetCustomCurve(AudioSourceCurveType.SpatialBlend, option.GetValue<AnimationCurve>());
						else
							_source.spatialBlend = option.GetValue<float>();
						break;
					case AudioOption.Types.ReverbZoneMix:
						if (option.HasCurve())
							_source.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, option.GetValue<AnimationCurve>());
						else
							_source.reverbZoneMix = option.GetValue<float>();
						break;
					case AudioOption.Types.PlayRange:
						Vector2 playRangeData = option.GetValue<Vector2>();
						_settings.PlayRangeStart = playRangeData.x;
						_settings.PlayRangeEnd = playRangeData.y;
						_lastTime = _source.time;
						break;
					case AudioOption.Types.Time:
						_source.time = option.GetValue<float>();
						_lastTime = _source.time;
						break;
					case AudioOption.Types.TimeSamples:
						_source.timeSamples = option.GetValue<int>();
						_lastTime = _source.time;
						break;
					case AudioOption.Types.VelocityUpdateMode:
						_source.velocityUpdateMode = option.GetValue<AudioVelocityUpdateMode>();
						break;
					case AudioOption.Types.IgnoreListenerPause:
						_source.ignoreListenerPause = option.GetValue<bool>();
						break;
					case AudioOption.Types.IgnoreListenerVolume:
						_source.ignoreListenerVolume = option.GetValue<bool>();
						break;
					case AudioOption.Types.Spatialize:
						_source.spatialize = option.GetValue<bool>();
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
			_source.SetScheduledStartTime(time);
		}

		public override double RemainingTime()
		{
			if (_state == AudioStates.Stopped || _source == null || _source.clip == null)
				return 0d;

			float endTime = _settings.PlayRangeEnd * _source.clip.length;
			double scheduledOffset = Math.Max(_scheduledTime - AudioSettings.dspTime, 0d);
			double remainingTime = (endTime - _source.time) / _source.pitch + scheduledOffset;

			return Math.Max(remainingTime, 0d);
		}

		public override void Break()
		{
			_break = true;
			_source.loop = false;
		}

		public override void SetRTPCValue(string name, float value)
		{
			if (_state == AudioStates.Stopped)
				return;

			AudioRTPC rtpc = _settings.GetRTPC(name);

			if (rtpc != null)
				rtpc.SetValue(value);
		}

		public override List<AudioItem> GetChildren()
		{
			return null;
		}

		protected override float GetDeltaTime()
		{
			return _deltaTime;
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			if (Application.isPlaying)
				ComponentPool<AudioSource>.Recycle(ref _source);
			else
				_source.gameObject.Destroy();

			Pool<AudioSourceSettings>.Recycle(ref _settings);
		}

		public void Copy(AudioSourceItem reference)
		{
			base.Copy(reference);

			_originalSettings = reference._originalSettings;
			_settings = reference._settings;
			_source = reference._source;
			_deltaTime = reference._deltaTime;
			_lastTime = reference._lastTime;
		}
	}
}