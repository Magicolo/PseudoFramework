using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2
{
	public interface IPool<T> : IPool where T : class
	{
		new T Create();
		void Recycle(T instance);
		bool Contains(T instance);
	}

	public interface IPool
	{
		Type Type { get; }
		int Size { get; set; }

		object Create();
		void Recycle(object instance);
		bool Contains(object instance);
		void Clear();
	}
}
