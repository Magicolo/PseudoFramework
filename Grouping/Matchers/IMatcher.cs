using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Grouping
{
	public interface IMatcher<T>
	{
		bool Matches(T value, ITemplate<T> template);
	}
}
