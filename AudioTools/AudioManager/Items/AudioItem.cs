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
	public abstract class AudioItem : IPoolable, ICopyable
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

		protected int id;
		protected string name;
		protected AudioStates state;
		protected AudioSpatializer spatializer;
		protected AudioItem parent;
		protected double scheduledTime;
		protected bool scheduleStarted;
		protected AudioModifier volumeModifier;
		protected AudioModifier pitchModifier;
		protected FloatTweener rampVolumeTweener;
		protected FloatTweener rampParentVolumeTweener;
		protected FloatTweener rampPitchTweener;
		protected FloatTweener rampParentPitchTweener;
		protected FloatTweener fadeTweener;
		protected AudioStates pausedState;
		protected bool hasFaded;
		protected bool hasBreak;
		protected List<AudioDelayedOption> delayedOptions = new List<AudioDelayedOption>();

		protected readonly Action stopImmediate;
		protected readonly Func<float> getDeltaTime;
		protected readonly Action<float> setVolumeRampModifier;
		protected readonly Action<float> setVolumeParentModifier;
		protected readonly Action<float> setPitchRampModifier;
		protected readonly Action<float> setPitchParentModifier;
		protected readonly Action<float> setVolumeFadeModifier;

		/// <summary>
		/// The name of the AudioSettingsBase or method from which the AudioItem has been created.
		/// </summary>
		public string Name { get { return name; } }
		/// <summary>
		/// The hashcode of the AudioSettingsBase or method from which the AudioItem has been created.
		/// </summary>
		public int Id { get { return id; } }
		/// <summary>
		/// The current state of the AudioItem.
		/// </summary>
		public AudioStates State { get { return state; } }
		/// <summary>
		/// The shared module that spatializes the AudioItem.
		/// </summary>
		public AudioSpatializer Spatializer { get { return spatializer; } }
		/// <summary>
		/// Is the AudioItem actually emitting sound (takes into account scheduled sounds)?
		/// </summary>
		public bool IsPlaying { get { return (state == AudioStates.Playing || state == AudioStates.Stopping) && (scheduledTime <= 0d || scheduleStarted); } }
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
			stopImmediate = StopImmediate;
			getDeltaTime = GetDeltaTime;
			setVolumeRampModifier = value => volumeModifier.RampModifier = value;
			setVolumeParentModifier = value => volumeModifier.ParentModifier = value;
			setPitchRampModifier = value => pitchModifier.RampModifier = value;
			setPitchParentModifier = value => pitchModifier.ParentModifier = value;
			setVolumeFadeModifier = value => volumeModifier.FadeModifier = value;
		}

		protected virtual void Initialize(int id, string name, AudioSpatializer spatializer, AudioItem parent)
		{
			this.id = id;
			this.name = name;
			this.spatializer = spatializer;
			this.parent = parent;

			if (this.parent == null)
				AudioManager.Instance.ItemManager.Activate(this);

			SetState(AudioStates.Waiting);
		}

		protected virtual void SetState(AudioStates state)
		{
			hasBreak |= state == AudioStates.Stopping || state == AudioStates.Stopped;

			RaiseStateChangeEvent(this.state, (this.state = state));
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
			if (hasFaded)
				return;

			hasFaded = true;
			fadeTweener.Ramp(0f, 1f, Settings.FadeIn, setVolumeFadeModifier, ease: Settings.FadeInEase, getDeltaTime: getDeltaTime);
		}

		protected virtual void FadeOut()
		{
			fadeTweener.Ramp(volumeModifier.FadeModifier, 0f, Settings.FadeOut, setVolumeFadeModifier, ease: Settings.FadeOutEase, getDeltaTime: getDeltaTime, endCallback: stopImmediate);
		}

		protected virtual float GetDeltaTime()
		{
			return Application.isPlaying ? Time.unscaledDeltaTime : 0.01f;
		}

		protected virtual void Spatialize()
		{
			if (parent == null)
				spatializer.Spatialize();
		}

		protected virtual void UpdateOptions()
		{
			for (int i = 0; i < delayedOptions.Count; i++)
			{
				AudioDelayedOption delayedOption = delayedOptions[i];

				if (delayedOption.Update())
				{
					ApplyOptionNow(delayedOption.Option, false);
					delayedOptions.RemoveAt(i--);
					TypePoolManager.Recycle(ref delayedOption);
				}
			}
		}

		protected void UpdateTweeners()
		{
			if (state == AudioStates.Stopped)
				return;

			rampVolumeTweener.Update();
			rampParentVolumeTweener.Update();
			rampPitchTweener.Update();
			rampParentPitchTweener.Update();
			fadeTweener.Update();
		}

		protected void UpdateRTPCs()
		{
			if (state == AudioStates.Stopped)
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

			volumeModifier.RTPCModifier = volumeRtpc;
			pitchModifier.RTPCModifier = pitchRtpc;
		}

		/// <summary>
		/// Used internally to update the AudioItem and its hierarchy.
		/// </summary>
		public virtual void Update()
		{
			if (state == AudioStates.Stopped)
				return;

			if (scheduledTime > 0d)
				scheduleStarted |= scheduledTime <= AudioSettings.dspTime;

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
			var delayedOption = TypePoolManager.Create<AudioDelayedOption>();
			delayedOption.Initialize(option, recycle, getDeltaTime);
			delayedOptions.Add(delayedOption);
		}
		protected abstract void ApplyOptionNow(AudioOption option, bool recycle);
		/// <summary>
		/// </summary>
		/// <returns> The dsp time at which the AudioItem has be scheduled or 0. </returns>
		public virtual double GetScheduledTime() { return scheduledTime; }
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
		public virtual AudioItem GetParent() { return parent; }
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
			if (state == AudioStates.Stopped)
				return 0f;

			return volumeModifier.Value;
		}
		protected virtual void SetVolumeScale(float volume, float time, Tweening.Ease ease, bool fromSelf)
		{
			if (state == AudioStates.Stopped)
				return;

			if (fromSelf)
			{
				rampVolumeTweener.Stop();

				if (time > 0f)
					rampVolumeTweener.Ramp(volumeModifier.RampModifier, volume, time, setVolumeRampModifier, ease, getDeltaTime);
				else
					volumeModifier.RampModifier = volume;
			}
			else
			{
				rampParentVolumeTweener.Stop();

				if (time > 0f)
					rampParentVolumeTweener.Ramp(volumeModifier.ParentModifier, volume, time, setVolumeParentModifier, ease, getDeltaTime);
				else
					volumeModifier.ParentModifier = volume;
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
			if (state == AudioStates.Stopped)
				return 0f;

			return pitchModifier.Value;
		}
		protected virtual void SetPitchScale(float pitch, float time, Tweening.Ease ease, bool fromSelf)
		{
			if (state == AudioStates.Stopped)
				return;

			if (fromSelf)
			{
				rampPitchTweener.Stop();

				if (time > 0f)
					rampPitchTweener.Ramp(pitchModifier.RampModifier, pitch, time, setPitchRampModifier, ease, getDeltaTime);
				else
					pitchModifier.RampModifier = pitch;
			}
			else
			{
				rampParentPitchTweener.Stop();

				if (time > 0f)
					rampParentPitchTweener.Ramp(pitchModifier.ParentModifier, pitch, time, setPitchParentModifier, ease, getDeltaTime);
				else
					pitchModifier.ParentModifier = pitch;
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
			volumeModifier = TypePoolManager.Create<AudioModifier>();
			pitchModifier = TypePoolManager.Create<AudioModifier>();
			fadeTweener = TypePoolManager.Create<FloatTweener>();
			rampVolumeTweener = TypePoolManager.Create<FloatTweener>();
			rampParentVolumeTweener = TypePoolManager.Create<FloatTweener>();
			rampPitchTweener = TypePoolManager.Create<FloatTweener>();
			rampParentPitchTweener = TypePoolManager.Create<FloatTweener>();
		}

		/// <summary>
		/// Internaly used by the pooling system.
		/// </summary>
		public virtual void OnRecycle()
		{
			TypePoolManager.Recycle(ref volumeModifier);
			TypePoolManager.Recycle(ref pitchModifier);
			TypePoolManager.Recycle(ref fadeTweener);
			TypePoolManager.Recycle(ref rampVolumeTweener);
			TypePoolManager.Recycle(ref rampParentVolumeTweener);
			TypePoolManager.Recycle(ref rampPitchTweener);
			TypePoolManager.Recycle(ref rampParentPitchTweener);

			// Only the AudioItem root should recycle the spatializer as it is shared with it's children
			if (parent == null)
				TypePoolManager.Recycle(ref spatializer);

			TypePoolManager.RecycleElements(delayedOptions);
			ClearEvents();
		}

		/// <summary>
		/// Copies another AudioItem.
		/// </summary>
		/// <param name="castedReference"> The AudioItem to copy. </param>
		public virtual void Copy(object reference)
		{
			var castedReference = (AudioItem)reference;
			id = castedReference.id;
			name = castedReference.name;
			state = castedReference.state;
			spatializer = castedReference.spatializer;
			parent = castedReference.parent;
			scheduledTime = castedReference.scheduledTime;
			scheduleStarted = castedReference.scheduleStarted;
			volumeModifier.Copy(castedReference.volumeModifier);
			pitchModifier.Copy(castedReference.pitchModifier);
			rampVolumeTweener.Copy(castedReference.rampVolumeTweener);
			rampParentVolumeTweener.Copy(castedReference.rampParentVolumeTweener);
			rampPitchTweener.Copy(castedReference.rampPitchTweener);
			rampParentPitchTweener.Copy(castedReference.rampParentPitchTweener);
			fadeTweener.Copy(castedReference.fadeTweener);
			pausedState = castedReference.pausedState;
			hasFaded = castedReference.hasFaded;
			hasBreak = castedReference.hasBreak;
			TypePoolManager.CreateCopies(delayedOptions, castedReference.delayedOptions);
			OnPlay = castedReference.OnPlay;
			OnPause = castedReference.OnPause;
			OnResume = castedReference.OnResume;
			OnStopping = castedReference.OnStopping;
			OnStop = castedReference.OnStop;
			OnUpdate = castedReference.OnUpdate;
			OnStateChanged = castedReference.OnStateChanged;
		}

		public override string ToString()
		{
			return string.Format("{0}({1}, {2})", GetType(), Name, state);
		}
	}
}
