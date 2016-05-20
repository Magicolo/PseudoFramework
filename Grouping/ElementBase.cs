using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Grouping.Internal
{
	public abstract class ElementBase<T> : IElement<T>
	{
		public T Element
		{
			get { return element; }
		}

		protected readonly T element;

		protected ElementBase(T element)
		{
			this.element = element;
		}

		public static implicit operator T(ElementBase<T> element)
		{
			return element.Element;
		}
	}
}
