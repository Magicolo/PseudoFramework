using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Grouping.Internal
{
	public class Template<T> : ITemplate<T>
	{
		public ITemplateElement<T>[] Elements
		{
			get { return elements; }
		}

		readonly ITemplateElement<T>[] elements;

		public Template(params ITemplateElement<T>[] elements)
		{
			this.elements = elements;
		}
	}
}
