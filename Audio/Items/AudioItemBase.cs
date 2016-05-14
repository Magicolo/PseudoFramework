using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using Pseudo.Audio.Internal;
using Pseudo.Internal;
using Pseudo.Pooling;

namespace Pseudo.Audio.Internal
{
	[Serializable]
	public abstract class AudioItemBase : IAudioItem, IPoolable, ICopyable<AudioItemBase>
	{
		protected int identifier;
		protected AudioItemManager itemManager;
		protected AudioStates state;
		protected AudioSpatializer spatializer;
		protected IAudioItem parent;
		protected double scheduledTime;
		protected bool scheduleStarted;
		protected AudioStates pausedState;
		protected bool hasFaded;
		protected bool hasBreak;

		protected readonly AudioModifier volumeModifier = new AudioModifier();
		protected readonly AudioModifier pitchModifier = new AudioModifier();
		protected readonly FloatTweener rampVolumeTweener = new FloatTweener();
		protected readonly FloatTweener rampParentVolumeTweener = new FloatTweener();
		protected readonly FloatTweener rampPitchTweener = new FloatTweener();
		protected readonly FloatTweener rampParentPitchTweener = new FloatTweener();
		protected readonly FloatTweener fadeTweener = new FloatTweener();
		readonly List<AudioDelayedOption> delayedOptions = new List<AudioDelayedOption>();
		readonly Action stopImmediate;
		readonly Func<float> getDeltaTime;
		readonly Action<float> setVolumeRampModifier;
		readonly Action<float> setVolumeParentModifier;
		readonly Action<float> setPitchRampModifier;
		readonly Action<float> setPitchParentModifier;
		readonly Action<float> setVolumeFadeModifier;

		/// <summary>
		/// The hashcode of the AudioSettingsBase or method from which the AudioItem has been created.
		/// </summary>
		public int Identifier { get { return identifier; } }
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
		public event Action<IAudioItem> OnPlay;
		/// <summary>
		/// An event triggered when the Pause() method is successfuly called.
		/// This event will be cleared automaticaly when the AudioItem is recycled.
		/// </summary>
		public event Action<IAudioItem> OnPause;
		/// <summary>
		/// An event triggered when the Resume() method is successfuly called.
		/// This event will be cleared automaticaly when the AudioItem is recycled.
		/// </summary>
		public event Action<IAudioItem> OnResume;
		/// <summary>
		/// An event triggered when the AudioItem starts fading out.
		/// This event will be cleared automaticaly when the AudioItem is recycled.
		/// </summary>
		public event Action<IAudioItem> OnStopping;
		/// <summary>
		/// An event triggered when the AudioItem stops. After this event has been triggered, the AudioItem becomes obsolete.
		/// This event will be cleared automaticaly when the AudioItem is recycled.
		/// </summary>
		public event Action<IAudioItem> OnStop;
		/// <summary>
		/// An event triggered on each Update(). Use this to insert dynamic logic into the AudioItem.
		/// This event will be cleared automaticaly when the AudioItem is recycled.
		/// </summary>
		public event Action<IAudioItem> OnUpdate;
		/// <summary>
		/// An event triggered when the AudioItem changes its state.
		/// This event will be cleared automaticaly when the AudioItem is recycled.
		/// </summary>
		public event Action<IAudioItem, AudioStates, AudioStates> OnStateChanged;

		protected AudioItemBase()
		{
			stopImmediate = StopImmediate;
			getDeltaTime = GetDeltaTime;
			setVolumeRampModifier = value => volumeModifier.RampModifier = value;
			setVolumeParentModifier = value => volumeModifier.ParentModifier = value;
			setPitchRampModifier = value => pitchModifier.RampModifier = value;
			setPitchParentModifier = value => pitchModifier.ParentModifier = value;
			setVolumeFadeModifier = value => volumeModifier.FadeModifier = value;
		}

		protected void Initialize(int identifier, AudioItemManager itemManager, AudioSpatializer spatializer, IAudioItem parent)
		{
			this.identifier = identifier;
			this.itemManager = itemManager;
			this.spatializer = spatializer;
			this.parent = parent;

			if (this.parent == null)
				itemManager.Activate(this);

			SetState(AudioStates.Waiting);
		}

		protected void SetState(AudioStates state)
		{
			hasBreak |= state == AudioStates.Stopping || state == AudioStates.Stopped;

			RaiseStateChangeEvent(this.state, (this.state = state));
		}

		protected void RaisePlayEvent()
		{
			if (OnPlay != null)
				OnPlay(this);
		}

		protected void RaisePauseEvent()
		{
			if (OnPause != null)
				OnPause(this);
		}

		protected void RaiseResumeEvent()
		{
			if (OnResume != null)
				OnResume(this);
		}

		protected void RaiseStoppingEvent()
		{
			if (OnStopping != null)
				OnStopping(this);
		}

		protected void RaiseStopEvent()
		{
			if (OnStop != null)
				OnStop(this);
		}

		protected void RaiseUpdateEvent()
		{
			if (OnUpdate != null)
				OnUpdate(this);
		}

		protected void RaiseStateChangeEvent(AudioStates oldState, AudioStates newState)
		{
			if (OnStateChanged != null)
				OnStateChanged(this, oldState, newState);
		}

		protected void FadeIn()
		{
			if (hasFaded)
				return;

			hasFaded = true;
			fadeTweener.Ramp(0f, 1f, Settings.FadeIn, setVolumeFadeModifier, ease: Settings.FadeInEase, getDeltaTime: getDeltaTime);
		}

