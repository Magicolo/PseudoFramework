using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public interface IPool<T> where T : class
	{
		T Create();
		TC CreateCopy<TC>(TC reference) where TC : class, T, ICopyable<TC>;
		void CreateElements<TC>(IList<TC> array) where TC : class, T, ICopyable<TC>;
		void Recycle(T item);
		void Recycle(ref T item);
		void RecycleElements(IList<T> array);
		bool Contains(T item);
		int Count();
		void Clear();
	}
}