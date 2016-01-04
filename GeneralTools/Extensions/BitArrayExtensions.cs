using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public static class BitArrayExtensions
	{
		public static int BitCount(this BitArray array)
		{
			int count = 0;

			for (int i = 0; i < array.Count; i++)
			{
				if (array[i])
					count++;
			}

			return count;
		}
	}
}