		protected void FadeOut()
		{
			fadeTweener.Ramp(volumeModifier.FadeModifier, 0f, Settings.FadeOut, setVolumeFadeModifier, ease: Settings.FadeOutEase, getDeltaTime: getDeltaTime, endCallback: stopImmediate);
		}

		protected virtual float GetDeltaTime()
		{
			return Application.isPlaying ? UnityEngine.Time.unscaledDeltaTime : 0.01f;
		}

		protected void Spatialize()
		{
			if (parent == null)
				spatializer.Spatialize();
		}

		protected void UpdateOptions()
		{
			for (int i = 0; i < delayedOptions.Count; i++)
			{
				var delayedOption = delayedOptions[i];

				if (delayedOption.Update())
				{
					ApplyOptionNow(delayedOption.Option, false);
					delayedOptions.RemoveAt(i--);
					//TypePoolManager.Recycle(ref delayedOption);
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
				var rtpc = Settings.RTPCs[i];

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
		public void ApplyOption(AudioOption option, bool recycle = true)
		{
			if (option.Delay > 0f)
				ApplyOptionDelayed(option, recycle);
			else
				ApplyOptionNow(option, recycle);
		}
		protected void ApplyOptionDelayed(AudioOption option, bool recycle)
		{
			//var delayedOption = TypePoolManager.Create<AudioDelayedOption>();
			var delayedOption = new AudioDelayedOption();
			delayedOption.Initialize(option, recycle, getDeltaTime);
			delayedOptions.Add(delayedOption);
		}
		protected abstract void ApplyOptionNow(AudioOption option, bool recycle);
		/// <summary>
		/// </summary>
		/// <returns> The dsp time at which the AudioItem has be scheduled or 0. </returns>
		public double GetScheduledTime() { return scheduledTime; }
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
		/// Sets all the AudioItem's events to null.
		/// </summary>
		public void ClearEvents()
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
		public float GetVolumeScale()
		{
			if (state == AudioStates.Stopped)
				return 0f;

			return volumeModifier.Value;
		}
		protected void SetVolumeScale(float volume, float time, TweenUtility.Ease ease, bool fromSelf)
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
		public void SetVolumeScale(float volume, float time, TweenUtility.Ease ease = TweenUtility.Ease.Linear) { SetVolumeScale(volume, time, ease, false); }
		/// <summary>
		/// Sets the volume scale of the AudioItem and its hierarchy.
		/// Other volume modifiers such as fades remain unaffected.
		/// </summary>
		/// <param name="volume"> The target volume at which the AudioItem should be set to. </param>
		public void SetVolumeScale(float volume) { SetVolumeScale(volume, 0f, TweenUtility.Ease.Linear, false); }

		/// <summary>
		/// </summary>
		/// <returns> The pitch scale of the AudioItem. </returns>
		public float GetPitchScale()
		{
			if (state == AudioStates.Stopped)
				return 0f;

			return pitchModifier.Value;
		}
		protected void SetPitchScale(float pitch, float time, TweenUtility.Ease ease, bool fromSelf)
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
		public void SetPitchScale(float pitch, float time, TweenUtility.Ease ease = TweenUtility.Ease.Linear) { SetPitchScale(pitch, time, ease, false); }
		/// <summary>
		/// Sets the pitch scale of the AudioItem and its hierarchy.
		/// Other pitch modifiers such as fades remain unaffected.
		/// </summary>
		/// <param name="pitch"> The target pitch at which the AudioItem should be set to. </param>
		public void SetPitchScale(float pitch) { SetPitchScale(pitch, 0f, TweenUtility.Ease.Linear, false); }

		/// <summary>
		/// Internaly used by the pooling system.
		/// </summary>
		public virtual void OnCreate() { }

		/// <summary>
		/// Internaly used by the pooling system.
		/// </summary>
		public virtual void OnRecycle()
		{
			// Only the AudioItem root should recycle the spatializer as it is shared with it's children
			//if (parent == null)
			//	TypePoolManager.Recycle(ref spatializer);

			//TypePoolManager.RecycleElements(delayedOptions);
			ClearEvents();
		}

		/// <summary>
		/// Copies another AudioItem.
		/// </summary>
		/// <param name="source"> The AudioItem to copy. </param>
		public void Copy(AudioItemBase source)
		{
			identifier = source.identifier;
			itemManager = source.itemManager;
			state = source.state;
			spatializer = source.spatializer;
			parent = source.parent;
			scheduledTime = source.scheduledTime;
			scheduleStarted = source.scheduleStarted;
			volumeModifier.Copy(source.volumeModifier);
			pitchModifier.Copy(source.pitchModifier);
			rampVolumeTweener.Copy(source.rampVolumeTweener);
			rampParentVolumeTweener.Copy(source.rampParentVolumeTweener);
			rampPitchTweener.Copy(source.rampPitchTweener);
			rampParentPitchTweener.Copy(source.rampParentPitchTweener);
			fadeTweener.Copy(source.fadeTweener);
			pausedState = source.pausedState;
			hasFaded = source.hasFaded;
			hasBreak = source.hasBreak;
			Copier<AudioDelayedOption>.Default.CopyTo(source.delayedOptions, delayedOptions);
			OnPlay = source.OnPlay;
			OnPause = source.OnPause;
			OnResume = source.OnResume;
			OnStopping = source.OnStopping;
			OnStop = source.OnStop;
			OnUpdate = source.OnUpdate;
			OnStateChanged = source.OnStateChanged;
		}

		public void CopyTo(AudioItemBase target)
		{
			target.Copy(this);
		}

		public override string ToString()
		{
			return string.Format("{0}({1}, {2})", GetType(), identifier, state);
		}
	}
}
