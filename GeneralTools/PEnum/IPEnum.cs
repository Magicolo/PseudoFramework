using System;

namespace Pseudo
{
	public interface IPEnum
	{
		Type ValueType { get; }
		object Value { get; }
		string Name { get; }
	}
}