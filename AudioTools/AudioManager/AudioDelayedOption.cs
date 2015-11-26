using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using UnityEngine.Audio;

namespace Pseudo.Internal.Audio
{
	public class AudioDelayedOption : IPoolable, ICopyable
	{
		AudioOption option;
		bool recycle;
		Func<float> getDeltaTime;
		float delayCounter;

		public AudioOption Option { get { return option; } }
		public bool Recycle { get { return recycle; } }

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

		public void OnCreate() { }

		public void OnRecycle()
		{
			if (recycle)
				TypePoolManager.Recycle(option);
		}

		public void Copy(object reference)
		{
			var castedReference = (AudioDelayedOption)reference;
			option = castedReference.option;
			recycle = castedReference.recycle;
			getDeltaTime = castedReference.getDeltaTime;
			delayCounter = castedReference.delayCounter;
		}
	}
}