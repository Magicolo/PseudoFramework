using System.Collections.Generic;

namespace Pseudo.EntityFramework.Internal
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