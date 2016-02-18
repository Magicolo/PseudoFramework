using UnityEngine;
using System.Collections;
using Pseudo;
using System;
using System.Collections.Generic;
using Pseudo.Internal.Audio;

namespace Pseudo
{
	public abstract class AudioSettingsBase : ScriptableObject, INamable, IPoolable
	{
		public enum PitchScaleModes
		{
			Ratio,
			Semitone
		}

		static int idCounter;

		string cachedName;
		int identifier = ++idCounter;

		/// <summary>
		/// The unique identifier of the AudioSettingsBase.
		/// </summary>
		public int Identifier { get { return identifier; } }
		/// <summary>
		/// The name of the AudioSettingsBase.
		/// </summary>
		public string Name { get { return string.IsNullOrEmpty(cachedName) ? (cachedName = name) : cachedName; } set { cachedName = value; } }
		/// <summary>
		/// The type of the AudioSettingsBase.
		/// </summary>
		public abstract AudioTypes Type { get; }
		/// <summary>
		/// Toggles the looping behaviour of the AudioSettingsBase.
		/// </summary>
		public bool Loop;
		/// <summary>
		/// Sets the duration of the fade in of the AudioSettingsBase.
		/// </summary>
		[Min]
		public float FadeIn;
		/// <summary>
		/// Sets the fade in curve of the AudioSettingsBase.
		/// </summary>
		public TweenUtility.Ease FadeInEase = TweenUtility.Ease.InQuad;
		/// <summary>
		/// Sets the duration of the fade out of the AudioSettingsBase.
		/// </summary>
		[Min]
		public float FadeOut;
		/// <summary>
		/// Sets the fade out curve of the AudioSettingsBase.
		/// </summary>
		public TweenUtility.Ease FadeOutEase = TweenUtility.Ease.OutQuad;
		/// <summary>
		/// Sets the volume scale of the AudioSettingsBase.
		/// </summary>
		[Range(0f, 5f)]
		public float VolumeScale = 1f;
		/// <summary>
		/// Sets the way the <paramref name="PitchScale"/> is displayed in the editor.
		/// Ratio: <paramref name="PitchScale"/> is displayed normally.
		/// Semitone: <paramref name="PitchScale"/> is displayed as a semitone ratio.
		/// </summary>
		public PitchScaleModes PitchScaleMode;
		/// <summary>
		/// Sets the pitch scale of the AudioSettingsBase.
		/// </summary>
		[Range(0.0001f, 5f)]
		public float PitchScale = 1f;
		/// <summary>
		/// Sets the random volume of the AudioSettingsBase.
		/// Random volume is applied when an AudioItem is created using this AudioSettingsBase.
		/// </summary>
		[Range(0f, 1f)]
		public float RandomVolume;
		/// <summary>
		/// Sets the random pitch of the AudioSettingsBase.
		/// Random pitch is applied when an AudioItem is created using this AudioSettingsBase.
		/// </summary>
		[Range(0f, 1f)]
		public float RandomPitch;
		/// <summary>
		/// RealTime Parameter Controls that will allow to modify the volume or pitch of an AudioItem dynamicaly.
		/// </summary>
		[InitializeContent]
		public List<AudioRTPC> RTPCs = new List<AudioRTPC>();
		/// <summary>
		/// Options that will override the default settings of the AudioItem.
		/// </summary>
		[InitializeContent]
		public List<AudioOption> Options = new List<AudioOption>();

		/// <summary>
		/// Gets an AudioRTPC.
		/// </summary>
		/// <param name="name">The name of the AudioRTPC to get.</param>
		/// <returns>The AudioRTPC.</returns>
		public AudioRTPC GetRTPC(string name)
		{
			for (int i = 0; i < RTPCs.Count; i++)
			{
				var rtpc = RTPCs[i];

				if (rtpc.Name == name)
					return rtpc;
			}

			return null;
		}

		/// <summary>
		/// Internaly used by the pooling system.
		/// </summary>
		public virtual void OnCreate() { }

		/// <summary>
		/// Internaly used by the pooling system.
		/// </summary>
		public virtual void OnRecycle()
		{
			TypePoolManager.RecycleElements(RTPCs);
			TypePoolManager.RecycleElements(Options);
		}
	}
}