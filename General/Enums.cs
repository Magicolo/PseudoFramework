using System;

namespace Pseudo
{
	[Flags]
	public enum Axes : byte
	{
		X = 1 << 0,
		Y = 1 << 1,
		Z = 1 << 2,
		W = 1 << 3,
		XY = X | Y,
		XZ = X | Z,
		XW = X | W,
		YZ = Y | Z,
		YW = Y | W,
		ZW = Z | W,
		XYZ = X | Y | Z,
		XYW = X | Y | W,
		XZW = X | Z | W,
		YZW = Y | Z | W,
		XYZW = X | Y | Z | W
	}

	[Flags]
	public enum Channels : byte
	{
		R = 1 << 0,
		G = 1 << 1,
		B = 1 << 2,
		A = 1 << 3,
		RG = R | G,
		RB = R | B,
		RA = R | A,
		GB = G | B,
		GA = G | A,
		BA = B | A,
		RGB = R | G | B,
		RGA = R | G | A,
		RBA = R | B | A,
		GBA = G | B | A,
		RGBA = R | G | B | A
	}

	[Flags]
	public enum HierarchyScopes : byte
	{
		/// <summary>
		/// Includes self.
		/// </summary>
		Self = 1 << 0,
		/// <summary>
		/// Includes the root of the hierarchy.
		/// </summary>
		Root = 1 << 1,
		/// <summary>
		/// Includes the immediate parent.
		/// </summary>
		Parent = 1 << 2,
		/// <summary>
		/// Includes all the ancestors in a bottom-top order.
		/// </summary>
		Ancestors = 1 << 3 | Parent,
		/// <summary>
		/// Includes the immediate children.
		/// </summary>
		Children = 1 << 4,
		/// <summary>
		/// Includes all the descendants in a top-bottom order.
		/// </summary>
		Descendants = 1 << 5 | Children,
		/// <summary>
		/// Includes the siblings.
		/// </summary>
		Siblings = 1 << 6,
		/// <summary>
		/// Includes all the hierarchy starting from the root in a top-bottom order.
		/// </summary>
		Hierarchy = 1 << 7 | Self | Root | Ancestors | Descendants | Siblings,
	}

	public enum MatchType : byte
	{
		All,
		Any,
		None
	}

	[Flags]
	public enum TransformModes : byte
	{
		None = 0,
		Position = 1 << 0,
		Rotation = 1 << 1,
		Scale = 1 << 2,
	}

	public enum ProbabilityDistributions : byte
	{
		Uniform,
		Gaussian,
		Exponential
	}

	public enum RaycastHitModes : byte
	{
		First,
		FirstOfEach,
		All
	}

	public enum QueryColliderInteraction : byte
	{
		UseGlobal,
		Ignore,
		Collide
	}
}

