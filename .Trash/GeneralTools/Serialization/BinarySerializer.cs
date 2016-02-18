using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.IO;
using Pseudo.Internal.Serialization;

namespace Pseudo
{
	public abstract class BinarySerializer<T> : IBinarySerializer<T>
	{
		public static readonly IBinarySerializer<T> Default = BinaryUtility.GetSerializer<T>();

		public abstract ushort TypeIdentifier { get; }

		public abstract void Serialize(BinaryWriter writer, T instance);

		public abstract T Deserialize(BinaryReader reader);

		void IBinarySerializer.Serialize(BinaryWriter writer, object instance)
		{
			Serialize(writer, (T)instance);
		}

		object IBinarySerializer.Deserialize(BinaryReader reader)
		{
			return Deserialize(reader);
		}
	}
}