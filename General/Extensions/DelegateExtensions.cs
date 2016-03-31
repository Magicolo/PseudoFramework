using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class DelegateExtensions
	{
		public static bool Contains(this System.Delegate @delegate, System.Type type, string methodName)
		{
			bool contains = false;

			if (@delegate != null && @delegate.GetInvocationList() != null)
				contains = !System.Array.TrueForAll(@delegate.GetInvocationList(), invoker => invoker.Method.DeclaringType != type && invoker.Method.Name != methodName);

			return contains;
		}

		public static bool Contains(this System.Delegate @delegate, object obj, string methodName)
		{
			return @delegate.Contains(obj.GetType(), methodName);
		}

		public static bool Contains(this System.Delegate @delegate, string methodName)
		{
			return !System.Array.TrueForAll(@delegate.GetInvocationList(), invoker => invoker.Method.Name != methodName);
		}
	}
}
