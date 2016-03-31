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

namespace Pseudo.Audio.Internal
{
	public class AudioSourceItem : AudioItemBase, ICopyable<AudioSourceItem>
	{
		AudioSourceSettings originalSettings;
		AudioSourceSettings settings;
		AudioSource source;
		float deltaTime;
		float lastTime;

		readonly Action<AudioModifier> setVolumeScale;
		readonly Action<AudioModifier> setPitchScale;

		public AudioSource Source { get { return source; } }
		public override AudioTypes Type { get { return AudioTypes.Source; } }
		public override AudioSettingsBase Settings { get { return settings; } }

		public AudioSourceItem()
		{
			setVolumeScale = modifier => source.volume = modifier.Value;
			setPitchScale = modifier => source.pitch = modifier.Value;
		}

		public void Initialize(AudioSourceSettings settings, AudioItemManager itemManager, AudioSource audioSource, AudioSpatializer spatializer, IAudioItem parent)
		{
			base.Initialize(settings.Identifier, itemManager, spatializer, parent);

			// General Setup
			originalSettings = settings;
			this.settings = PrefabPoolManager.Create(settings);
			source = audioSource;
			source.transform.parent = itemManager.AudioManager.Reference.transform.parent;
			base.spatializer.AddSource(source.transform);

			// Setup the AudioSource
			source.Stop();
			source.clip = this.settings.Clip;
			source.name = settings.Name;
			source.outputAudioMixerGroup = this.settings.Output;
			source.loop = this.settings.Loop;
			source.spatialBlend = base.spatializer.SpatializeMode == AudioSpatializer.SpatializeModes.None ? 0f : source.spatialBlend;

			InitializeModifiers(originalSettings);

			for (int i = 0; i < originalSettings.Options.Count; i++)
				ApplyOption(originalSettings.Options[i], false);

			if (this.settings.MaxInstances > 0)
				itemManager.TrimInstances(this, this.settings.MaxInstances);
		}

		protected void InitializeModifiers(AudioSettingsBase settings)
		{
			volumeModifier.OnValueChanged += setVolumeScale;
			volumeModifier.InitialValue = settings.VolumeScale;
			volumeModifier.RandomModifier = 1f + UnityEngine.Random.Range(-settings.RandomVolume, settings.RandomVolume);

			pitchModifier.OnValueChanged += setPitchScale;
			pitchModifier.InitialValue = settings.PitchScale;
			pitchModifier.RandomModifier = 1f + UnityEngine.Random.Range(-settings.RandomPitch, settings.RandomPitch);
		}

		public override void Update()
		{
			if (state == AudioStates.Stopped)
				return;

			UpdateDeltaTime();
			UpdatePlayback();

			base.Update();
		}

		protected void UpdateDeltaTime()
		{
			float time = source.time;
			deltaTime = time > lastTime ? Mathf.Abs(time - lastTime) : 0f;
			lastTime = time;
		}

		protected void UpdatePlayback()
		{
			if (state == AudioStates.Paused)
				return;

			// Stops AudioSources that have no AudioClip
			if (state == AudioStates.Playing && !source.isPlaying)
			{
				StopImmediate();
				return;
			}

			// Clamps the playback position of the AudioSource to the AudioSourceSettings's PlayRange
			float startTime = source.clip.length * settings.PlayRangeStart;
			float endTime = source.clip.length * settings.PlayRangeEnd;

			if (source.loop && !hasBreak)
			{
				if ((settings.PlayRangeStart > 0f && source.time < startTime) || (settings.PlayRangeEnd < 1f && source.time >= endTime))
				{
					source.time = startTime;
					lastTime = source.time;
				}
			}
			else if (source.time >= endTime - settings.FadeOut)
				Stop();
		}

