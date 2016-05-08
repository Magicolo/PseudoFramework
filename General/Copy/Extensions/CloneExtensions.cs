using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal
{
	public static class CloneExtensions
	{
		static readonly MethodInfo cloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
		static readonly Func<object, object> cloner = (Func<object, object>)Delegate.CreateDelegate(typeof(Func<object, object>), cloneMethod);

		public static T Clone<T>(this T reference)
		{
			return (T)cloner(reference);
		}
	}
}
