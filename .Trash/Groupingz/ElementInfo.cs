using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Groupingz.Internal
{
	public class ElementInfo<TElement> : IElementInfo<TElement>
	{
		public TElement Element
		{
			get { return element; }
		}
		public IList<int> Identifiers
		{
			get { return identifiers; }
		}

		readonly List<int> identifiers = new List<int>();

		readonly TElement element;

		public ElementInfo(TElement element)
		{
			this.element = element;
		}

		public void Add(int identifier)
		{
			int index = identifiers.BinarySearch(identifier);
			index = index >= 0 ? index : ~index;

			Identifiers.Insert(index, identifier);
		}

		public bool Remove(int identifier)
		{
			int index = identifiers.BinarySearch(identifier);

			if (index >= 0)
			{
				Identifiers.RemoveAt(index);
				return true;
			}
			else
				return false;
		}
	}
}
