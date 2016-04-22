using System;

namespace Pseudo.Pooling2
{
	/// <summary>
	/// Analyzes a type and creates accessors for its members stored in a ITypeInfo instance.
	/// </summary>
	public interface ITypeAnalyzer
	{
		ITypeInfo Analyze(Type type);
	}
}