using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using UnityEngine.Audio;

namespace Pseudo.Internal.Audio
{
	public class AudioDelayedOption : IPoolable, ICopyable<AudioDelayedOption>
	{
		AudioOption option;
		bool recycle;
		Func<float> getDeltaTime;
		float delayCounter;

		public AudioOption Option { get { return option; } }
		public bool Recycle { get { return recycle; } }

		public static AudioDelayedOption Default = new AudioDelayedOption();

		public void Initialize(AudioOption option, bool recycle, Func<float> getDeltaTime)
		{
			this.option = option;
			this.recycle = recycle;
			this.getDeltaTime = getDeltaTime;
		}

		public bool Update()
		{
			delayCounter += getDeltaTime();

			return delayCounter >= option.Delay;
		}

		public void OnCreate()
		{
		}

		public void OnRecycle()
		{
		}

		public void Copy(AudioDelayedOption reference)
		{
			option = reference.option;
			recycle = reference.recycle;
			getDeltaTime = reference.getDeltaTime;
			delayCounter = reference.delayCounter;
		}
	}
}