using System;

namespace Pseudo
{
	public interface IEnum
	{
		Type ValueType { get; }
		object Value { get; }
		string Name { get; }

		Array GetValues();
		string[] GetNames();
	}
}