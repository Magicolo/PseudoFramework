using System.Collections.Generic;

namespace Pseudo.Groupingz
{
	public interface IElementInfo<TElement>
	{
		TElement Element { get; }
		IList<int> Identifiers { get; }

		void Add(int identifier);
		bool Remove(int identifier);
	}
}