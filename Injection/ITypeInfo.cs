using System;

namespace Pseudo.Injection
{
	public interface ITypeInfo
	{
		Type Type { get; }
		Type[] BaseTypes { get; }
		IBindingInstaller[] Installers { get; }
		IInjectableConstructor[] Constructors { get; }
		IInjectableField[] Fields { get; }
		IInjectableProperty[] Properties { get; }
		IInjectableMethod[] Methods { get; }
	}
}