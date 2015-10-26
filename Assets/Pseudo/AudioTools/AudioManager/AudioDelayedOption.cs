using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using UnityEngine.Audio;

namespace Pseudo
{
	public class AudioDelayedOption : IPoolable, ICopyable<AudioDelayedOption>
	{
		AudioOption _option;
		bool _recycle;
		Func<float> _getDeltaTime;
		float _delayCounter;

		public AudioOption Option { get { return _option; } }
		public bool Recycle { get { return _recycle; } }

		public static AudioDelayedOption Default = new AudioDelayedOption();

		public void Initialize(AudioOption option, bool recycle, Func<float> getDeltaTime)
		{
			_option = option;
			_recycle = recycle;
			_getDeltaTime = getDeltaTime;
		}

		public bool Update()
		{
			_delayCounter += _getDeltaTime();

			return _delayCounter >= _option.Delay;
		}

		public void OnCreate()
		{
		}

		public void OnRecycle()
		{
		}

		public void Copy(AudioDelayedOption reference)
		{
			_option = reference._option;
			_recycle = reference._recycle;
			_getDeltaTime = reference._getDeltaTime;
			_delayCounter = reference._delayCounter;
		}
	}
}