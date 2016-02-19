using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.IO;
using System.Reflection;

namespace Pseudo.Internal.Serialization
{
	public class ObjectBinarySerializer<T> : BinarySerializer<T>
	{
		public override ushort TypeIdentifier
		{
			get { return ushort.MaxValue; }
		}

		public override void Serialize(BinaryWriter writer, T instance)
		{
			var fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

			writer.Write(typeof(T));
			writer.Write(fields.Length);

			for (int i = 0; i < fields.Length; i++)
			{
				var field = fields[i];
				writer.Write(field.Name);
				writer.Write(field.GetValue(instance));
			}
		}

		public override T Deserialize(BinaryReader reader)
		{
			var instance = Activator.CreateInstance<T>();
			var fieldCount = reader.ReadInt32();

			for (int i = 0; i < fieldCount; i++)
			{
				var field = typeof(T).GetField(reader.ReadString());
				var obj = reader.ReadObject();

				if (field != null)
					field.SetValue(instance, obj);
			}

			return instance;
		}
	}
}