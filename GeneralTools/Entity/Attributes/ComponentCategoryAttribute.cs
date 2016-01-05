using System;

namespace Pseudo
{
	public sealed class ComponentCategoryAttribute : Attribute
	{
		public readonly string Category;

		public ComponentCategoryAttribute(string category)
		{
			Category = category;
		}
	}
}