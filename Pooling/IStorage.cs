using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling
{
	public interface IStorage<T> : IStorage where T : class
	{
		new T Take();
		bool Put(T instance);
		bool Contains(T instance);
	}

	public interface IStorage
	{
		int Count { get; }
		int Capacity { get; set; }

		object Take();
		bool Put(object instance);
		void Fill(int count);
		void Trim(int count);
		bool Contains(object instance);
		void Clear();
	}
}
