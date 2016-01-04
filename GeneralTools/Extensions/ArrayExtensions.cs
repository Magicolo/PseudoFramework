﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class ArrayExtensions
	{
		public static T Get<T>(this T[,] array, Point2 point)
		{
			return array[point.X, point.Y];
		}

		public static void Set<T>(this T[,] array, Point2 point, T value)
		{
			array[point.X, point.Y] = value;
		}

		public static bool ContainsPoint<T>(this T[,] array, Point2 point)
		{
			return point.X >= array.GetLowerBound(0) && point.X < array.GetUpperBound(0) && point.Y >= array.GetLowerBound(1) && point.Y < array.GetUpperBound(1);
		}

		public static bool Contains<T>(this T[] array, T value)
		{
			return Contains((IList<T>)array, value);
		}

		public static void Clear<T>(this T[] array)
		{
			Array.Clear(array, 0, array.Length);
		}

		public static T Pop<T>(this T[] array, int index, out T[] remaining)
		{
			List<T> list = new List<T>(array);

			T item = list.Pop(index);
			remaining = list.ToArray();

			return item;
		}

		public static T Pop<T>(this T[] array, T element, out T[] remaining)
		{
			return array.Pop(Array.IndexOf(array, element), out remaining);
		}

		public static T Pop<T>(this T[] array, out T[] remaining)
		{
			return array.Pop(0, out remaining);
		}

		public static T PopRandom<T>(this T[] array, out T[] remaining)
		{
			return array.Pop(PRandom.Range(0, array.Length - 1), out remaining);
		}

		public static T[] PopRange<T>(this T[] array, int startIndex, int count, out T[] remaining)
		{
			List<T> list = new List<T>(array);
			T[] popped = list.PopRange(startIndex, count).ToArray();
			remaining = list.ToArray();

			return popped;
		}

		public static T[] PopRange<T>(this T[] array, int count, out T[] remaining)
		{
			return array.PopRange(0, count, out remaining);
		}

		public static T[] Joined<T>(this T[] array, IList<T> other)
		{
			T[] joined = new T[array.Length + other.Count];

			array.CopyTo(joined, 0);
			other.CopyTo(joined, array.Length);

			return joined;
		}

		public static T[] Slice<T>(this T[] array, int startIndex)
		{
			return array.Slice(startIndex, array.Length - startIndex);
		}

		public static T[] Slice<T>(this T[] array, int startIndex, int count)
		{
			T[] slicedArray = new T[count];

			for (int i = 0; i < count; i++)
				slicedArray[i] = array[i + startIndex];

			return slicedArray;
		}

		public static U[] Convert<T, U>(this IList<T> array, Func<T, U> conversion)
		{
			return array.Convert(conversion, 0, array.Count);
		}

		public static U[] Convert<T, U>(this IList<T> array, Func<T, U> conversion, int startIndex, int count)
		{
			U[] converted = new U[array.Count];

			for (int i = startIndex; i < Mathf.Min(startIndex + count, array.Count); i++)
				converted[i] = conversion(array[i]);

			return converted;
		}

		public static T[] Reversed<T>(this T[] array)
		{
			T[] reversedArray = new T[array.Length];

			for (int i = 0; i < array.Length; i++)
				reversedArray[i] = array[array.Length - i - 1];

			return reversedArray;
		}

		public static void Reverse<T>(this IList<T> array)
		{
			for (int i = 0; i < array.Count / 2; i++)
				array.Switch(i, array.Count - i - 1);
		}

		public static T First<T>(this IList<T> array)
		{
			return array != null && array.Count > 0 ? array[0] : default(T);
		}

		public static T Last<T>(this IList<T> array)
		{
			return array != null && array.Count > 0 ? array[array.Count - 1] : default(T);
		}

		public static void Fill<T>(this IList<T> array, Func<int, T> getValue)
		{
			array.Fill(getValue, 0, array.Count);
		}

		public static void Fill<T>(this IList<T> array, Func<int, T> getValue, int startIndex, int count)
		{
			for (int i = startIndex; i < Mathf.Min(startIndex + count, array.Count); i++)
				array[i] = getValue(i);
		}

		public static void Fill<T>(this IList<T> array, T value)
		{
			array.Fill(value, 0, array.Count);
		}

		public static void Fill<T>(this IList<T> array, T value, int startIndex, int count)
		{
			for (int i = startIndex; i < Mathf.Min(startIndex + count, array.Count); i++)
				array[i] = value;
		}

		public static Type[] GetTypes(this IList array)
		{
			var types = new Type[array.Count];

			for (int i = 0; i < array.Count; i++)
			{
				var element = array[i];
				types[i] = element == null ? null : element.GetType();
			}

			return types;
		}

		public static string[] GetTypeNames(this IList array)
		{
			var typeNames = new string[array.Count];

			for (int i = 0; i < array.Count; i++)
			{
				var element = array[i];
				typeNames[i] = element == null ? "" : element.GetType().Name;
			}

			return typeNames;
		}

		public static T GetRandom<T>(this IList<T> array)
		{
			if (array == null || array.Count == 0)
				return default(T);

			return array[PRandom.Range(0, array.Count - 1)];
		}

		public static void Move<T>(this IList<T> array, int sourceIndex, int targetIndex)
		{
			while (sourceIndex != targetIndex)
				array.Switch(sourceIndex, sourceIndex += (targetIndex - sourceIndex).Sign());
		}

		public static void Switch<T>(this IList<T> array, int sourceIndex, int targetIndex)
		{
			var temp = array[sourceIndex];
			array[sourceIndex] = array[targetIndex];
			array[targetIndex] = temp;
		}

		public static void Switch<T>(this IList<T> array, T source, T target)
		{
			if (array.Contains(source) && array.Contains(target))
				array.Switch(array.IndexOf(source), array.IndexOf(target));
		}

		public static bool ContentEquals<T>(this IList<T> array, IList<T> otherArray)
		{
			if (array == null && otherArray == null)
				return true;

			if (array == null || otherArray == null)
				return false;

			if (array.Count != otherArray.Count)
				return false;

			for (int i = 0; i < array.Count; i++)
			{
				if (!EqualityComparer<T>.Default.Equals(array[i], otherArray[i]))
					return false;
			}

			return true;
		}

		public static string[] ToStringArray(this IList array)
		{
			var stringArray = new string[array.Count];

			for (int i = 0; i < array.Count; i++)
			{
				var element = array[i];

				if (element is ValueType || element != null)
					stringArray[i] = array[i].ToString();
			}

			return stringArray;
		}

		public static T GetClosest<T>(this IList<T> array, Vector3 position) where T : PMonoBehaviour
		{
			T closest = null;
			float closestDistance = float.MaxValue;

			for (int i = 0; i < array.Count; i++)
			{
				var element = array[i];
				float distance = (element.CachedTransform.position - position).sqrMagnitude;

				if (distance < closestDistance)
				{
					closest = element;
					closestDistance = distance;
				}
			}

			return closest;
		}

		public static bool ContainsAll<T>(this IList<T> array, IList<T> otherArray)
		{
			if (array.Count == 0 && otherArray.Count == 0)
				return true;
			else if (array.Count == 0)
				return false;
			else if (otherArray.Count == 0)
				return true;

			for (int i = 0; i < otherArray.Count; i++)
			{
				if (!Contains(array, otherArray[i]))
					return false;
			}

			return true;
		}

		public static bool ContainsAny<T>(this IList<T> array, IList<T> otherArray)
		{
			if (array.Count == 0 || otherArray.Count == 0)
				return false;

			for (int i = 0; i < otherArray.Count; i++)
			{
				if (Contains(array, otherArray[i]))
					return true;
			}

			return false;
		}

		public static bool ContainsNone<T>(this IList<T> array, IList<T> otherArray)
		{
			return !array.ContainsAny(otherArray);
		}

		public static int Count<T>(this IList<T> array, Predicate<T> match)
		{
			int count = 0;

			for (int i = 0; i < array.Count; i++)
			{
				if (match(array[i]))
					count++;
			}

			return count;
		}

		static bool Contains<T>(IList<T> array, T element)
		{
			if (element == null)
			{
				for (int i = 0; i < array.Count; i++)
				{
					if (array[i] == null)
						return true;
				}

				return false;
			}
			else
			{
				var comparer = EqualityComparer<T>.Default;
				for (int i = 0; i < array.Count; i++)
				{
					if (comparer.Equals(array[i], element))
						return true;
				}

				return false;
			}
		}
	}
}
