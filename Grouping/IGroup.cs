using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Grouping
{
	public interface IGroup<T> : IEnumerable<T>
	{
		event Action<T> OnAdded;
		event Action<T> OnRemoved;

		ITemplate<T> Template { get; }
		IMatcher<T> Matcher { get; }
		int Count { get; }

		T this[int index] { get; }

		bool Update(T element);
		bool Remove(T element);
		bool Contains(T element);
	}
}
