using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Mechanics
{
	public class PrimaryLineOfSight
	{
		public LineOfSightInfo info;
		public int x;
		public int y;
		public float minRadius;
		public float maxRadius;
		public float strength;
		public float preFalloff;
		public float falloff;
		public bool inverted;
		public float[,] alphaMap;
		public float[,] heightMap;

		public float alpha;
		public float halfRadius;
		public float eighthRadius;
		public int width;
		public int height;
		public List<LineOfSightInfo>[,] lineInfos;
		public int childrenCount;

		public PrimaryLineOfSight(LineOfSightInfo info, int x, int y, float minRadius, float maxRadius, float strength, float preFalloff, float falloff, bool inverted, float[,] alphaMap, float[,] heightMap, List<LineOfSightInfo>[,] lineInfos)
		{
			this.info = info;
			this.x = x;
			this.y = y;
			this.minRadius = minRadius;
			this.maxRadius = maxRadius;
			this.halfRadius = maxRadius / 2;
			this.eighthRadius = maxRadius / 8;
			this.strength = strength;
			this.preFalloff = preFalloff;
			this.falloff = falloff;
			this.inverted = inverted;
			this.alphaMap = alphaMap;
			this.heightMap = heightMap;
			this.lineInfos = lineInfos;
			this.alpha = strength;

			width = alphaMap.GetLength(0);
			height = alphaMap.GetLength(1);
		}

		public void Progress()
		{
			PointInfo point = info.GetNextPoint();

			x += info.directionX;
			y += info.directionY;

			if (alpha > 0)
			{
				if (point.distance > halfRadius)
				{
					alpha *= falloff - (point.distance - halfRadius) / halfRadius;
				}
				else if (point.distance > minRadius)
				{
					alpha *= preFalloff;
				}
			}

			if (x >= 0 && x < width && y >= 0 && y < height)
			{
				alpha *= 1 - heightMap[x, y];
				alphaMap[x, y] += inverted ? 1 - alpha : alpha;
			}

			List<LineOfSightInfo> childInfos = lineInfos[point.coordinateX, point.coordinateY];
			new SecondaryLineOfSight(childInfos[0], this).Complete();
			new SecondaryLineOfSight(childInfos[1], this).Complete();
		}

		public void Complete()
		{
			info.Reset();

			while (alpha > 0)
			{
				Progress();
			}

			info.Reset();
		}
	}
}