using System.Collections.Generic;
using UnityEngine;

namespace Pseudo
{
	[System.Serializable]
	public class SerializableDictionary<K, V>
	{
		public List<K> Keys
		{
			get { return keys; }
		}

		public List<V> Values
		{
			get { return values; }
		}

		[SerializeField]
		List<K> keys = new List<K>();
		[SerializeField]
		List<V> values = new List<V>();

		public V this[K key]
		{
			get { return Get(key); }
			set { Set(key, value); }
		}

		private V Get(K key)
		{
			int index = keys.IndexOf(key);
			return values[index];
		}

		private void Set(K key, V value)
		{
			if (!ContainsKey(key))
			{
				keys.Add(key);
				values.Add(value);
			}
			else
			{
				int index = keys.IndexOf(key);
				values[index] = value;
			}

		}

		public int IndexOf(K key)
		{
			return keys.IndexOf(key);
		}
		public int IndexOf(V value)
		{
			return values.IndexOf(value);
		}

		public bool ContainsKey(K key)
		{
			return keys.IndexOf(key) != -1;
		}
		public bool ContainsValue(V value)
		{
			return values.IndexOf(value) != -1;
		}

		public void Add(Dictionary<K, V> dictionary)
		{
			foreach (var item in dictionary)
			{
				this[item.Key] = item.Value;
			}
		}
	}

	//Specifics
	[System.Serializable]
	public class StringStringSerializableDictionary : SerializableDictionary<string, string> { }

	[System.Serializable]
	public class StringVector3SerializableDictionary : SerializableDictionary<string, Vector3> { }

	[System.Serializable]
	public class StringVector2SerializableDictionary : SerializableDictionary<string, Vector2> { }

	[System.Serializable]
	public class StringGameObjectSerializableDictionary : SerializableDictionary<string, GameObject> { }
}