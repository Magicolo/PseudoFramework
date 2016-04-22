using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Reflection
{
	public delegate TValue Getter<TTarget, out TValue>(ref TTarget target);
	public delegate void Setter<TTarget, in TValue>(ref TTarget target, TValue value);

	public delegate void Invoker<TTarget>(ref TTarget target);
	public delegate void InvokerIn<TTarget, in TIn>(ref TTarget target, TIn argument);
	public delegate void InvokerIn<TTarget, in TIn1, in TIn2>(ref TTarget target, TIn1 argument1, TIn2 argument2);
	public delegate void InvokerIn<TTarget, in TIn1, in TIn2, in TIn3>(ref TTarget target, TIn1 argument1, TIn2 argument2, TIn3 argument);
	public delegate TOut InvokerOut<TTarget, out TOut>(ref TTarget target);
	public delegate TOut InvokerInOut<TTarget, in TIn, out TOut>(ref TTarget target, TIn argument);
	public delegate TOut InvokerInOut<TTarget, in TIn1, in TIn2, out TOut>(ref TTarget target, TIn1 argument1, TIn2 argument2);
	public delegate TOut InvokerInOut<TTarget, in TIn1, in TIn2, in TIn3, out TOut>(ref TTarget target, TIn1 argument1, TIn2 argument2, TIn3 argument);

	public delegate TTarget Constructor<out TTarget>();
	public delegate TTarget Constructor<out TTarget, in TIn>(TIn argument);
	public delegate TTarget Constructor<out TTarget, in TIn1, in TIn2>(TIn1 argument1, TIn2 argument2);
	public delegate TTarget Constructor<out TTarget, in TIn1, in TIn2, in TIn3>(TIn1 argument1, TIn2 argument2, TIn3 argument3);

	public interface IMemberWrapper
	{
		string Name { get; }
		Type Type { get; }
	}

	public interface IFieldOrPropertyWrapper : IMemberWrapper
	{
		object Get(ref object target);
		void Set(ref object target, object value);
	}

	public interface IMethodWrapper : IMemberWrapper
	{
		object Invoke(ref object target);
		object Invoke(ref object target, params object[] arguments);
	}

	public interface IConstructorWrapper : IMemberWrapper
	{
		object Invoke();
		object Invoke(params object[] arguments);
	}
}
