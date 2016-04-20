using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Reflection
{
	public delegate TTarget Constructor<out TTarget>();
	public delegate TTarget Constructor<out TTarget, in TIn>(TIn argument);
	public delegate TTarget Constructor<out TTarget, in TIn1, in TIn2>(TIn1 argument1, TIn2 argument2);
	public delegate TTarget Constructor<out TTarget, in TIn1, in TIn2, in TIn3>(TIn1 argument1, TIn2 argument2, TIn3 argument3);

	public interface IConstructorWrapper
	{
		object Construct();
		object Construct(params object[] arguments);
	}
}
