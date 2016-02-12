using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface ICopier
	{
		void CopyTo(object source, object target);
		void CopyTo(IList source, IList target);
	}

	public interface ICopier<T> : ICopier
	{
		void CopyTo(T source, T target);
		void CopyTo(IList<T> source, IList<T> target);
	}
}
