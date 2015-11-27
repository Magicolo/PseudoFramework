using System;
using Pseudo.Internal.Editor;

namespace Pseudo
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class FlagAttribute : CustomAttributeBase
	{
		public Type Type;

		public FlagAttribute(Type type)
		{
			Type = type;
		}
	}
}