using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2
{
	public interface IStorage<T> where T : class
	{
		int Count { get; }
		int Capacity { get; set; }

		T Take();
		bool Put(T instance);
		void Fill(int count, Func<T> factory);
		void Trim(int count);
		bool Contains(T instance);
		void Clear();
	}
}
