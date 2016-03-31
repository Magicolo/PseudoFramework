using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Internal;

namespace Pseudo
{
	public abstract class Copier<T> : ICopier<T>
	{
		public static readonly ICopier<T> Default = CopyUtility.GetCopier<T>();

		protected static readonly bool isValueType = typeof(T).IsValueType;

		public abstract void CopyTo(T source, T target);

		public virtual void CopyTo(IList<T> source, IList<T> target)
		{
			for (int i = 0; i < source.Count; i++)
				CopyTo(source[i], target[i]);
		}

		void ICopier.CopyTo(object source, object target)
		{
			CopyTo((T)source, (T)target);
		}

		void ICopier.CopyTo(IList source, IList target)
		{
			for (int i = 0; i < source.Count; i++)
				CopyTo((T)source[i], (T)target[i]);
		}
	}
}
