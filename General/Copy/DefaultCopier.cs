using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public class DefaultCopier<T> : Copier<T> where T : class
	{
		public override void CopyTo(T source, T target)
		{
			var copyable = source as ICopyable<T>;

			if (copyable != null)
			{
				copyable.CopyTo(target);
				return;
			}

			copyable = target as ICopyable<T>;

			if (copyable != null)
			{
				copyable.Copy(source);
				return;
			}
		}
	}
}
