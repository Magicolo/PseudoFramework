using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class TypeInfo : ITypeInfo
	{
		public Type Type { get; set; }
		public Type[] BaseTypes { get; set; }
		public IBindingInstaller[] Installers { get; set; }
		public IInjectableConstructor[] Constructors { get; set; }
		public IInjectableField[] Fields { get; set; }
		public IInjectableProperty[] Properties { get; set; }
		public IInjectableMethod[] Methods { get; set; }
	}
}
