using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface ICopier<in T> : ICopier where T : class
	{
		void CopyTo(T source, T target);
	}

	public interface ICopier
	{
		void CopyTo(object source, object target);
	}
}
