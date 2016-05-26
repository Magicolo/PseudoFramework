using System;

namespace Pseudo
{
	[Flags]
	public enum Axes : byte
	{
		None = 0,
		X = 1,
		Y = 2,
		Z = 4,
		W = 8,
		XY = X | Y,
		XZ = X | Z,
		YZ = Y | Z,
		XYZ = X | Y | Z,
		XW = X | W,
		YW = Y | W,
		XYW = X | Y | W,
		ZW = Z | W,
		XZW = X | Z | W,
		YZW = Y | Z | W,
		XYZW = X | Y | Z | W
	}

	[Flags]
	public enum Channels : byte
	{
		None = 0,
		R = 1,
		G = 2,
		B = 4,
		A = 8,
		RG = R | G,
		RB = R | B,
		GB = G | B,
		RGB = R | G | B,
		RA = R | A,
		GA = G | A,
		RGA = R | G | A,
		BA = B | A,
		RBA = R | B | A,
		GBA = G | B | A,
		RGBA = R | G | B | A
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

