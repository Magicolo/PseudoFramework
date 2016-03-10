using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Schema
{
	public class SchemaAsset : ScriptableObject, ISerializationCallbackReceiver
	{
		public Schema Schema
		{
			get { return schema; }
		}

		Schema schema;
		[SerializeField]
		string data;

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			data = Schema.Serialize(schema);
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			schema = Schema.Deserialize(data);
		}
	}
}
