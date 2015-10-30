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
		public static void CopyTo<T>(T[] source, ref T[] target)
		{
			if (source == null)
			{
				target = null;
				return;
			}

			if (target == null)
				target = source.Clone() as T[];
			else
			{
				if (source.Length != target.Length)
					Array.Resize(ref target, source.Length);

				for (int i = 0; i < source.Length; i++)
					target[i] = source[i];
			}
		}

		public static void CopyTo<T>(List<T> source, ref List<T> target)
		{
			if (source == null)
			{
				target = null;
				return;
			}

			if (target == null)
				target = new List<T>(target);
			else
			{
				target.Clear();

				for (int i = 0; i < source.Count; i++)
					target.Add(source[i]);
			}
		}

		public static void CopyTo<T>(Stack<T> source, ref Stack<T> target)
		{
			if (source == null)
			{
				target = null;
				return;
			}

			target = new Stack<T>(source.Reverse());
		}

		public static void CopyTo<T>(Queue<T> source, ref Queue<T> target)
		{
			if (source == null)
			{
				target = null;
				return;
			}

			target = new Queue<T>(source);
		}

		public static void CopyTo<T, U>(Dictionary<T, U> source, ref Dictionary<T, U> target)
		{
			if (source == null)
			{
				target = null;
				return;
			}

			if (target == null)
				target = new Dictionary<T, U>(source);
			else
			{
				target.Clear();

				foreach (KeyValuePair<T, U> pair in source)
					target[pair.Key] = pair.Value;
			}
		}
	}
}