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
	public abstract class Copier<T> : ICopier<T> where T : class
	{
		public static ICopier<T> Default
		{
			get
			{
				if (defaultCopier == null)
					defaultCopier = CopyUtility.CreateCopier<T>();

				return defaultCopier;
			}
		}

		static ICopier<T> defaultCopier;

		public abstract void CopyTo(T source, T target);

		void ICopier.CopyTo(object source, object target)
		{
			CopyTo((T)source, (T)target);
		}
	}
}
