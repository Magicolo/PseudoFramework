using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using UnityEngine.Audio;

public class AudioOptionDrawerDummy : ScriptableObject
{
    [Min]
    public float EaseTime = 0f;
    public TweenManager.Ease EaseType = TweenManager.Ease.Linear;
    [Range(0f, 5f)]
    public float VolumeScale = 1f;
    [Range(0f, 5f)]
    public float PitchScale = 1f;
    [Range(0f, 1f)]
    public float RandomVolume = 0f;
    [Range(0f, 1f)]
    public float RandomPitch = 0f;
    [Min]
    public float FadeIn = 0f;
    [Min]
    public float FadeOut = 0f;
    public bool Loop = false;
    public AudioClip Clip = null;
    public AudioMixerGroup Output = null;
    [Range(0f, 5f)]
    public float DopplerLevel = 1f;
    public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;
    [Clamp]
    public AnimationCurve RolloffModeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [Min]
    public float MinDistance = 1f;
    [Min]
    public float MaxDistance = 500f;
    [Range(0f, 360f)]
    public float Spread = 0f;
    [Clamp]
    public AnimationCurve SpreadCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public bool Mute = false;
    public bool BypassEffects = false;
    public bool BypassListenerEffects = false;
    public bool BypassReverbZones = false;
    [Range(0, 256)]
    public int Priority = 128;
    [Range(-1f, 1f)]
    public float StereoPan = 0f;
    [Range(0f, 1f)]
    public float SpatialBlend = 0f;
    [Clamp]
    public AnimationCurve SpatialBlendCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [Range(0f, 1.1f)]
    public float ReverbZoneMix = 1f;
    [Clamp]
    public AnimationCurve ReverbZoneMixCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [MinMaxSlider(0f, 1f)]
    public Vector2 PlayRange = Vector2.up;
    [Min]
    public float Time;
    [Min]
    public int TimeSamples;
    public AudioVelocityUpdateMode VelocityUpdateMode;
    public bool IgnoreListenerPause = false;
    public bool IgnoreListenerVolume = false;
    public bool Spatialize = false;
}
