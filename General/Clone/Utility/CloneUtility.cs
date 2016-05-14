using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal
{
	public static class CloneUtility
	{
		static readonly Type[] clonerTypes = TypeUtility.AllTypes
			.Where(t => t.Is<ICloner>() && t.IsConcrete() && t.HasEmptyConstructor())
			.ToArray();
		static readonly MethodInfo cloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
		static readonly Func<object, object> cloner = (Func<object, object>)Delegate.CreateDelegate(typeof(Func<object, object>), cloneMethod);

		public static T MemberwiseClone<T>(T reference)
		{
			if (typeof(T).IsValueType || Equals(reference, null))
				return reference;

			return (T)cloner(reference);
		}

		public static ICloner<T> CreateCloner<T>()
		{
			var clonerType = Array.Find(clonerTypes, t => t.Is<ICloner<T>>());

			if (clonerType == null)
				return new DefaultCloner<T>();

			return (ICloner<T>)Activator.CreateInstance(clonerType);
		}
	}
}
