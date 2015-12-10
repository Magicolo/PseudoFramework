using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.IO;

namespace Pseudo.Internal
{
	public class ObjectSerializer : BinarySerializer<object>
	{
		public static readonly ObjectSerializer Instance = new ObjectSerializer();

		public override ushort TypeIdentifier { get { return ushort.MaxValue; } }

		public override void Serialize(BinaryWriter writer, object value)
		{
			var type = value.GetType();
			var fields = type.GetFields();

			writer.Write(type);
			writer.Write(fields.Length);

			for (int i = 0; i < fields.Length; i++)
			{
				var field = fields[i];
				writer.Write(field.Name);
				writer.Write(field.GetValue(value));
			}
		}

		public override object Deserialize(BinaryReader reader)
		{
			var type = reader.ReadType();
			var fieldCount = reader.ReadInt32();
			var value = Activator.CreateInstance(type);

			for (int i = 0; i < fieldCount; i++)
			{
				var field = type.GetField(reader.ReadString());
				var obj = reader.ReadObject();

				if (field != null)
					field.SetValue(value, obj);
			}

			return value;
		}
	}
}