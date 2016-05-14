using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface IFactory
	{
		Type Type { get; }

		object Create(params object[] arguments);
	}

	public interface IFactory<out TTarget> : IFactory
	{
		TTarget Create();
	}

	public interface IFactory<in TArg, out TTarget> : IFactory
	{
		TTarget Create(TArg argument);
	}

	public interface IFactory<in TArg1, in TArg2, out TTarget> : IFactory
	{
		TTarget Create(TArg1 argument1, TArg2 argument2);
	}

	public interface IFactory<in TArg1, in TArg2, in TArg3, out TTarget> : IFactory
	{
		TTarget Create(TArg1 argument1, TArg2 argument2, TArg3 argument3);
	}
}
