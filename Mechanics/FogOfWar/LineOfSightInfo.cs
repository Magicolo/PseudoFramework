using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Reflection;

namespace Pseudo.Mechanics.Internal
{
	public class LineOfSightInfo
	{
		public int X;
		public int Y;
		public int DirectionX;
		public int DirectionY;
		public PointInfo[] Points;

		int counter;

		public LineOfSightInfo(int x, int y, int directionX, int directionY)
		{
			X = x;
			Y = y;
			DirectionX = directionX;
			DirectionY = directionY;
		}

		public void GeneratePoints(int amount)
		{
			Points = new PointInfo[amount];
			Points[0] = new PointInfo(X, Y, amount);
		}

		public PointInfo GetCurrentPoint()
		{
			return Points[counter];
		}

		public PointInfo GetNextPoint()
		{
			counter += 1;

			var point = Points[counter];

			if (point == null)
			{
				var previousPoint = Points[counter - 1];
				Points[counter] = point = new PointInfo(previousPoint.coordinateX + DirectionX, previousPoint.coordinateY + DirectionY, Points.Length);
			}

			return point;
		}

		public void Reset()
		{
			counter = 0;
		}

		public override string ToString()
		{
			return string.Format("{0}(({1}, {2}), ({3}, {4}), {5})", this.GetTypeName(), X, Y, DirectionX, DirectionY, Points.Length);
		}
	}
}