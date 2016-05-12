using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2
{
	public interface IInitializer<T> : IInitializer where T : class
	{
		void OnCreate(T instance);
		void OnRecycle(T instance);
	}

	public interface IInitializer
	{
		void OnCreate(object instance);
		void OnRecycle(object instance);
	}
}
