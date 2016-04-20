using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Reflection.Internal
{
	public class MethodWrapper : MethodWrapperBase
	{
		readonly MethodInfo method;

		public MethodWrapper(MethodInfo method)
		{
			this.method = method;
		}

		public override object Invoke(ref object target, params object[] arguments)
		{
			return method.Invoke(target, arguments);
		}
	}

	public class MethodWrapper<TTarget> : MethodWrapperBase<Invoker<TTarget>>
	{
		public MethodWrapper(MethodInfo method) : base(method) { }

		public override object Invoke(ref object target, params object[] arguments)
		{
			var castedTarget = (TTarget)target;
			method(ref castedTarget);
			target = castedTarget;

			return null;
		}
	}

	public class MethodWrapperIn<TTarget, TIn> : MethodWrapperBase<InvokerIn<TTarget, TIn>>
	{
		public MethodWrapperIn(MethodInfo method) : base(method) { }

		public override object Invoke(ref object target)
		{
			return Invoke(ref target, default(TIn));
		}

		public override object Invoke(ref object target, params object[] arguments)
		{
			var castedTarget = (TTarget)target;
			method(ref castedTarget, (TIn)arguments[0]);
			target = castedTarget;

			return null;
		}
	}

	public class MethodWrapperIn<TTarget, TIn1, TIn2> : MethodWrapperBase<InvokerIn<TTarget, TIn1, TIn2>>
	{
		public MethodWrapperIn(MethodInfo method) : base(method) { }

		public override object Invoke(ref object target)
		{
			return Invoke(ref target, default(TIn1), default(TIn2));
		}

		public override object Invoke(ref object target, params object[] arguments)
		{
			var castedTarget = (TTarget)target;
			method(ref castedTarget, (TIn1)arguments[0], (TIn2)arguments[1]);
			target = castedTarget;

			return null;
		}
	}

	public class MethodWrapperIn<TTarget, TIn1, TIn2, TIn3> : MethodWrapperBase<InvokerIn<TTarget, TIn1, TIn2, TIn3>>
	{
		public MethodWrapperIn(MethodInfo method) : base(method) { }

		public override object Invoke(ref object target)
		{
			return Invoke(ref target, default(TIn1), default(TIn2), default(TIn3));
		}

		public override object Invoke(ref object target, params object[] arguments)
		{
			var castedTarget = (TTarget)target;
			method(ref castedTarget, (TIn1)arguments[0], (TIn2)arguments[1], (TIn3)arguments[2]);
			target = castedTarget;

			return null;
		}
	}

	public class MethodWrapperOut<TTarget, TOut> : MethodWrapperBase<InvokerOut<TTarget, TOut>>
	{
		public MethodWrapperOut(MethodInfo method) : base(method) { }

		public override object Invoke(ref object target, params object[] arguments)
		{
			var castedTarget = (TTarget)target;
			var result = method(ref castedTarget);
			target = castedTarget;

			return result;
		}
	}

	public class MethodWrapperInOut<TTarget, TIn, TOut> : MethodWrapperBase<InvokerInOut<TTarget, TIn, TOut>>
	{
		public MethodWrapperInOut(MethodInfo method) : base(method) { }

		public override object Invoke(ref object target)
		{
			return Invoke(ref target, default(TIn));
		}

		public override object Invoke(ref object target, params object[] arguments)
		{
			var castedTarget = (TTarget)target;
			var result = method(ref castedTarget, (TIn)arguments[0]);
			target = castedTarget;

			return result;
		}
	}

	public class MethodWrapperInOut<TTarget, TIn1, TIn2, TOut> : MethodWrapperBase<InvokerInOut<TTarget, TIn1, TIn2, TOut>>
	{
		public MethodWrapperInOut(MethodInfo method) : base(method) { }

		public override object Invoke(ref object target)
		{
			return Invoke(ref target, default(TIn1), default(TIn2));
		}

		public override object Invoke(ref object target, params object[] arguments)
		{
			var castedTarget = (TTarget)target;
			var result = method(ref castedTarget, (TIn1)arguments[0], (TIn2)arguments[1]);
			target = castedTarget;

			return result;
		}
	}

	public class MethodWrapperInOut<TTarget, TIn1, TIn2, TIn3, TOut> : MethodWrapperBase<InvokerInOut<TTarget, TIn1, TIn2, TIn3, TOut>>
	{
		public MethodWrapperInOut(MethodInfo method) : base(method) { }

		public override object Invoke(ref object target)
		{
			return Invoke(ref target, default(TIn1), default(TIn2), default(TIn3));
		}

		public override object Invoke(ref object target, params object[] arguments)
		{
			var castedTarget = (TTarget)target;
			var result = method(ref castedTarget, (TIn1)arguments[0], (TIn2)arguments[1], (TIn3)arguments[2]);
			target = castedTarget;

			return result;
		}
	}
}
