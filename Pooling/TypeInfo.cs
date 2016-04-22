using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2
{
	public class TypeInfo : ITypeInfo
	{
		public Type Type { get; set; }
		public Type[] BaseTypes { get; set; }
		public IInitializableField[] Fields { get; set; }
		public IInitializableProperty[] Properties { get; set; }
	}
}
