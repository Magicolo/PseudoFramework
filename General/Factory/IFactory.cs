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

		object Create();
		object Create(params object[] arguments);
	}

	public interface IFactory<TTarget> : IFactory
	{
		new TTarget Create();
	}

	public interface IFactory<TArg, TTarget> : IFactory
	{
		TTarget Create(TArg argument);
	}

	public interface IFactory<TArg1, TArg2, TTarget> : IFactory
	{
		TTarget Create(TArg1 argument1, TArg2 argument2);
	}

	public interface IFactory<TArg1, TArg2, TArg3, TTarget> : IFactory
	{
		TTarget Create(TArg1 argument1, TArg2 argument2, TArg3 argument3);
	}
}
