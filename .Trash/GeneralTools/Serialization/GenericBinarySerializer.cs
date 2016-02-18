using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.IO;

namespace Pseudo.Internal.Serialization
{
	public class GenericBinarySerializer<T> : BinarySerializer<T> where T : IBinarySerializable
	{
		public override ushort TypeIdentifier
		{
			get { return ushort.MaxValue - 1; }
		}

		public override void Serialize(BinaryWriter writer, T instance)
		{
			writer.Write(typeof(T));
			instance.Serialize(writer);
		}

		public override T Deserialize(BinaryReader reader)
		{
			var instance = Activator.CreateInstance<T>();
			instance.Deserialize(reader);

			return instance;
		}
	}
}
