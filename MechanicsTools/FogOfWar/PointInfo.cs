using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Mechanics
{
	public class PointInfo
	{
		public int x;
		public int y;
		public int coordinateX;
		public int coordinateY;
		public float distance;

		public PointInfo(int x, int y, int amount)
		{
			this.x = x - amount;
			this.y = y - amount;
			this.coordinateX = x;
			this.coordinateY = y;
			this.distance = Mathf.Sqrt(this.x * this.x + this.y * this.y);
		}

		public override string ToString()
		{
			return string.Format("{0}(({1}, {2}), {3}, ({4}, {5}))", this.GetTypeName(), x, y, distance, coordinateX, coordinateY);
		}
	}
}