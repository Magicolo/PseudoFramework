using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using UnityEngine.Audio;
using Pseudo.Pooling;

namespace Pseudo.Audio.Internal
{
	public class AudioDelayedOption : IPoolable, ICopyable<AudioDelayedOption>, ICopyable
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
			//if (recycle)
			//	TypePoolManager.Recycle(ref option);
		}

		public void Copy(AudioDelayedOption reference)
		{
			option = reference.option;
			recycle = reference.recycle;
			getDeltaTime = reference.getDeltaTime;
			delayCounter = reference.delayCounter;
		}

		public void CopyTo(AudioDelayedOption instance)
		{
			instance.Copy(this);
		}

		public void Copy(object source)
		{
			Copy((AudioDelayedOption)source);
		}
	}
}