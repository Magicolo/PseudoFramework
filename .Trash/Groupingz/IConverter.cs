using System;

namespace Pseudo.Groupingz
{
	public interface IConverter<TIn, TOut>
	{
		TOut ConvertTo(TIn value);
		TIn ConvertFrom(TOut value);
	}
}