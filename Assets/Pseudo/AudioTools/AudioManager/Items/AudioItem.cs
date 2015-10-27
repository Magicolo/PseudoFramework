using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using Pseudo.Internal.Audio;

namespace Pseudo
{
	[Serializable]
	public abstract class AudioItem : IPoolable, ICopyable<AudioItem>
	{
		public enum AudioStates
		{
			Waiting,
			Playing,
			Paused,
			Stopping,
			Stopped,
		}

		public enum AudioTypes
		{
			Source,
			Dynamic,
			MixContainer,
			RandomContainer,
			EnumeratorContainer,
			SwitchContainer,
			SequenceContainer,
		}

		protected int _id;
		protected string _name;
		protected AudioStates _state;
		protected AudioSpatializer _spatializer;
		protected AudioItem _parent;
		protected double _scheduledTime;
		protected bool _scheduleStarted;
		protected AudioModifier _volumeModifier;
		protected AudioModifier _pitchModifier;
		protected FloatTweener _rampVolumeTweener;
		protected FloatTweener _rampParentVolumeTweener;
		protected FloatTweener _rampPitchTweener;
		protected FloatTweener _rampParentPitchTweener;
		protected FloatTweener _fadeTweener;
		protected AudioStates _pausedState;
		protected bool _hasFaded;
		protected bool _break;
		protected List<AudioDelayedOption> _delayedOptions = new List<AudioDelayedOption>();

		protected readonly Action _stopImmediate;
		protected readonly Func<float> _getDeltaTime;
		protected readonly Action<float> _setVolumeRampModifier;
		protected readonly Action<float> _setVolumeParentModifier;
		protected readonly Action<float> _setPitchRampModifier;
		protected readonly Action<float> _setPitchParentModifier;
		protected readonly Action<float> _setVolumeFadeModifier;

		/// <summary>
		/// The name of the AudioSettingsBase or method from which the AudioItem has been created.
		/// </summary>
		public string Name { get { return _name; } }
		/// <summary>
		/// The hashcode of the AudioSettingsBase or method from which the AudioItem has been created.
		/// </summary>
		public int Id { get { return _id; } }
		/// <summary>
		/// The current state of the AudioItem.
		/// </summary>
		public AudioStates State { get { return _state; } }
		/// <summary>
		/// The shared module that spatializes the AudioItem.
		/// </summary>
		public AudioSpatializer Spatializer { get { return _spatializer; } }
		/// <summary>
		/// Is the AudioItem actually emitting sound (takes into account scheduled sounds)?
		/// </summary>
		public bool IsPlaying { get { return (_state == AudioStates.Playing || _state == AudioStates.Stopping) && (_scheduledTime <= 0d || _scheduleStarted); } }
		/// <summary>
		/// The AudioSettingsBase used by the AudioItem (a copy of the AudioSettingsBase from which the AudioItem has been created).
		/// </summary>
		public abstract AudioSettingsBase Settings { get; }
		/// <summary>
		/// The type of the AudioItem.
		/// </summary>
		public abstract AudioTypes Type { get; }

		/// <summary>
		/// An event triggered when the Play() method is successfuly called.
		/// This event will be cleared automaticaly when the AudioItem is recycled.
		/// </summary>
		public event Action<AudioItem> OnPlay;
		/// <summary>
		/// An event triggered when the Pause() method is successfuly called.
		/// This event will be cleared automaticaly when the AudioItem is recycled.
		/// </summary>
		public event Action<AudioItem> OnPause;
		/// <summary>
		/// An event triggered when the Resume() method is successfuly called.
		/// This event will be cleared automaticaly when the AudioItem is recycled.
		/// </summary>
		public event Action<AudioItem> OnResume;
		/// <summary>
		/// An event triggered when the AudioItem starts fading out.
		/// This event will be cleared automaticaly when the AudioItem is recycled.
		/// </summary>
		public event Action<AudioItem> OnStopping;
		/// <summary>
		/// An event triggered when the AudioItem stops. After this event has been triggered, the AudioItem becomes obsolete.
		/// This event will be cleared automaticaly when the AudioItem is recycled.
		/// </summary>
		public event Action<AudioItem> OnStop;
		/// <summary>
		/// An event triggered on each Update(). Use this to insert dynamic logic into the AudioItem.
		/// This event will be cleared automaticaly when the AudioItem is recycled.
		/// </summary>
		public event Action<AudioItem> OnUpdate;
		/// <summary>
		/// An event triggered when the AudioItem changes its state.
		/// This event will be cleared automaticaly when the AudioItem is recycled.
		/// </summary>
		public event Action<AudioItem, AudioStates, AudioStates> OnStateChanged;

