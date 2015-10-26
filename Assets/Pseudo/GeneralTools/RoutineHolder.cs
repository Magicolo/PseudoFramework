using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

public class RoutineHolder : IPoolable, ICopyable<RoutineHolder>
{
	readonly List<IEnumerator> _routines = new List<IEnumerator>();

	public static readonly RoutineHolder Default = new RoutineHolder();

	public void Update()
	{
		for (int i = 0; i < _routines.Count; i++)
		{
			if (!_routines[i].MoveNext())
				_routines.RemoveAt(i--);
		}
	}

	public void StartRoutine(IEnumerator routine)
	{
		_routines.Add(routine);
	}

	public void StopRoutine(IEnumerator routine)
	{
		_routines.Remove(routine);
	}

	public void StopAllRoutines()
	{
		_routines.Clear();
	}

	public void InvokeDelayed(Action action, float delay, Func<float> getDeltaTime = null)
	{
		if (delay > 0f)
			_routines.Add(TweenManager.GetWaitRoutine(delay, getDeltaTime, endCallback: action));
		else
			action();
	}

	public void OnCreate()
	{
	}

	public void OnRecycle()
	{
		_routines.Clear();
	}

	public void Copy(RoutineHolder reference)
	{
	}
}
