using System;
using System.Collections.Generic;

namespace Pseudo.Groupingz
{
	public interface IGroup<TElement> : IEnumerable<TElement>
	{
		event Action<TElement> OnAdded;
		event Action<TElement> OnRemoved;

		IMatcher Matcher { get; }
		int Count { get; }

		TElement this[int index] { get; }

		bool Update(IElementInfo<TElement> info);
		bool Contains(TElement element);
		int IndexOf(TElement element);
		TElement Find(Predicate<TElement> match);
		int FindIndex(Predicate<TElement> match);
		TElement[] ToArray();
		void CopyTo(TElement[] array, int index = 0);
	}
}