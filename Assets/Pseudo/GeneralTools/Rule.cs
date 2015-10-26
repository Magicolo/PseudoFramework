using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;

namespace Pseudo
{
	public class Rule<T> : IPoolable, ICopyable<Rule<T>>
	{
		T _instance;
		Predicate<T> _condition;
		Action<T> _action;

		public static readonly Rule<T> Default = new Rule<T>();

		public void Update()
		{
			if (_condition(_instance))
				_action(_instance);
		}

		public void Initialize(T instance, Predicate<T> condition, Action<T> action)
		{
			_instance = instance;
			_condition = condition;
			_action = action;
		}

		public void OnCreate()
		{
		}

		public void OnRecycle()
		{
		}

		public void Copy(Rule<T> reference)
		{
			_instance = reference._instance;
			_condition = reference._condition;
			_action = reference._action;
		}
	}
}