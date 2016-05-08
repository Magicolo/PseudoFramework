using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.PoolingNOOOO
{
	/// <summary>
	/// Creates and disposes of instances managed by an IPool instance.
	/// </summary>
	public interface IPoolingFactory
	{
		object Create();
		void Destroy(object instance);
	}

	public interface IPoolingFactory<T> : IPoolingFactory
	{
		new T Create();
		void Destroy(T instance);
	}
}
