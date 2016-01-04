using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

namespace Pseudo
{
	public class FloatTweener : IPoolable, ICopyable
	{
		public enum TweenStates
		{
			Waiting,
			Playing,
			Stopped
		}

		float start;
		float end;
		float time;
		Action<float> setValue;
		Func<float, float> easeFunction;
		Func<float> getDeltaTime;
		float delay;
		Action startCallback;
		Action endCallback;
		TweenStates state = TweenStates.Stopped;
		float value;
		float completion;
		float counter;

		public TweenStates State { get { return state; } }
		public float Value { get { return value; } }
		public float Completion { get { return completion; } }

		public void Update()
		{
			switch (state)
			{
				case TweenStates.Waiting:
					delay -= getDeltaTime();

					if (delay <= 0f)
					{
						SetState(TweenStates.Playing);
						Update();
					}
					break;
				case TweenStates.Playing:
					completion = Mathf.Clamp01(counter / time);
					value = (end - start) * easeFunction(completion) + start;
					setValue(value);
					counter += getDeltaTime();

					if (counter >= time)
						SetState(TweenStates.Stopped);
					break;
			}
		}

		public void Stop()
		{
			if (state == TweenStates.Stopped)
				return;

			SetState(TweenStates.Stopped);
		}

		public void Ramp(float start, float end, float time, Action<float> setValue, Tweening.Ease ease = Tweening.Ease.Linear, Func<float> getDeltaTime = null, float delay = 0f, Action startCallback = null, Action endCallback = null)
		{
			this.start = start;
			this.end = end;
			this.time = time;
			this.setValue = setValue ?? Tweening.EmptyFloatAction;
			this.easeFunction = Tweening.ToEaseFunction(ease);
			this.getDeltaTime = getDeltaTime ?? (ApplicationUtility.IsPlaying ? Tweening.DefaultGetDeltaTime : Tweening.DefaultEditorGetDeltaTime);
			this.delay = delay;
			this.startCallback = startCallback ?? Tweening.EmptyAction;
			this.endCallback = endCallback ?? Tweening.EmptyAction;

			SetState(TweenStates.Waiting);
			Update();
		}

		void SetState(TweenStates state)
		{
			this.state = state;

			switch (this.state)
			{
				case TweenStates.Waiting:
					completion = 0f;
					counter = 0f;
					break;
				case TweenStates.Playing:
					value = start;
					startCallback();
					break;
				case TweenStates.Stopped:
					completion = 1f;
					counter = time;
					value = end;
					endCallback();
					break;
			}
		}

		public virtual void OnCreate()
		{
		}

		public virtual void OnRecycle()
		{
		}

		public void Copy(object reference)
		{
			var castedReference = (FloatTweener)reference;
			start = castedReference.start;
			end = castedReference.end;
			time = castedReference.time;
			setValue = castedReference.setValue;
			easeFunction = castedReference.easeFunction;
			getDeltaTime = castedReference.getDeltaTime;
			delay = castedReference.delay;
			startCallback = castedReference.startCallback;
			endCallback = castedReference.endCallback;
			state = castedReference.state;
			value = castedReference.value;
			completion = castedReference.completion;
			counter = castedReference.counter;
		}
	}
}