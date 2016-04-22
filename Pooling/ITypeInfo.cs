using System;

namespace Pseudo.Pooling2
{
	/// <summary>
	/// Stores the accessors for the members a given Type.
	/// </summary>
	public interface ITypeInfo
	{
		Type Type { get; }
		Type[] BaseTypes { get; }
		IInitializableField[] Fields { get; }
		IInitializableProperty[] Properties { get; }
	}
}