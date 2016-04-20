using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Reflection
{
	public delegate void Invoker<TTarget>(ref TTarget target);
	public delegate void InvokerIn<TTarget, in TIn>(ref TTarget target, TIn argument);
	public delegate void InvokerIn<TTarget, in TIn1, in TIn2>(ref TTarget target, TIn1 argument1, TIn2 argument2);
	public delegate void InvokerIn<TTarget, in TIn1, in TIn2, in TIn3>(ref TTarget target, TIn1 argument1, TIn2 argument2, TIn3 argument);
	public delegate TOut InvokerOut<TTarget, out TOut>(ref TTarget target);
	public delegate TOut InvokerInOut<TTarget, in TIn, out TOut>(ref TTarget target, TIn argument);
	public delegate TOut InvokerInOut<TTarget, in TIn1, in TIn2, out TOut>(ref TTarget target, TIn1 argument1, TIn2 argument2);
	public delegate TOut InvokerInOut<TTarget, in TIn1, in TIn2, in TIn3, out TOut>(ref TTarget target, TIn1 argument1, TIn2 argument2, TIn3 argument);

	public interface IMethodWrapper
	{
		object Invoke(ref object target);
		object Invoke(ref object target, params object[] arguments);
	}
}