		protected AudioItem()
		{
			_stopImmediate = StopImmediate;
			_getDeltaTime = GetDeltaTime;
			_setVolumeRampModifier = value => _volumeModifier.RampModifier = value;
			_setVolumeParentModifier = value => _volumeModifier.ParentModifier = value;
			_setPitchRampModifier = value => _pitchModifier.RampModifier = value;
			_setPitchParentModifier = value => _pitchModifier.ParentModifier = value;
			_setVolumeFadeModifier = value => _volumeModifier.FadeModifier = value;
		}

		protected virtual void Initialize(int id, string name, AudioSpatializer spatializer, AudioItem parent)
		{
			_id = id;
			_name = name;
			_spatializer = spatializer;
			_parent = parent;

			if (_parent == null)
				PAudio.Instance.ItemManager.Activate(this);

			SetState(AudioStates.Waiting);
		}

		protected virtual void SetState(AudioStates state)
		{
			_break |= state == AudioStates.Stopping || state == AudioStates.Stopped;

			RaiseStateChangeEvent(_state, (_state = state));
		}

		protected virtual void RaisePlayEvent()
		{
			if (OnPlay != null)
				OnPlay(this);
		}

		protected virtual void RaisePauseEvent()
		{
			if (OnPause != null)
				OnPause(this);
		}

		protected virtual void RaiseResumeEvent()
		{
			if (OnResume != null)
				OnResume(this);
		}

		protected virtual void RaiseStoppingEvent()
		{
			if (OnStopping != null)
				OnStopping(this);
		}

		protected virtual void RaiseStopEvent()
		{
			if (OnStop != null)
				OnStop(this);
		}

		protected virtual void RaiseUpdateEvent()
		{
			if (OnUpdate != null)
				OnUpdate(this);
		}

		protected virtual void RaiseStateChangeEvent(AudioStates oldState, AudioStates newState)
		{
			if (OnStateChanged != null)
				OnStateChanged(this, oldState, newState);
		}

		protected virtual void FadeIn()
		{
			if (_hasFaded)
				return;

			_hasFaded = true;
			_fadeTweener.Ramp(0f, 1f, Settings.FadeIn, _setVolumeFadeModifier, ease: Settings.FadeInEase, getDeltaTime: _getDeltaTime);
		}

		protected virtual void FadeOut()
		{
			_fadeTweener.Ramp(_volumeModifier.FadeModifier, 0f, Settings.FadeOut, _setVolumeFadeModifier, ease: Settings.FadeOutEase, getDeltaTime: _getDeltaTime, endCallback: _stopImmediate);
		}

		protected virtual float GetDeltaTime()
		{
			return Application.isPlaying ? Time.unscaledDeltaTime : 0.01f;
		}

		protected virtual void Spatialize()
		{
			if (_parent == null)
				_spatializer.Spatialize();
		}

		protected virtual void UpdateOptions()
		{
			for (int i = 0; i < _delayedOptions.Count; i++)
			{
				AudioDelayedOption delayedOption = _delayedOptions[i];

				if (delayedOption.Update())
				{
					ApplyOptionNow(delayedOption.Option, delayedOption.Recycle);
					_delayedOptions.RemoveAt(i--);
					Pool<AudioDelayedOption>.Recycle(delayedOption);
				}
			}
		}

		protected void UpdateTweeners()
		{
			if (_state == AudioStates.Stopped)
				return;

			_rampVolumeTweener.Update();
			_rampParentVolumeTweener.Update();
			_rampPitchTweener.Update();
			_rampParentPitchTweener.Update();
			_fadeTweener.Update();
		}

