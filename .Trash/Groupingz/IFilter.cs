using System;
using System.Collections.Generic;

namespace Pseudo.Groupingz
{
	public interface IFilter : IEquatable<IFilter>
	{
		MatchType Match { get; }
		IList<int> Identifiers { get; }
	}
}