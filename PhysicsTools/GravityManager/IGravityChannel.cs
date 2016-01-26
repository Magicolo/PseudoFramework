﻿using UnityEngine;

namespace Pseudo.Internal.Physics
{
	public interface IGravityChannel
	{
		GravityManager.GravityChannels Channel { get; }
		Vector3 Gravity { get; }
		Vector2 Gravity2D { get; }
		float GravityScale { get; set; }
		Vector3 Rotation { get; set; }
	}
}