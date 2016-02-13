using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Internal.Comparison
{
	public class EnumEqualityComparer<TEnum, TUnder> : PEqualityComparer<TEnum> where TUnder : struct, IComparable, IFormattable, IConvertible, IComparable<TUnder>, IEquatable<TUnder>
	{
		public override bool Equals(TEnum x, TEnum y)
		{
			return Cast<TEnum, TUnder>.To(x).Equals(Cast<TEnum, TUnder>.To(y));
		}

		public override int GetHashCode(TEnum obj)
		{
			return Cast<TEnum, TUnder>.To(obj).GetHashCode();
		}
	}
}
