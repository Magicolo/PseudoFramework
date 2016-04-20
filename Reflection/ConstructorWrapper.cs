using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Reflection.Internal
{
	public class ConstructorWrapper : ConstructorWrapperBase
	{
		readonly ConstructorInfo constructor;

		public ConstructorWrapper(ConstructorInfo constructor)
		{
			this.constructor = constructor;
		}

		public override object Construct(params object[] arguments)
		{
			return constructor.Invoke(arguments);
		}
	}

	public class ConstructorWrapper<TTarget> : ConstructorWrapperBase<Constructor<TTarget>>
	{
		public ConstructorWrapper(ConstructorInfo constructor) : base(constructor) { }

		public override object Construct(params object[] arguments)
		{
			return constructor();
		}
	}

	public class ConstructorWrapper<TTarget, TIn> : ConstructorWrapperBase<Constructor<TTarget, TIn>>
	{
		public ConstructorWrapper(ConstructorInfo constructor) : base(constructor) { }

		public override object Construct()
		{
			return Construct(default(TIn));
		}

		public override object Construct(params object[] arguments)
		{
			return constructor((TIn)arguments[0]);
		}
	}

	public class ConstructorWrapper<TTarget, TIn1, TIn2> : ConstructorWrapperBase<Constructor<TTarget, TIn1, TIn2>>
	{
		public ConstructorWrapper(ConstructorInfo constructor) : base(constructor) { }

		public override object Construct()
		{
			return Construct(default(TIn1), default(TIn2));
		}

		public override object Construct(params object[] arguments)
		{
			return constructor((TIn1)arguments[0], (TIn2)arguments[1]);
		}
	}

	public class ConstructorWrapper<TTarget, TIn1, TIn2, TIn3> : ConstructorWrapperBase<Constructor<TTarget, TIn1, TIn2, TIn3>>
	{
		public ConstructorWrapper(ConstructorInfo constructor) : base(constructor) { }

		public override object Construct()
		{
			return Construct(default(TIn1), default(TIn2), default(TIn3));
		}

		public override object Construct(params object[] arguments)
		{
			return constructor((TIn1)arguments[0], (TIn2)arguments[1], (TIn3)arguments[2]);
		}
	}
}
