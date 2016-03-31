using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Mechanics.Internal
{
	public class SecondaryLineOfSight
	{
		public LineOfSightInfo Info;
		public PrimaryLineOfSight Parent;
		public int X;
		public int Y;

		public float alpha;

		public SecondaryLineOfSight(LineOfSightInfo info, PrimaryLineOfSight parent)
		{
			Info = info;
			Parent = parent;
			alpha = parent.alpha;
			X = parent.x;
			Y = parent.y;
		}

		public void Progress()
		{
			var point = Info.GetNextPoint();

			X += Info.DirectionX;
			Y += Info.DirectionY;

			if (alpha > 0)
			{
				if (point.distance > Parent.halfRadius)
					alpha *= Parent.falloff - (point.distance - Parent.halfRadius) / Parent.halfRadius;
				else if (point.distance > Parent.minRadius)
					alpha *= Parent.preFalloff;
			}

			if (X >= 0 && X < Parent.width && Y >= 0 && Y < Parent.height)
			{
				alpha *= 1 - Parent.heightMap[X, Y];
				Parent.alphaMap[X, Y] += Parent.inverted ? (1 - alpha) / 2 : alpha / 2;
			}
		}

		public void Complete()
		{
			Info.Reset();

			while (alpha > 0)
				Progress();

			Info.Reset();
		}
	}
}