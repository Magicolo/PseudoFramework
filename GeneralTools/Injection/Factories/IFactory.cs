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
		object Create();
	}

	public interface IFactory<out TResult>
	{
		TResult Create();
	}

	public interface IFactory<in TArg, out TResult>
	{
		TResult Create(TArg argument);
	}

	public interface IFactory<in TArg1, in TArg2, out TResult>
	{
		TResult Create(TArg1 argument, TArg2 argument2);
	}

	public interface IFactory<in TArg1, in TArg2, in TArg3, out TResult>
	{
		TResult Create(TArg1 argument, TArg2 argument2, TArg3 argument3);
	}
}
