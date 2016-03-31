using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pseudo.Internal;

namespace Pseudo
{
	[Serializable]
	public class StringStringDictionary : PDictionary<string, string> { }
	[Serializable]
	public class StringVector3Dictionary : PDictionary<string, Vector3> { }
	[Serializable]
	public class StringVector2Dictionary : PDictionary<string, Vector2> { }
	[Serializable]
	public class StringGameObjectDictionary : PDictionary<string, GameObject> { }

	namespace Internal
	{
		public class PDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
		{
			[SerializeField]
			TKey[] keys;
			[SerializeField]
			TValue[] values;

			void ISerializationCallbackReceiver.OnBeforeSerialize() { }

			void ISerializationCallbackReceiver.OnAfterDeserialize()
			{
				keys = keys ?? new TKey[0];
				Array.Resize(ref values, keys.Length);
				Clear();

				for (int i = 0; i < keys.Length; i++)
					this[keys[i]] = i < values.Length ? values[i] : default(TValue);
			}
		}
	}
}