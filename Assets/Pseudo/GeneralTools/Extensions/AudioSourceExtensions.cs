using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class AudioSourceExtensions
	{
		public static AudioSource PlayOnListener(this AudioSource audioSource)
		{
			if (audioSource == null || audioSource.clip == null)
				return null;

			AudioSource source = audioSource.clip.PlayOnListener();
			source.Copy(audioSource);
			source.Play();

			return source;
		}

		public static float RemainingTime(this AudioSource audioSource)
		{
			if (!audioSource.isPlaying)
				return 0f;

			if (audioSource.pitch >= 0f)
				return (audioSource.clip.length - audioSource.time) / audioSource.pitch;
			else
				return audioSource.time / audioSource.pitch;
		}

		public static void Copy(this AudioSource target, AudioSource source, bool useCustomCurves = true)
		{
			target.enabled = source.enabled;
			target.clip = source.clip;
			target.mute = source.mute;
			target.bypassEffects = source.bypassEffects;
			target.bypassListenerEffects = source.bypassListenerEffects;
			target.bypassReverbZones = source.bypassReverbZones;
			target.playOnAwake = source.playOnAwake;
			target.loop = source.loop;
			target.priority = source.priority;
			target.volume = source.volume;
			target.pitch = source.pitch;
			target.dopplerLevel = source.dopplerLevel;
			target.rolloffMode = source.rolloffMode;
			target.minDistance = source.minDistance;
			target.spatialBlend = source.spatialBlend;
			target.spread = source.spread;
			target.maxDistance = source.maxDistance;
			target.panStereo = source.panStereo;
			target.time = source.time;
			target.timeSamples = source.timeSamples;
			target.velocityUpdateMode = source.velocityUpdateMode;
			target.ignoreListenerPause = source.ignoreListenerPause;
			target.ignoreListenerVolume = source.ignoreListenerVolume;
			target.spatialize = source.spatialize;

			if (useCustomCurves)
			{
				target.SetCustomCurve(AudioSourceCurveType.CustomRolloff, source.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
				target.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, source.GetCustomCurve(AudioSourceCurveType.ReverbZoneMix));
				target.SetCustomCurve(AudioSourceCurveType.SpatialBlend, source.GetCustomCurve(AudioSourceCurveType.SpatialBlend));
				target.SetCustomCurve(AudioSourceCurveType.Spread, source.GetCustomCurve(AudioSourceCurveType.Spread));
			}
		}
	}
}
