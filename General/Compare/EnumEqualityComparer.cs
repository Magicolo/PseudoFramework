using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Internal
{
	public class EnumEqualityComparer<TEnum, TUnder> : PEqualityComparer<TEnum>
		where TUnder : struct, IComparable, IFormattable, IConvertible, IComparable<TUnder>, IEquatable<TUnder>
	{
		static readonly ICaster<TEnum, TUnder> caster = Caster<TEnum, TUnder>.Default;

		static EnumEqualityComparer()
		{
			if (!typeof(TEnum).IsEnum)
				throw new InvalidOperationException(string.Format("Type {0} must be an enum.", typeof(TEnum).Name));
			else if (Enum.GetUnderlyingType(typeof(TEnum)) != typeof(TUnder))
				throw new InvalidOperationException(string.Format("Type {0} must be equals to the underlying type of the enum type {1}.", typeof(TEnum).Name, typeof(TUnder).Name));
		}

		public override bool Equals(TEnum x, TEnum y)
		{
			return caster.Cast(x).Equals(caster.Cast(y));
		}

		public override int GetHashCode(TEnum obj)
		{
			return caster.Cast(obj).GetHashCode();
		}
	}
}
