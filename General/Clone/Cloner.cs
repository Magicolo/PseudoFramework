using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public abstract class Cloner<T> : ICloner<T>
	{
		public static ICloner<T> Default
		{
			get
			{
				if (defaultCloner == null)
					defaultCloner = CloneUtility.CreateCloner<T>();

				return defaultCloner;
			}
		}

		static ICloner<T> defaultCloner;

		public abstract T Clone(T source);

		object ICloner.Clone(object source)
		{
			return Clone((T)source);
		}
	}
}
