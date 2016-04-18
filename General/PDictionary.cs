using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pseudo.Internal;
using System.Runtime.Serialization;

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

			public PDictionary() : base(PEqualityComparer<TKey>.Default) { }

			public PDictionary(int capacity) : base(capacity, PEqualityComparer<TKey>.Default) { }

			public PDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }

			public PDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary, PEqualityComparer<TKey>.Default) { }

			public PDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }

			public PDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }

			protected PDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

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