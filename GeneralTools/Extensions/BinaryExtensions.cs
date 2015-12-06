using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace Pseudo.Internal
{
	public static class BinaryExtensions
	{
		public static void Write(this BinaryWriter writer, object value)
		{
			var typeCode = BinaryUtility.ToTypeCode(value);

			writer.Write((byte)typeCode);
			writer.Write(value, typeCode);
		}

		static void Write(this BinaryWriter writer, object value, BinaryUtility.TypeCodes typeCode)
		{
			switch (typeCode)
			{
				case BinaryUtility.TypeCodes.Other:
					writer.WriteOther(value);
					break;
				case BinaryUtility.TypeCodes.Bool:
					writer.Write((bool)value);
					break;
				case BinaryUtility.TypeCodes.SByte:
					writer.Write((sbyte)value);
					break;
				case BinaryUtility.TypeCodes.Byte:
					writer.Write((byte)value);
					break;
				case BinaryUtility.TypeCodes.Int16:
					writer.Write((short)value);
					break;
				case BinaryUtility.TypeCodes.UInt16:
					writer.Write((ushort)value);
					break;
				case BinaryUtility.TypeCodes.Int32:
					writer.Write((int)value);
					break;
				case BinaryUtility.TypeCodes.UInt32:
					writer.Write((uint)value);
					break;
				case BinaryUtility.TypeCodes.Int64:
					writer.Write((long)value);
					break;
				case BinaryUtility.TypeCodes.UInt64:
					writer.Write((ulong)value);
					break;
				case BinaryUtility.TypeCodes.Single:
					writer.Write((float)value);
					break;
				case BinaryUtility.TypeCodes.Double:
					writer.Write((double)value);
					break;
				case BinaryUtility.TypeCodes.Decimal:
					writer.Write((decimal)value);
					break;
				case BinaryUtility.TypeCodes.Char:
					writer.Write((char)value);
					break;
				case BinaryUtility.TypeCodes.String:
					writer.Write((string)value);
					break;
				case BinaryUtility.TypeCodes.Vector2:
					writer.Write((Vector2)value);
					break;
				case BinaryUtility.TypeCodes.Vector3:
					writer.Write((Vector3)value);
					break;
				case BinaryUtility.TypeCodes.Vector4:
					writer.Write((Vector4)value);
					break;
				case BinaryUtility.TypeCodes.Color:
					writer.Write((Color)value);
					break;
				case BinaryUtility.TypeCodes.Quaternion:
					writer.Write((Quaternion)value);
					break;
				case BinaryUtility.TypeCodes.Rect:
					writer.Write((Rect)value);
					break;
				case BinaryUtility.TypeCodes.Bounds:
					writer.Write((Bounds)value);
					break;
				case BinaryUtility.TypeCodes.AnimationCurve:
					writer.Write((AnimationCurve)value);
					break;
				case BinaryUtility.TypeCodes.Keyframe:
					writer.Write((Keyframe)value);
					break;
				case BinaryUtility.TypeCodes.Array:
					writer.Write((Array)value);
					break;
			}
		}

		public static void Write(this BinaryWriter writer, Array value)
		{
			var typeCode = BinaryUtility.ToTypeCode(value.GetType().GetElementType());
			writer.Write((byte)typeCode);

			switch (typeCode)
			{
				case BinaryUtility.TypeCodes.Other:
					writer.WriteArray(value);
					break;
				case BinaryUtility.TypeCodes.Bool:
					writer.WriteArray((bool[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.SByte:
					writer.WriteArray((sbyte[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Byte:
					writer.WriteArray((byte[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Int16:
					writer.WriteArray((short[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.UInt16:
					writer.WriteArray((ushort[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Int32:
					writer.WriteArray((int[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.UInt32:
					writer.WriteArray((uint[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Int64:
					writer.WriteArray((long[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.UInt64:
					writer.WriteArray((ulong[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Single:
					writer.WriteArray((float[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Double:
					writer.WriteArray((double[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Decimal:
					writer.WriteArray((decimal[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Char:
					writer.WriteArray((char[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.String:
					writer.WriteArray((string[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Vector2:
					writer.WriteArray((Vector2[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Vector3:
					writer.WriteArray((Vector3[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Vector4:
					writer.WriteArray((Vector4[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Color:
					writer.WriteArray((Color[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Quaternion:
					writer.WriteArray((Quaternion[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Rect:
					writer.WriteArray((Rect[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Bounds:
					writer.WriteArray((Bounds[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.AnimationCurve:
					writer.WriteArray((AnimationCurve[])value, typeCode);
					break;
				case BinaryUtility.TypeCodes.Keyframe:
					writer.WriteArray((Keyframe[])value, typeCode);
					break;
			}
		}

		static void WriteArray(this BinaryWriter writer, Array array)
		{
			int length = array.Length;
			writer.Write(length);

			for (int i = 0; i < length; i++)
				writer.Write(array.GetValue(i));
		}

		static void WriteArray<T>(this BinaryWriter writer, T[] array, BinaryUtility.TypeCodes typeCode)
		{
			int length = array.Length;
			writer.Write(length);

			for (int i = 0; i < length; i++)
				writer.Write(array[i], typeCode);
		}

		static void WriteOther(this BinaryWriter writer, object value)
		{
			IBinarySerializer serializer = BinaryUtility.GetSerializer(value.GetType());

			if (serializer == null)
				throw new NotSupportedException(string.Format("Type {0} is not supported.", value.GetType()));
			else
			{
				writer.Write(serializer.TypeIdentifier);
				serializer.Serialie(writer, value);
			}
		}

		public static void Write(this BinaryWriter writer, Vector2 value)
		{
			writer.Write(value.x);
			writer.Write(value.y);
		}

		public static void Write(this BinaryWriter writer, Vector3 value)
		{
			writer.Write(value.x);
			writer.Write(value.y);
			writer.Write(value.z);
		}

		public static void Write(this BinaryWriter writer, Vector4 value)
		{
			writer.Write(value.x);
			writer.Write(value.y);
			writer.Write(value.z);
			writer.Write(value.w);
		}

		public static void Write(this BinaryWriter writer, Quaternion value)
		{
			writer.Write(value.x);
			writer.Write(value.y);
			writer.Write(value.z);
			writer.Write(value.w);
		}

		public static void Write(this BinaryWriter writer, Color value)
		{
			writer.Write(value.r);
			writer.Write(value.g);
			writer.Write(value.b);
			writer.Write(value.a);
		}

		public static void Write(this BinaryWriter writer, Rect value)
		{
			writer.Write(value.xMin);
			writer.Write(value.yMin);
			writer.Write(value.width);
			writer.Write(value.height);
		}

		public static void Write(this BinaryWriter writer, Bounds value)
		{
			writer.Write(value.center);
			writer.Write(value.size);
		}

		public static void Write(this BinaryWriter writer, AnimationCurve value)
		{
			writer.WriteArray(value.keys, BinaryUtility.TypeCodes.Keyframe);
			writer.Write((int)value.preWrapMode);
			writer.Write((int)value.postWrapMode);
		}

		public static void Write(this BinaryWriter writer, Keyframe value)
		{
			writer.Write(value.time);
			writer.Write(value.value);
			writer.Write(value.inTangent);
			writer.Write(value.outTangent);
		}

		public static T ReadObject<T>(this BinaryReader reader)
		{
			return (T)reader.ReadObject();
		}

		public static object ReadObject(this BinaryReader reader)
		{
			return reader.ReadObject((BinaryUtility.TypeCodes)reader.ReadByte());
		}

		public static object ReadObject(this BinaryReader reader, BinaryUtility.TypeCodes typeCode)
		{
			object value = null;

			switch (typeCode)
			{
				case BinaryUtility.TypeCodes.Other:
					value = reader.ReadOther();
					break;
				case BinaryUtility.TypeCodes.Bool:
					value = reader.ReadBoolean();
					break;
				case BinaryUtility.TypeCodes.SByte:
					value = reader.ReadSByte();
					break;
				case BinaryUtility.TypeCodes.Byte:
					value = reader.ReadByte();
					break;
				case BinaryUtility.TypeCodes.Int16:
					value = reader.ReadInt16();
					break;
				case BinaryUtility.TypeCodes.UInt16:
					value = reader.ReadUInt16();
					break;
				case BinaryUtility.TypeCodes.Int32:
					value = reader.ReadInt32();
					break;
				case BinaryUtility.TypeCodes.UInt32:
					value = reader.ReadUInt32();
					break;
				case BinaryUtility.TypeCodes.Int64:
					value = reader.ReadInt64();
					break;
				case BinaryUtility.TypeCodes.UInt64:
					value = reader.ReadUInt64();
					break;
				case BinaryUtility.TypeCodes.Single:
					value = reader.ReadSingle();
					break;
				case BinaryUtility.TypeCodes.Double:
					value = reader.ReadDouble();
					break;
				case BinaryUtility.TypeCodes.Decimal:
					value = reader.ReadDecimal();
					break;
				case BinaryUtility.TypeCodes.Char:
					value = reader.ReadChar();
					break;
				case BinaryUtility.TypeCodes.String:
					value = reader.ReadString();
					break;
				case BinaryUtility.TypeCodes.Vector2:
					value = reader.ReadVector2();
					break;
				case BinaryUtility.TypeCodes.Vector3:
					value = reader.ReadVector3();
					break;
				case BinaryUtility.TypeCodes.Vector4:
					value = reader.ReadVector4();
					break;
				case BinaryUtility.TypeCodes.Color:
					value = reader.ReadColor();
					break;
				case BinaryUtility.TypeCodes.Quaternion:
					value = reader.ReadQuaternion();
					break;
				case BinaryUtility.TypeCodes.Rect:
					value = reader.ReadRect();
					break;
				case BinaryUtility.TypeCodes.Bounds:
					value = reader.ReadBounds();
					break;
				case BinaryUtility.TypeCodes.AnimationCurve:
					value = reader.ReadAnimationCurve();
					break;
				case BinaryUtility.TypeCodes.Keyframe:
					value = reader.ReadKeyframe();
					break;
				case BinaryUtility.TypeCodes.Array:
					value = reader.ReadArray();
					break;
			}

			return value;
		}

		public static T[] ReadArray<T>(this BinaryReader reader)
		{
			return (T[])reader.ReadArray();
		}

		public static Array ReadArray(this BinaryReader reader)
		{
			var typeCode = (BinaryUtility.TypeCodes)reader.ReadByte();
			Array array = null;

			switch (typeCode)
			{
				case BinaryUtility.TypeCodes.Other:
					array = reader.ReadObjectArray();
					break;
				case BinaryUtility.TypeCodes.Bool:
					array = reader.ReadObjectArray<bool>(typeCode);
					break;
				case BinaryUtility.TypeCodes.SByte:
					array = reader.ReadObjectArray<sbyte>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Byte:
					array = reader.ReadObjectArray<byte>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Int16:
					array = reader.ReadObjectArray<short>(typeCode);
					break;
				case BinaryUtility.TypeCodes.UInt16:
					array = reader.ReadObjectArray<ushort>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Int32:
					array = reader.ReadObjectArray<int>(typeCode);
					break;
				case BinaryUtility.TypeCodes.UInt32:
					array = reader.ReadObjectArray<uint>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Int64:
					array = reader.ReadObjectArray<long>(typeCode);
					break;
				case BinaryUtility.TypeCodes.UInt64:
					array = reader.ReadObjectArray<ulong>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Single:
					array = reader.ReadObjectArray<float>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Double:
					array = reader.ReadObjectArray<double>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Decimal:
					array = reader.ReadObjectArray<decimal>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Char:
					array = reader.ReadObjectArray<char>(typeCode);
					break;
				case BinaryUtility.TypeCodes.String:
					array = reader.ReadObjectArray<string>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Vector2:
					array = reader.ReadObjectArray<Vector2>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Vector3:
					array = reader.ReadObjectArray<Vector3>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Vector4:
					array = reader.ReadObjectArray<Vector4>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Color:
					array = reader.ReadObjectArray<Color>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Quaternion:
					array = reader.ReadObjectArray<Quaternion>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Rect:
					array = reader.ReadObjectArray<Rect>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Bounds:
					array = reader.ReadObjectArray<Bounds>(typeCode);
					break;
				case BinaryUtility.TypeCodes.AnimationCurve:
					array = reader.ReadObjectArray<AnimationCurve>(typeCode);
					break;
				case BinaryUtility.TypeCodes.Keyframe:
					array = reader.ReadObjectArray<Keyframe>(typeCode);
					break;
			}

			return array;
		}

		static object[] ReadObjectArray(this BinaryReader reader)
		{
			int length = reader.ReadInt32();
			object[] array = new object[length];

			for (int i = 0; i < length; i++)
				array[i] = reader.ReadObject();

			return array;
		}

		static T[] ReadObjectArray<T>(this BinaryReader reader, BinaryUtility.TypeCodes typeCode)
		{
			int length = reader.ReadInt32();
			T[] array = new T[length];

			for (int i = 0; i < length; i++)
				array[i] = (T)reader.ReadObject(typeCode);

			return array;
		}

		static object ReadOther(this BinaryReader reader)
		{
			var typeIdentifier = reader.ReadInt16();
			IBinarySerializer serializer = BinaryUtility.GetSerializer(typeIdentifier);

			if (serializer == null)
				throw new NotSupportedException(string.Format("Type with identifier {0} is not supported.", typeIdentifier));
			else
				return serializer.Deserialize(reader);
		}

		public static Vector2 ReadVector2(this BinaryReader reader)
		{
			return new Vector2(reader.ReadSingle(), reader.ReadSingle());
		}

		public static Vector3 ReadVector3(this BinaryReader reader)
		{
			return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}

		public static Vector4 ReadVector4(this BinaryReader reader)
		{
			return new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}

		public static Quaternion ReadQuaternion(this BinaryReader reader)
		{
			return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}

		public static Color ReadColor(this BinaryReader reader)
		{
			return new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}

		public static Rect ReadRect(this BinaryReader reader)
		{
			return new Rect(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}

		public static Bounds ReadBounds(this BinaryReader reader)
		{
			return new Bounds(reader.ReadVector3(), reader.ReadVector3());
		}

		public static AnimationCurve ReadAnimationCurve(this BinaryReader reader)
		{
			var value = new AnimationCurve(reader.ReadObjectArray<Keyframe>(BinaryUtility.TypeCodes.Keyframe))
			{
				preWrapMode = (WrapMode)reader.ReadInt32(),
				postWrapMode = (WrapMode)reader.ReadInt32()
			};

			return value;
		}

		public static Keyframe ReadKeyframe(this BinaryReader reader)
		{
			return new Keyframe(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}
	}
}