		protected void UpdateRTPCs()
		{
			if (_state == AudioStates.Stopped)
				return;

			float volumeRtpc = 1f;
			float pitchRtpc = 1f;

			for (int i = 0; i < Settings.RTPCs.Count; i++)
			{
				AudioRTPC rtpc = Settings.RTPCs[i];

				switch (rtpc.Type)
				{
					case AudioRTPC.RTPCTypes.Volume:
						volumeRtpc *= rtpc.GetAdjustedValue();
						break;
					case AudioRTPC.RTPCTypes.Pitch:
						pitchRtpc *= rtpc.GetAdjustedValue();
						break;
				}
			}

			_volumeModifier.RTPCModifier = volumeRtpc;
			_pitchModifier.RTPCModifier = pitchRtpc;
		}

		/// <summary>
		/// Used internally to update the AudioItem and its hierarchy.
		/// </summary>
		public virtual void Update()
		{
			if (_state == AudioStates.Stopped)
				return;

			if (_scheduledTime > 0d)
				_scheduleStarted |= _scheduledTime <= AudioSettings.dspTime;

			Spatialize();
			UpdateOptions();
			UpdateRTPCs();
			UpdateTweeners();

			RaiseUpdateEvent();
		}

		/// <summary>
		/// Plays the AudioItem and its hierarchy.
		/// </summary>
		public abstract void Play();
		/// <summary>
		/// Plays the AudioItem and its hierarchy at the specified dsp time. The current dsp time can be retreived from <code> AudioSettings.dspTime </code>.
		/// </summary>
		/// <param name="time"> The dsp time at which the AudioItem should be scheduled. </param>
		public abstract void PlayScheduled(double time);
		/// <summary>
		/// Pauses the AudioItem and its hierarchy.
		/// </summary>
		public abstract void Pause();
		/// <summary>
		/// Resumes the paused AudioItem and its hierarchy.
		/// </summary>
		public abstract void Resume();
		/// <summary>
		/// Stops the AudioItem and its hierarchy with fade out.
		/// </summary>
		public abstract void Stop();
		/// <summary>
		/// Stops the AudioItem and its hierarchy immediatly.
		/// The AudioItem becomes obsolete after StopImmediate() has been called.
		/// </summary>
		public abstract void StopImmediate();
		/// <summary>
		/// Modifies a setting of the AudioItem and, when appropriate, of its hierarchy.
		/// </summary>
		/// <param name="option"> The AudioOption to be applied. </param>
		/// <param name="recycle"> Should the AudioOption be recycled after it has been applied? If true, the AudioOption will become obsolete after it has been applied. </param>
		public virtual void ApplyOption(AudioOption option, bool recycle = true)
		{
			if (option.Delay > 0f)
				ApplyOptionDelayed(option, recycle);
			else
				ApplyOptionNow(option, recycle);
		}
		protected virtual void ApplyOptionDelayed(AudioOption option, bool recycle)
		{
			AudioDelayedOption delayedOption = Pool<AudioDelayedOption>.Create(AudioDelayedOption.Default);
			delayedOption.Initialize(option, recycle, _getDeltaTime);
			_delayedOptions.Add(delayedOption);
		}
		protected abstract void ApplyOptionNow(AudioOption option, bool recycle);
		/// <summary>
		/// </summary>
		/// <returns> The dsp time at which the AudioItem has be scheduled or 0. </returns>
		public virtual double GetScheduledTime() { return _scheduledTime; }
		/// <summary>
		/// Sets the dsp time at which the AudioItem should be played.
		/// Trying to set the scheduled time after the AudioItem has started playing will not work.
		/// </summary>
		/// <param name="time"> The dsp time at which to schedule the AudioItem. </param>
		public abstract void SetScheduledTime(double time);
		/// <summary>
		/// This value will not take into account looping and will not be accurate for sequences and dynamic items.
		/// </summary>
		/// <returns> The remaining time before the AudioItem stops. </returns>
		public abstract double RemainingTime();
		/// <summary>
		/// Clears all looping flags from the AudioItem and its hierarchy.
		/// </summary>
		public abstract void Break();
		/// <summary>
		/// Sets the value of a defined RTPC value (RTPC values are defined in AudioSettingsBase assets).
		/// If the RTPC has been defined as global, all corresponding global RTPCs will also be set to the <paramref name="value"/>. Global RTPCs can also be accessed by AudioRTPC.GetGlobalRTPC(string name).
		/// </summary>
		/// <param name="name"> The name of the RTPC value. </param>
		/// <param name="value"> The value at which the RTPC should be set. </param>
		public abstract void SetRTPCValue(string name, float value);
		/// <summary>
		/// </summary>
		/// <returns> The parent of the AudioItem or null if the AudioItem is the root of its hierarchy. </returns>
		public virtual AudioItem GetParent() { return _parent; }
		/// <summary>
		/// The returned List<AudioItem> is the actual list used by the AudioItem, so care must be used when modifying it.
		/// </summary>
		/// <returns> The immediate children of the AudioItem or null if there are none. </returns>
		public abstract List<AudioItem> GetChildren();
		/// <summary>
		/// Sets all the AudioItem's events to null.
		/// </summary>
		public virtual void ClearEvents()
		{
			OnPlay = null;
			OnPause = null;
			OnResume = null;
			OnStopping = null;
			OnStop = null;
			OnUpdate = null;
			OnStateChanged = null;
		}

