﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Internal
{
	public class DefaultCloner<T> : Cloner<T>
	{
		public override T Clone(T reference)
		{
			var cloneable = reference as ICloneable<T>;

			if (cloneable == null)
				return CloneUtility.MemberwiseClone(reference);
			else
				return cloneable.Clone();
		}
	}
}
