using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface ICopyer
	{
		void CopyTo(object source, object target);
		void CopyTo(IList source, IList target);
	}

	public interface ICopyer<T> : ICopyer
	{
		void CopyTo(T source, T target);
		void CopyTo(IList<T> source, IList<T> target);
	}
}
