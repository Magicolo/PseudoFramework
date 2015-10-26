using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

namespace Pseudo.Internal.Audio
{
	public class AudioModifier : IPoolable, ICopyable<AudioModifier>
	{
		float _initialValue = 1f;
		float _fadeModifier = 1f;
		float _rampModifier = 1f;
		float _parentModifier = 1f;
		float _randomModifier = 1f;
		float _rtpcModifier = 1f;

		public virtual float Value { get { return _initialValue * _rampModifier * _parentModifier * _randomModifier * _fadeModifier * _rtpcModifier; } }
		public float InitialValue { get { return _initialValue; } set { if (_initialValue != value) { _initialValue = value; RaiseValueChangedEvent(); }; } }
		public float FadeModifier { get { return _fadeModifier; } set { if (_fadeModifier != value) { _fadeModifier = value; RaiseValueChangedEvent(); }; } }
		public float RampModifier { get { return _rampModifier; } set { if (_rampModifier != value) { _rampModifier = value; RaiseValueChangedEvent(); }; } }
		public float ParentModifier { get { return _parentModifier; } set { if (_parentModifier != value) { _parentModifier = value; RaiseValueChangedEvent(); }; } }
		public float RandomModifier { get { return _randomModifier; } set { if (_randomModifier != value) { _randomModifier = value; RaiseValueChangedEvent(); }; } }
		public float RTPCModifier { get { return _rtpcModifier; } set { if (_rtpcModifier != value) { _rtpcModifier = value; RaiseValueChangedEvent(); }; } }

		public static readonly AudioModifier Default = new AudioModifier();

		public event Action<AudioModifier> OnValueChanged;

		public void SimulateChange()
		{
			RaiseValueChangedEvent();
		}

		protected virtual void RaiseValueChangedEvent()
		{
			if (OnValueChanged != null)
				OnValueChanged(this);
		}

		public virtual void OnCreate()
		{
		}

		public virtual void OnRecycle()
		{
			OnValueChanged = null;
		}

		public void Copy(AudioModifier reference)
		{
			_initialValue = reference._initialValue;
			_fadeModifier = reference._fadeModifier;
			_rampModifier = reference._rampModifier;
			_parentModifier = reference._parentModifier;
			_randomModifier = reference._randomModifier;
			_rtpcModifier = reference._rtpcModifier;
			OnValueChanged = reference.OnValueChanged;
		}
	}
}