using System;
using System.Collections.Generic;
using Pseudo.Audio.Internal;

namespace Pseudo.Audio
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

	public interface IAudioItem
	{
		event Action<IAudioItem> OnPause;
		event Action<IAudioItem> OnPlay;
		event Action<IAudioItem> OnResume;
		event Action<IAudioItem, AudioStates, AudioStates> OnStateChanged;
		event Action<IAudioItem> OnStop;
		event Action<IAudioItem> OnStopping;
		event Action<IAudioItem> OnUpdate;

		bool IsPlaying { get; }
		AudioSettingsBase Settings { get; }
		AudioSpatializer Spatializer { get; }
		AudioStates State { get; }
		AudioTypes Type { get; }

		void ApplyOption(AudioOption option, bool recycle = true);
		void Break();
		float GetPitchScale();
		double GetScheduledTime();
		float GetVolumeScale();
		void Pause();
		void Play();
		void PlayScheduled(double time);
		double RemainingTime();
		void Resume();
		void SetPitchScale(float pitch);
		void SetPitchScale(float pitch, float time, TweenUtility.Ease ease = TweenUtility.Ease.Linear);
		void SetRTPCValue(string name, float value);
		void SetScheduledTime(double time);
		void SetVolumeScale(float volume);
		void SetVolumeScale(float volume, float time, TweenUtility.Ease ease = TweenUtility.Ease.Linear);
		void Stop();
		void StopImmediate();
	}
}