		/// <summary>
		/// </summary>
		/// <returns> The volume scale of the AudioItem. </returns>
		public virtual float GetVolumeScale()
		{
			if (_state == AudioStates.Stopped)
				return 0f;

			return _volumeModifier.Value;
		}
		protected virtual void SetVolumeScale(float volume, float time, Tweening.Ease ease, bool fromSelf)
		{
			if (_state == AudioStates.Stopped)
				return;

			if (fromSelf)
			{
				_rampVolumeTweener.Stop();

				if (time > 0f)
					_rampVolumeTweener.Ramp(_volumeModifier.RampModifier, volume, time, _setVolumeRampModifier, ease, _getDeltaTime);
				else
					_volumeModifier.RampModifier = volume;
			}
			else
			{
				_rampParentVolumeTweener.Stop();

				if (time > 0f)
					_rampParentVolumeTweener.Ramp(_volumeModifier.ParentModifier, volume, time, _setVolumeParentModifier, ease, _getDeltaTime);
				else
					_volumeModifier.ParentModifier = volume;
			}
		}
		/// <summary>
		/// Ramps the volume scale of the AudioItem and its hierarchy.
		/// Other volume modifiers such as fades remain unaffected.
		/// </summary>
		/// <param name="volume"> The target volume at which the AudioItem should be ramped to. </param>
		/// <param name="time"> The duration of the ramping. </param>
		/// <param name="ease"> The curve of the interpolation. </param>
		public virtual void SetVolumeScale(float volume, float time, Tweening.Ease ease = Tweening.Ease.Linear) { SetVolumeScale(volume, time, ease, false); }
		/// <summary>
		/// Sets the volume scale of the AudioItem and its hierarchy.
		/// Other volume modifiers such as fades remain unaffected.
		/// </summary>
		/// <param name="volume"> The target volume at which the AudioItem should be set to. </param>
		public virtual void SetVolumeScale(float volume) { SetVolumeScale(volume, 0f, Tweening.Ease.Linear, false); }

		/// <summary>
		/// </summary>
		/// <returns> The pitch scale of the AudioItem. </returns>
		public virtual float GetPitchScale()
		{
			if (_state == AudioStates.Stopped)
				return 0f;

			return _pitchModifier.Value;
		}
		protected virtual void SetPitchScale(float pitch, float time, Tweening.Ease ease, bool fromSelf)
		{
			if (_state == AudioStates.Stopped)
				return;

			if (fromSelf)
			{
				_rampPitchTweener.Stop();

				if (time > 0f)
					_rampPitchTweener.Ramp(_pitchModifier.RampModifier, pitch, time, _setPitchRampModifier, ease, _getDeltaTime);
				else
					_pitchModifier.RampModifier = pitch;
			}
			else
			{
				_rampParentPitchTweener.Stop();

				if (time > 0f)
					_rampParentPitchTweener.Ramp(_pitchModifier.ParentModifier, pitch, time, _setPitchParentModifier, ease, _getDeltaTime);
				else
					_pitchModifier.ParentModifier = pitch;
			}
		}
		/// <summary>
		/// Ramps the pitch scale of the AudioItem and its hierarchy.
		/// Other pitch modifiers such as fades remain unaffected.
		/// </summary>
		/// <param name="pitch"> The target pitch at which the AudioItem should be ramped to. </param>
		/// <param name="time"> The duration of the ramping. </param>
		/// <param name="ease"> The curve of the interpolation. </param>
		public virtual void SetPitchScale(float pitch, float time, Tweening.Ease ease = Tweening.Ease.Linear) { SetPitchScale(pitch, time, ease, false); }
		/// <summary>
		/// Sets the pitch scale of the AudioItem and its hierarchy.
		/// Other pitch modifiers such as fades remain unaffected.
		/// </summary>
		/// <param name="pitch"> The target pitch at which the AudioItem should be set to. </param>
		public virtual void SetPitchScale(float pitch) { SetPitchScale(pitch, 0f, Tweening.Ease.Linear, false); }

