using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

namespace Pseudo
{
	public static class ListExtensions
	{
		public static void Copy<T>(this List<T> list, IList<T> source, bool resize = true)
		{
			source.CopyTo(list, resize);
		}

		public static void CopyTo<T>(this IList<T> list, List<T> target, bool resize = true)
		{
			if (resize && list.Count != target.Count)
				target.Resize(list.Count);

			int count = Mathf.Min(list.Count, target.Count);

			for (int i = 0; i < count; i++)
				target[i] = list[i];
		}

		public static void Resize<T>(this List<T> list, int length)
		{
			int count = list.Count;

			if (count > length)
				list.RemoveRange(length, count - length);
			else if (count < length)
			{
				list.Capacity = length;
				list.AddRange(Enumerable.Repeat(default(T), length - count));
			}
		}

		public static bool RemoveRange<T>(this List<T> list, T[] elements)
		{
			bool success = true;

			foreach (T element in elements)
			{
				success &= list.Remove(element);
			}

			return success;
		}

		public static T Pop<T>(this List<T> list, int index = 0)
		{
			if (list == null || list.Count == 0)
				return default(T);

			T item = list[index];
			list.RemoveAt(index);

			return item;
		}

		public static T Pop<T>(this List<T> list, T element)
		{
			return list.Pop(list.IndexOf(element));
		}

		public static T PopLast<T>(this List<T> list)
		{
			return list.Pop(list.Count - 1);
		}

		public static T PopRandom<T>(this List<T> list)
		{
			return list.Pop(Random.Range(0, list.Count));
		}

		public static List<T> PopRange<T>(this List<T> list, int startIndex, int count)
		{
			List<T> popped = new List<T>(count);

			for (int i = 0; i < count; i++)
			{
				popped[i] = list.Pop(i + startIndex);
			}
			return popped;
		}

		public static List<T> PopRange<T>(this List<T> list, int count)
		{
			return list.PopRange(0, count);
		}

		public static List<T> Slice<T>(this List<T> list, int startIndex)
		{
			return list.Slice(startIndex, list.Count - 1);
		}

		public static List<T> Slice<T>(this List<T> list, int startIndex, int endIndex)
		{
			List<T> slicedArray = new List<T>(endIndex - startIndex);
			for (int i = 0; i < endIndex - startIndex; i++)
			{
				slicedArray[i] = list[i + startIndex];
			}
			return slicedArray;
		}
	}
}
