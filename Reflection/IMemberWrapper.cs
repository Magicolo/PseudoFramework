using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Reflection
{
	public delegate TValue Getter<TTarget, out TValue>(ref TTarget target);
	public delegate void Setter<TTarget, in TValue>(ref TTarget target, TValue value);

	public interface IMemberWrapper
	{
		object Get(ref object target);
		void Set(ref object target, object value);
	}
}
