using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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
			keys = dictionary.GetKeyArray();
			values = new U[keys.Length];

			for (int i = 0; i < keys.Length; i++)
				values[i] = dictionary[keys[i]];
		}

		public static T[] GetKeyArray<T, U>(this IDictionary<T, U> dictionary)
		{
			T[] keys = new T[dictionary.Keys.Count];
			int counter = 0;

			foreach (T key in dictionary.Keys)
			{
				keys[counter] = key;
				counter += 1;
			}

			return keys;
		}

		public static U[] GetValueArray<T, U>(this IDictionary<T, U> dictionary)
		{
			U[] values = new U[dictionary.Values.Count];
			int counter = 0;

			foreach (U value in dictionary.Values)
			{
				values[counter] = value;
				counter += 1;
			}

			return values;
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
