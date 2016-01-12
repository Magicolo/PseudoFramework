using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Entity3
{
	public class ComponentIndicesComparer : IEqualityComparer<int[]>
	{
		public static ComponentIndicesComparer Instance = new ComponentIndicesComparer();

		public bool Equals(int[] x, int[] y)
		{
			if (x.Length != y.Length)
				return false;
			else if (x.Length == 0 && y.Length == 0)
				return true;
			else if (x.Length == 1 && y.Length == 1)
				return x[0] == y[0];

			for (int i = 0; i < x.Length; i++)
			{
				if (x[i] != y[i])
					return false;
			}

			return true;
		}

		public int GetHashCode(int[] obj)
		{
			int hashCode = 0;

			for (int i = 0; i < obj.Length; i++)
				hashCode ^= obj[i] * 253;

			return hashCode;
		}
	}
}
