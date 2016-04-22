using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Reflection;

namespace Pseudo
{
	public abstract class FactoryBase : IFactory
	{
		public abstract Type Type { get; }

		public virtual object Create()
		{
			return Create(ReflectionUtility.EmptyArguments);
		}

		public abstract object Create(params object[] arguments);
	}

	public abstract class FactoryBase<TTarget> : IFactory<TTarget>
	{
		public Type Type
		{
			get { return typeof(TTarget); }
		}

		public abstract TTarget Create();

		object IFactory.Create()
		{
			return Create();
		}

		object IFactory.Create(params object[] arguments)
		{
			return Create();
		}
	}

	public abstract class FactoryBase<TArg, TTarget> : IFactory<TArg, TTarget>
	{
		public Type Type
		{
			get { return typeof(TTarget); }
		}

		public abstract TTarget Create(TArg argument);

		object IFactory.Create()
		{
			return Create(default(TArg));
		}

		object IFactory.Create(params object[] arguments)
		{
			return Create(arguments.Length > 0 ? (TArg)arguments[0] : default(TArg));
		}
	}

	public abstract class FactoryBase<TArg1, TArg2, TTarget> : IFactory<TArg1, TArg2, TTarget>
	{
		public Type Type
		{
			get { return typeof(TTarget); }
		}

		public abstract TTarget Create(TArg1 argument1, TArg2 argument2);

		object IFactory.Create()
		{
			return Create(default(TArg1), default(TArg2));
		}

		object IFactory.Create(params object[] arguments)
		{
			return Create(
				arguments.Length > 0 ? (TArg1)arguments[0] : default(TArg1),
				arguments.Length > 1 ? (TArg2)arguments[1] : default(TArg2));
		}
	}

	public abstract class FactoryBase<TArg1, TArg2, TArg3, TTarget> : IFactory<TArg1, TArg2, TArg3, TTarget>
	{
		public Type Type
		{
			get { return typeof(TTarget); }
		}

		public abstract TTarget Create(TArg1 argument1, TArg2 argument2, TArg3 argument3);

		object IFactory.Create()
		{
			return Create(default(TArg1), default(TArg2), default(TArg3));
		}

		object IFactory.Create(params object[] arguments)
		{
			return Create(
				arguments.Length > 0 ? (TArg1)arguments[0] : default(TArg1),
				arguments.Length > 1 ? (TArg2)arguments[1] : default(TArg2),
				arguments.Length > 2 ? (TArg3)arguments[2] : default(TArg3));
		}
	}
}
