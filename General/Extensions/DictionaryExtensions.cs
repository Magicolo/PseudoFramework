using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

namespace Pseudo
{
	public static class DictionaryExtensions
	{
		public static void SwitchValues<T, U>(this IDictionary<T, U> dictionary, T key1, T key2)
		{
			U temp = dictionary[key1];
			dictionary[key1] = dictionary[key2];
			dictionary[key2] = temp;
		}

		public static T GetRandomKey<T, U>(this IDictionary<T, U> dictionary)
		{
			return new List<T>(dictionary.Keys).GetRandom();
		}

		public static U GetRandomValue<T, U>(this IDictionary<T, U> dictionary)
		{
			return new List<U>(dictionary.Values).GetRandom();
		}

		public static void GetOrderedKeysValues<T, U>(this IDictionary<T, U> dictionary, out T[] keys, out U[] values)
		{
			keys = dictionary.Keys.ToArray();
			values = keys.Convert(k => dictionary[k]);
		}

		public static bool Pop<T, U>(this IDictionary<T, U> dictionary, T key, out U value)
		{
			if (dictionary.TryGetValue(key, out value))
			{
				dictionary.Remove(key);
				return true;
			}
			else
				return false;
		}
	}
}
