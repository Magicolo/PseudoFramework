using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<K, V>
{

	public List<K> Keys = new List<K>();
	public List<V> Values = new List<V>();

	public V this[K key]
	{
		get { return Get(key); }
		set { Set(key, value); }
	}

	private V Get(K key)
	{
		int index = Keys.IndexOf(key);
		return Values[index];
	}

	private void Set(K key, V value)
	{
		if (!ContainsKey(key))
		{
			Keys.Add(key);
			Values.Add(value);
		}
		else
		{
			int index = Keys.IndexOf(key);
			Values[index] = value;
		}

	}

	public int IndexOf(K key)
	{
		return Keys.IndexOf(key);
	}
	public int IndexOf(V value)
	{
		return Values.IndexOf(value);
	}


	public bool ContainsKey(K key)
	{
		return Keys.IndexOf(key) != -1;
	}
	public bool ContainsValue(V value)
	{
		return Values.IndexOf(value) != -1;
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