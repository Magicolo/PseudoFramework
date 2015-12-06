using UnityEngine;
using System;
using System.Collections;

namespace Pseudo
{
	public static class BoolExtensions
	{
		public static int Sign(this bool b)
		{
			return b ? 1 : -1;
		}
	}
}
