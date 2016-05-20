using System.Collections.Generic;

namespace Pseudo.Groupingz
{
	public enum MatchType : byte
	{
		All,
		Any,
		None,
		Exact
	}

	public interface IMatcher
	{
		bool Matches(IList<int> a, IList<int> b);
	}
}