		/// <summary>
		/// Internaly used by the pooling system.
		/// </summary>
		public virtual void OnCreate()
		{
			_volumeModifier = Pool<AudioModifier>.Create(AudioModifier.Default);
			_pitchModifier = Pool<AudioModifier>.Create(AudioModifier.Default);
			_fadeTweener = Pool<FloatTweener>.Create(FloatTweener.Default);
			_rampVolumeTweener = Pool<FloatTweener>.Create(FloatTweener.Default);
			_rampParentVolumeTweener = Pool<FloatTweener>.Create(FloatTweener.Default);
			_rampPitchTweener = Pool<FloatTweener>.Create(FloatTweener.Default);
			_rampParentPitchTweener = Pool<FloatTweener>.Create(FloatTweener.Default);
			Pool<AudioDelayedOption>.CreateElements(_delayedOptions);
		}

		/// <summary>
		/// Internaly used by the pooling system.
		/// </summary>
		public virtual void OnRecycle()
		{
			Pool<AudioModifier>.Recycle(ref _volumeModifier);
			Pool<AudioModifier>.Recycle(ref _pitchModifier);
			Pool<FloatTweener>.Recycle(ref _fadeTweener);
			Pool<FloatTweener>.Recycle(ref _rampVolumeTweener);
			Pool<FloatTweener>.Recycle(ref _rampParentVolumeTweener);
			Pool<FloatTweener>.Recycle(ref _rampPitchTweener);
			Pool<FloatTweener>.Recycle(ref _rampParentPitchTweener);

			// Only the AudioItem root should recycle the spatializer as it is shared with it's children
			if (_parent == null)
				Pool<AudioSpatializer>.Recycle(ref _spatializer);

			RecycleDelayedOptions();
			ClearEvents();
		}

		void RecycleDelayedOptions()
		{
			for (int i = 0; i < _delayedOptions.Count; i++)
			{
				AudioDelayedOption delayedOption = _delayedOptions[i];

				if (delayedOption.Recycle)
					Pool<AudioOption>.Recycle(delayedOption.Option);

				Pool<AudioDelayedOption>.Recycle(delayedOption);
			}

			_delayedOptions.Clear();
		}

		/// <summary>
		/// Copies another AudioItem.
		/// </summary>
		/// <param name="reference"> The AudioItem to copy. </param>
		public void Copy(AudioItem reference)
		{
			_id = reference._id;
			_name = reference._name;
			_state = reference._state;
			_spatializer = reference._spatializer;
			_parent = reference._parent;
			_scheduledTime = reference._scheduledTime;
			_scheduleStarted = reference._scheduleStarted;
			_volumeModifier = reference._volumeModifier;
			_pitchModifier = reference._pitchModifier;
			_rampVolumeTweener = reference._rampVolumeTweener;
			_rampParentVolumeTweener = reference._rampParentVolumeTweener;
			_rampPitchTweener = reference._rampPitchTweener;
			_rampParentPitchTweener = reference._rampParentPitchTweener;
			_fadeTweener = reference._fadeTweener;
			_pausedState = reference._pausedState;
			_hasFaded = reference._hasFaded;
			_break = reference._break;
			CopyUtility.CopyTo(reference._delayedOptions, ref _delayedOptions);
			OnPlay = reference.OnPlay;
			OnPause = reference.OnPause;
			OnResume = reference.OnResume;
			OnStopping = reference.OnStopping;
			OnStop = reference.OnStop;
			OnUpdate = reference.OnUpdate;
			OnStateChanged = reference.OnStateChanged;
		}

		public override string ToString()
		{
			return string.Format("{0}({1}, {2})", GetType(), Name, _state);
		}
	}
}
