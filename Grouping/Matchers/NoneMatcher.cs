using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Grouping.Internal
{
	public class NoneMatcher<T> : AnyMatcher<T>
	{
		public override bool Matches(T value, ITemplate<T> template)
		{
			return !base.Matches(value, template);
		}
	}
}
