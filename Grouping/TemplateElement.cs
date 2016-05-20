using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Grouping.Internal
{
	public class TemplateElement<T> : ITemplateElement<T>
	{
		readonly Predicate<T> match;

		public TemplateElement(Predicate<T> match)
		{
			this.match = match;
		}

		public bool Matches(T value)
		{
			return match(value);
		}
	}
}
