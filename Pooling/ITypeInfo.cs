using System;

namespace Pseudo.PoolingNOOOO
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