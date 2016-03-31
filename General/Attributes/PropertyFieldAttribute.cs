using System;
using Pseudo.Editor.Internal;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class PropertyFieldAttribute : CustomAttributeBase
	{
		public Type attributeType;
		public object[] arguments;

		public PropertyFieldAttribute() { }

		public PropertyFieldAttribute(Type attributeType, params object[] arguments)
		{
			this.attributeType = attributeType;
			this.arguments = arguments;
		}
	}
}