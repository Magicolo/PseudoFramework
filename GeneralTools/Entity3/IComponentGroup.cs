using System.Collections.Generic;

namespace Pseudo.Internal.Entity3
{
	public interface IComponentGroup
	{
		IList<IComponent> Components { get; }
	}

	public interface IComponentGroup<T> where T : IComponent
	{
		IList<T> Components { get; }
	}
}