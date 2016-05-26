using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public class GenericCopier<T> : Copier<T> where T : ICopyable<T>
	{
		static readonly bool isValueType = typeof(T).IsValueType;

		public override void CopyTo(T source, T target)
		{
			if (!isValueType && (source == null || target == null))
				return;

			target.Copy(source);
		}
	}
}
