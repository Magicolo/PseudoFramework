using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Pseudo
{
	public static class EnumExtensions
	{
		public static T ConvertByName<T>(this Enum e)
		{
			return (T)Enum.Parse(typeof(T), e.ToString());
		}
	}
}
