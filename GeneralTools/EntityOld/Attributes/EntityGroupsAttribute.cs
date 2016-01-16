using Pseudo.Internal.Editor;
using System;

namespace Pseudo
{
	/// <summary>
	/// Defines a class as an entity groups holder. Entity groups must be static readonly fields of type ByteFlag
	/// OR
	/// Defines a field of type ByteFlag as an entity group so it is correctly shown in the Unity Editor.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
	public sealed class EntityGroupsAttribute : CustomAttributeBase { }
}