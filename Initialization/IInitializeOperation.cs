using System.Collections.Generic;

namespace Pseudo.Initialization
{
	public interface IInitializationOperation
	{
		void Initialize(ref object instance, HashSet<object> toIgnore);
	}
}