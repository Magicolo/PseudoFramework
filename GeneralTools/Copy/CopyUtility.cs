using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Internal.Copy
{
	public static class CopyUtility
	{
		static readonly Dictionary<Type, ICopier> typeToCopyer = new Dictionary<Type, ICopier>();

		public static void CopyTo<T>(T[] source, ref T[] target)
		{
			if (source == null)
				target = null;
			else if (target == null || target.Length != source.Length)
				target = (T[])source.Clone();
			else
				Array.Copy(source, target, source.Length);
		}

		public static void CopyTo<T>(List<T> source, ref List<T> target)
		{
			if (source == null)
				target = null;
			else if (target == null)
				target = new List<T>(target);
			else
			{
				target.Clear();
				target.AddRange(source);
			}
		}

		public static ICopier GetCopier(Type type)
		{
			ICopier copier;

			if (!typeToCopyer.TryGetValue(type, out copier))
			{
				copier = CreateCopier(type);
				typeToCopyer[type] = copier;
			}

			return copier;
		}

		public static ICopier<T> GetCopier<T>()
		{
			return (ICopier<T>)GetCopier(typeof(T));
		}

		static ICopier CreateCopier(Type type)
		{
			Type copierType;

			if (typeof(ICopyable<>).MakeGenericType(type).IsAssignableFrom(type))
				copierType = typeof(GenericCopier<>).MakeGenericType(type);
			else
			{
				var iCopyerType = typeof(ICopier<>).MakeGenericType(type);
				copierType = TypeUtility.FindType(t => t.Is(iCopyerType) && !t.IsInterface && !t.IsAbstract && t.HasEmptyConstructor());
			}

			if (copierType == null)
				return null;
			else
				return (ICopier)Activator.CreateInstance(copierType);
		}
	}
}