		public override void Play()
		{
			if (state != AudioStates.Waiting)
				return;

			Spatialize();
			UpdateRTPCs();

			if (source.clip != null && settings.PlayRangeEnd - settings.PlayRangeStart > 0f)
			{
				source.time = source.clip.length * settings.PlayRangeStart;
				lastTime = source.time;

				if (settings.FadeIn > 0f)
					FadeIn();

				if (scheduledTime > 0d)
					source.PlayScheduled(scheduledTime);
				else
					source.Play();
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

			pausedState = state;
			source.Pause();
			SetState(AudioStates.Paused);
			RaisePauseEvent();
		}

		public override void Resume()
		{
			if (state != AudioStates.Paused)
				return;

			source.UnPause();
			SetState(pausedState);
			RaiseResumeEvent();
		}

		public override void Stop()
		{
			if (state == AudioStates.Stopping || state == AudioStates.Stopped)
				return;

			fadeTweener.Stop();

			if (settings.FadeOut > 0f && state != AudioStates.Paused)
			{
				FadeOut();
				SetState(AudioStates.Stopping);
				RaiseStoppingEvent();
			}
			else
			{
				RaiseStoppingEvent();
				StopImmediate();
			}
		}

		public override void StopImmediate()
		{
			if (state == AudioStates.Stopped)
				return;

			source.Stop();
			SetState(AudioStates.Stopped);
			RaiseStopEvent();

			if (parent == null)
				itemManager.Deactivate(this);

			spatializer.RemoveSource(source.transform);
			TypePoolManager.Recycle(this);
		}

		protected override void ApplyOptionNow(AudioOption option, bool recycle)
		{
			if (state != AudioStates.Stopped)
			{
				switch (option.Type)
				{
					case AudioOption.Types.VolumeScale:
						var volumeData = option.GetValue<Vector3>();
						SetVolumeScale(volumeData.x, volumeData.y, (TweenUtility.Ease)volumeData.z, true);
						break;
					case AudioOption.Types.PitchScale:
						var pitchData = option.GetValue<Vector3>();
						SetPitchScale(pitchData.x, pitchData.y, (TweenUtility.Ease)pitchData.z, true);
						break;
					case AudioOption.Types.RandomVolume:
						float randomVolume = option.GetValue<float>();
						settings.RandomVolume = randomVolume;
						volumeModifier.RandomModifier = 1f + UnityEngine.Random.Range(-randomVolume, randomVolume);
						break;
					case AudioOption.Types.RandomPitch:
						float randomPitch = option.GetValue<float>();
						settings.RandomPitch = randomPitch;
						pitchModifier.RandomModifier = 1f + UnityEngine.Random.Range(-randomPitch, randomPitch);
						break;
					case AudioOption.Types.FadeIn:
						var fadeInData = option.GetValue<Vector2>();
						settings.FadeIn = fadeInData.x;
						settings.FadeInEase = (TweenUtility.Ease)fadeInData.y;
						break;
					case AudioOption.Types.FadeOut:
						var fadeOutData = option.GetValue<Vector2>();
						settings.FadeIn = fadeOutData.x;
						settings.FadeInEase = (TweenUtility.Ease)fadeOutData.y;
						break;
					case AudioOption.Types.Loop:
						bool loop = option.GetValue<bool>();
						settings.Loop = loop;
						source.loop = loop && !hasBreak;
						break;
					case AudioOption.Types.Clip:
						var clip = option.GetValue<AudioClip>();
						settings.Clip = clip;
						source.clip = clip;
						break;
					case AudioOption.Types.Output:
						var output = option.GetValue<AudioMixerGroup>();
						settings.Output = output;
						source.outputAudioMixerGroup = output;
						break;
					case AudioOption.Types.DopplerLevel:
						source.dopplerLevel = option.GetValue<float>();
						break;
					case AudioOption.Types.RolloffMode:
						if (option.HasCurve())
						{
							source.rolloffMode = AudioRolloffMode.Custom;
							source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, option.GetValue<AnimationCurve>());
						}
						else
							source.rolloffMode = option.GetValue<AudioRolloffMode>();
						break;
					case AudioOption.Types.MinDistance:
						source.minDistance = option.GetValue<float>();
						break;
					case AudioOption.Types.MaxDistance:
						source.maxDistance = option.GetValue<float>();
						break;
					case AudioOption.Types.Spread:
						if (option.HasCurve())
							source.SetCustomCurve(AudioSourceCurveType.Spread, option.GetValue<AnimationCurve>());
						else
							source.spread = option.GetValue<float>();
						break;
					case AudioOption.Types.Mute:
						source.mute = option.GetValue<bool>();
						break;
					case AudioOption.Types.BypassEffects:
						source.bypassEffects = option.GetValue<bool>();
						break;
					case AudioOption.Types.BypassListenerEffects:
						source.bypassListenerEffects = option.GetValue<bool>();
						break;
					case AudioOption.Types.BypassReverbZones:
						source.bypassReverbZones = option.GetValue<bool>();
						break;
					case AudioOption.Types.Priority:
						source.priority = option.GetValue<int>();
						break;
					case AudioOption.Types.StereoPan:
						source.panStereo = option.GetValue<float>();
						break;
					case AudioOption.Types.SpatialBlend:
						if (option.HasCurve())
							source.SetCustomCurve(AudioSourceCurveType.SpatialBlend, option.GetValue<AnimationCurve>());
						else
							source.spatialBlend = option.GetValue<float>();
						break;
					case AudioOption.Types.ReverbZoneMix:
						if (option.HasCurve())
							source.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, option.GetValue<AnimationCurve>());
						else
							source.reverbZoneMix = option.GetValue<float>();
						break;
					case AudioOption.Types.PlayRange:
						var playRangeData = option.GetValue<Vector2>();
						settings.PlayRangeStart = playRangeData.x;
						settings.PlayRangeEnd = playRangeData.y;
						lastTime = source.time;
						break;
					case AudioOption.Types.Time:
						source.time = option.GetValue<float>();
						lastTime = source.time;
						break;
					case AudioOption.Types.TimeSamples:
						source.timeSamples = option.GetValue<int>();
						lastTime = source.time;
						break;
					case AudioOption.Types.VelocityUpdateMode:
						source.velocityUpdateMode = option.GetValue<AudioVelocityUpdateMode>();
						break;
					case AudioOption.Types.IgnoreListenerPause:
						source.ignoreListenerPause = option.GetValue<bool>();
						break;
					case AudioOption.Types.IgnoreListenerVolume:
						source.ignoreListenerVolume = option.GetValue<bool>();
						break;
					case AudioOption.Types.Spatialize:
						source.spatialize = option.GetValue<bool>();
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
			source.SetScheduledStartTime(time);
		}

		public override double RemainingTime()
		{
			if (state == AudioStates.Stopped || source == null || source.clip == null)
				return 0d;

			float endTime = settings.PlayRangeEnd * source.clip.length;
			double scheduledOffset = Math.Max(scheduledTime - AudioSettings.dspTime, 0d);
			double remainingTime = (endTime - source.time) / source.pitch + scheduledOffset;

			return Math.Max(remainingTime, 0d);
		}

		public override void Break()
		{
			hasBreak = true;
			source.loop = false;
		}

		public override void SetRTPCValue(string name, float value)
		{
			if (state == AudioStates.Stopped)
				return;

			var rtpc = settings.GetRTPC(name);

			if (rtpc != null)
				rtpc.SetValue(value);
		}

		protected override float GetDeltaTime()
		{
			return deltaTime;
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			PrefabPoolManager.Recycle(source);
			PrefabPoolManager.Recycle(ref settings);
		}

		public void Copy(AudioSourceItem reference)
		{
			base.Copy(reference);

			originalSettings = reference.originalSettings;
			settings = reference.settings;
			source = reference.source;
			deltaTime = reference.deltaTime;
			lastTime = reference.lastTime;
		}
	}
}