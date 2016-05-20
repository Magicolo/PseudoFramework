using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Grouping.Internal
{
	public class Group<T> : IGroup<T>
	{
		public event Action<T> OnAdded;
		public event Action<T> OnRemoved;

		public ITemplate<T> Template
		{
			get { return template; }
		}
		public IMatcher<T> Matcher
		{
			get { return matcher; }
		}
		public int Count
		{
			get { return elements.Count; }
		}

		public T this[int index]
		{
			get { return elements[index]; }
		}

		protected readonly ITemplate<T> template;
		protected readonly IMatcher<T> matcher;
		protected readonly List<T> elements = new List<T>();

		public Group(ITemplate<T> template, IMatcher<T> matcher)
		{
			this.template = template;
			this.matcher = matcher;
		}

		public bool Contains(T element)
		{
			return elements.Contains(element);
		}

		public bool Update(T element)
		{
			if (matcher.Matches(element, template))
			{
				if (!Contains(element))
				{
					elements.Add(element);
					RaiseOnAdded(element);
				}

				return true;
			}
			else
			{
				Remove(element);
				return false;
			}
		}

		public bool Remove(T element)
		{
			if (elements.Remove(element))
			{
				RaiseOnRemoved(element);
				return true;
			}

			return false;
		}

		public void Clear()
		{
			for (int i = 0; i < elements.Count; i++)
				Remove(elements[i--]);
		}

		protected virtual void RaiseOnAdded(T element)
		{
			if (OnAdded != null)
				OnAdded(element);
		}

		protected virtual void RaiseOnRemoved(T element)
		{
			if (OnRemoved != null)
				OnRemoved(element);
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(elements));
		}

		public IEnumerator<T> GetEnumerator()
		{
			return elements.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
