using System;
using System.Reflection;
using UnityEngine;

namespace Pseudo.Internal.References
{
	public interface IPropertyReference
	{
		UnityEngine.Object Target { get; }
		PropertyInfo Property { get; }
		Type ValueType { get; }
		object Value { get; set; }
	}
}