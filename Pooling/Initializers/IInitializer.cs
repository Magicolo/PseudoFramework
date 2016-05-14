using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling
{
	public interface IInitializer<in T> : IInitializer
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
