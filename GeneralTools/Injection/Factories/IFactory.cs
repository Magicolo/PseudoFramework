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
		object Create(params object[] arguments);
	}

	public interface IFactory<TResult> : IFactory
	{
		TResult Create();
	}

	public interface IFactory<TResult, TArg> : IFactory
	{
		TResult Create(TArg argument);
	}
}
