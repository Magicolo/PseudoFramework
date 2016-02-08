using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public static class CopyUtility
	{
		static readonly Dictionary<Type, ICopyer> typeToCopyer = new Dictionary<Type, ICopyer>();

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

		public static ICopyer GetCopyer(Type type)
		{
			ICopyer copyer;

			if (!typeToCopyer.TryGetValue(type, out copyer))
			{
				copyer = CreateCopier(type);
				typeToCopyer[type] = copyer;
			}

			return copyer;
		}

		public static ICopyer<T> GetCopyer<T>()
		{
			return CopyerHolder<T>.Copyer;
		}

		static ICopyer CreateCopier(Type type)
		{
			Type copyerType;

			if (typeof(ICopyable<>).MakeGenericType(type).IsAssignableFrom(type))
				copyerType = typeof(GenericCopyer<>).MakeGenericType(type);
			else
				copyerType = Array.Find(TypeUtility.GetAssignableTypes(typeof(ICopyer<>).MakeGenericType(type), false), t => !t.IsInterface && !t.IsAbstract && t.HasEmptyConstructor());

			if (copyerType == null)
				return null;
			else
				return (ICopyer)Activator.CreateInstance(copyerType);
		}

		static class CopyerHolder<T>
		{
			public static ICopyer<T> Copyer = (ICopyer<T>)GetCopyer(typeof(T));
		}
	}
}