using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Comparison
{
	public class EnumComparer<TEnum, TUnder> : PComparer<TEnum> where TUnder : struct, IComparable, IFormattable, IConvertible, IComparable<TUnder>, IEquatable<TUnder>
	{
		public override int Compare(TEnum x, TEnum y)
		{
			return Cast<TEnum, TUnder>.To(x).CompareTo(Cast<TEnum, TUnder>.To(y));
		}
	}
}
