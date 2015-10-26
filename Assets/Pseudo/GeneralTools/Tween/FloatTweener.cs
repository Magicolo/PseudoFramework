using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

namespace Pseudo
{
	public class FloatTweener : IPoolable, ICopyable<FloatTweener>
	{
		public enum TweenStates
		{
			Waiting,
			Playing,
			Stopped
		}

		float _start;
		float _end;
		float _time;
		Action<float> _setValue;
		Func<float, float> _easeFunction;
		Func<float> _getDeltaTime;
		float _delay;
		Action _startCallback;
		Action _endCallback;
		TweenStates _state = TweenStates.Stopped;
		float _value;
		float _completion;
		float _counter;

		public TweenStates State { get { return _state; } }
		public float Value { get { return _value; } }
		public float Completion { get { return _completion; } }

		public static readonly FloatTweener Default = new FloatTweener();

		public void Update()
		{
			switch (_state)
			{
				case TweenStates.Waiting:
					_delay -= _getDeltaTime();

					if (_delay <= 0f)
					{
						SetState(TweenStates.Playing);
						Update();
					}
					break;
				case TweenStates.Playing:
					_completion = Mathf.Clamp01(_counter / _time);
					_value = (_end - _start) * _easeFunction(_completion) + _start;
					_setValue(_value);
					_counter += _getDeltaTime();

					if (_counter >= _time)
						SetState(TweenStates.Stopped);
					break;
			}
		}

		public void Stop()
		{
			if (_state == TweenStates.Stopped)
				return;

			SetState(TweenStates.Stopped);
		}

		public void Ramp(float start, float end, float time, Action<float> setValue, TweenManager.Ease ease = TweenManager.Ease.Linear, Func<float> getDeltaTime = null, float delay = 0f, Action startCallback = null, Action endCallback = null)
		{
			_start = start;
			_end = end;
			_time = time;
			_setValue = setValue ?? TweenManager.EmptyFloatAction;
			_easeFunction = TweenManager.ToEaseFunction(ease);
			_getDeltaTime = getDeltaTime ?? (Application.isPlaying ? TweenManager.DefaultGetDeltaTime : TweenManager.DefaultEditorGetDeltaTime);
			_delay = delay;
			_startCallback = startCallback ?? TweenManager.EmptyAction;
			_endCallback = endCallback ?? TweenManager.EmptyAction;

			SetState(TweenStates.Waiting);
			Update();
		}

		void SetState(TweenStates state)
		{
			_state = state;

			switch (_state)
			{
				case TweenStates.Waiting:
					_completion = 0f;
					_counter = 0f;
					break;
				case TweenStates.Playing:
					_value = _start;
					_startCallback();
					break;
				case TweenStates.Stopped:
					_completion = 1f;
					_counter = _time;
					_value = _end;
					_endCallback();
					break;
			}
		}

		public virtual void OnCreate()
		{
		}

		public virtual void OnRecycle()
		{
		}

		public void Copy(FloatTweener reference)
		{
			_start = reference._start;
			_end = reference._end;
			_time = reference._time;
			_setValue = reference._setValue;
			_easeFunction = reference._easeFunction;
			_getDeltaTime = reference._getDeltaTime;
			_delay = reference._delay;
			_startCallback = reference._startCallback;
			_endCallback = reference._endCallback;
			_state = reference._state;
			_value = reference._value;
			_completion = reference._completion;
			_counter = reference._counter;
		}
	}
}