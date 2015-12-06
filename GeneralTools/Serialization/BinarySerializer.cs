using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.IO;

namespace Pseudo.Internal
{
	public abstract class BinarySerializer<T> : IBinarySerializer
	{
		public abstract short TypeIdentifier { get; }

		public abstract void Serialize(BinaryWriter writer, T instance);

		public abstract T Deserialize(BinaryReader reader);

		void IBinarySerializer.Serialie(BinaryWriter writer, object instance)
		{
			Serialize(writer, (T)instance);
		}

		object IBinarySerializer.Deserialize(BinaryReader reader)
		{
			return Deserialize(reader);
		}
	}
}