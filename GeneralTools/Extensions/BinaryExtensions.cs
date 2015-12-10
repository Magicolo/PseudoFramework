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

		public static void Write(this BinaryWriter writer, object value, BinaryUtility.TypeCodes typeCode)
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
				case BinaryUtility.TypeCodes.Type:
					writer.Write((Type)value);
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
				case BinaryUtility.TypeCodes.List:
					writer.Write((IList)value);
					break;
			}
		}

		public static void Write(this BinaryWriter writer, Array value)
		{
			var typeCode = BinaryUtility.ToTypeCode(value.GetType().GetElementType());
			writer.Write((byte)typeCode);
			writer.Write(value, typeCode);
		}

		public static void Write(this BinaryWriter writer, Array value, BinaryUtility.TypeCodes elementTypeCode)
		{
			if (elementTypeCode == BinaryUtility.TypeCodes.Other || BinaryUtility.IsGenericTypeCode(elementTypeCode))
				writer.Write(value.GetType().GetElementType());

			if (BinaryUtility.IsReferenceTypeCode(elementTypeCode))
			{
				writer.Write(value.Length);

				for (int i = 0; i < value.Length; i++)
					writer.Write(value.GetValue(i));
			}
			else
			{
				writer.Write(value.Length);

				for (int i = 0; i < value.Length; i++)
					writer.Write(value.GetValue(i), elementTypeCode);
			}
		}

		public static void Write(this BinaryWriter writer, IList value)
		{
			var elementTypeCode = BinaryUtility.ToTypeCode(value.GetType().GetGenericArguments()[0]);
			writer.Write((byte)elementTypeCode);
			writer.Write(value, elementTypeCode);
		}

		public static void Write(this BinaryWriter writer, IList value, BinaryUtility.TypeCodes elementTypeCode)
		{
			if (elementTypeCode == BinaryUtility.TypeCodes.Other || BinaryUtility.IsGenericTypeCode(elementTypeCode))
				writer.Write(value.GetType().GetGenericArguments()[0]);

			if (elementTypeCode == BinaryUtility.TypeCodes.Other || BinaryUtility.IsReferenceTypeCode(elementTypeCode))
			{
				writer.Write(value.Count);

				for (int i = 0; i < value.Count; i++)
					writer.Write(value[i]);
			}
			else
			{
				writer.Write(value.Count);

				for (int i = 0; i < value.Count; i++)
					writer.Write(value[i], elementTypeCode);
			}
		}

		public static void Write(this BinaryWriter writer, Type type)
		{
			writer.Write(type.FullName);
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
			writer.Write(value.keys, BinaryUtility.TypeCodes.Keyframe);
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

		static void WriteOther(this BinaryWriter writer, object value)
		{
			var serializer = BinaryUtility.GetSerializer(value.GetType());

			if (serializer == null)
				writer.Write(ushort.MaxValue - 1);
			else
			{
				writer.Write(serializer.TypeIdentifier);
				serializer.Serialize(writer, value);
			}
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
				case BinaryUtility.TypeCodes.Type:
					value = reader.ReadType();
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
				case BinaryUtility.TypeCodes.List:
					value = reader.ReadList();
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
			var elementTypeCode = (BinaryUtility.TypeCodes)reader.ReadByte();
			Array array;

			switch (elementTypeCode)
			{
				default:
					array = reader.ReadObjectArray(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Bool:
					array = reader.ReadObjectArray<bool>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.SByte:
					array = reader.ReadObjectArray<sbyte>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Byte:
					array = reader.ReadObjectArray<byte>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Int16:
					array = reader.ReadObjectArray<short>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.UInt16:
					array = reader.ReadObjectArray<ushort>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Int32:
					array = reader.ReadObjectArray<int>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.UInt32:
					array = reader.ReadObjectArray<uint>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Int64:
					array = reader.ReadObjectArray<long>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.UInt64:
					array = reader.ReadObjectArray<ulong>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Single:
					array = reader.ReadObjectArray<float>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Double:
					array = reader.ReadObjectArray<double>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Decimal:
					array = reader.ReadObjectArray<decimal>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Char:
					array = reader.ReadObjectArray<char>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.String:
					array = reader.ReadObjectArray<string>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Type:
					array = reader.ReadObjectArray<Type>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Vector2:
					array = reader.ReadObjectArray<Vector2>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Vector3:
					array = reader.ReadObjectArray<Vector3>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Vector4:
					array = reader.ReadObjectArray<Vector4>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Color:
					array = reader.ReadObjectArray<Color>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Quaternion:
					array = reader.ReadObjectArray<Quaternion>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Rect:
					array = reader.ReadObjectArray<Rect>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Bounds:
					array = reader.ReadObjectArray<Bounds>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Keyframe:
					array = reader.ReadObjectArray<Keyframe>(elementTypeCode);
					break;
			}

			return array;
		}

		public static List<T> ReadList<T>(this BinaryReader reader)
		{
			return (List<T>)reader.ReadList();
		}

		public static IList ReadList(this BinaryReader reader)
		{
			var elementTypeCode = (BinaryUtility.TypeCodes)reader.ReadByte();
			IList list;

			switch (elementTypeCode)
			{
				default:
					list = reader.ReadObjectList(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Bool:
					list = reader.ReadObjectList<bool>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.SByte:
					list = reader.ReadObjectList<sbyte>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Byte:
					list = reader.ReadObjectList<byte>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Int16:
					list = reader.ReadObjectList<short>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.UInt16:
					list = reader.ReadObjectList<ushort>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Int32:
					list = reader.ReadObjectList<int>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.UInt32:
					list = reader.ReadObjectList<uint>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Int64:
					list = reader.ReadObjectList<long>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.UInt64:
					list = reader.ReadObjectList<ulong>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Single:
					list = reader.ReadObjectList<float>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Double:
					list = reader.ReadObjectList<double>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Decimal:
					list = reader.ReadObjectList<decimal>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Char:
					list = reader.ReadObjectList<char>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.String:
					list = reader.ReadObjectList<string>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Type:
					list = reader.ReadObjectList<Type>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Vector2:
					list = reader.ReadObjectList<Vector2>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Vector3:
					list = reader.ReadObjectList<Vector3>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Vector4:
					list = reader.ReadObjectList<Vector4>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Color:
					list = reader.ReadObjectList<Color>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Quaternion:
					list = reader.ReadObjectList<Quaternion>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Rect:
					list = reader.ReadObjectList<Rect>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Bounds:
					list = reader.ReadObjectList<Bounds>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.AnimationCurve:
					list = reader.ReadObjectList<AnimationCurve>(elementTypeCode);
					break;
				case BinaryUtility.TypeCodes.Keyframe:
					list = reader.ReadObjectList<Keyframe>(elementTypeCode);
					break;
			}

			return list;
		}

		public static Type ReadType(this BinaryReader reader)
		{
			return Type.GetType(reader.ReadString());
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

		static object ReadOther(this BinaryReader reader)
		{
			ushort serializerId = reader.ReadUInt16();
			var serializer = BinaryUtility.GetSerializer(serializerId);

			if (serializer == null)
				return null;
			else
				return serializer.Deserialize(reader);
		}

		static Array ReadObjectArray(this BinaryReader reader, BinaryUtility.TypeCodes elementTypeCode)
		{
			Type elementType = BinaryUtility.ToType(elementTypeCode) ?? reader.ReadType();
			int length = reader.ReadInt32();
			var array = Array.CreateInstance(elementType, length);

			for (int i = 0; i < length; i++)
				array.SetValue(reader.ReadObject(), i);

			return array;
		}

		static T[] ReadObjectArray<T>(this BinaryReader reader, BinaryUtility.TypeCodes elementTypeCode)
		{
			int length = reader.ReadInt32();
			var array = new T[length];

			for (int i = 0; i < length; i++)
				array[i] = (T)reader.ReadObject(elementTypeCode);

			return array;
		}

		static IList ReadObjectList(this BinaryReader reader, BinaryUtility.TypeCodes elementTypeCode)
		{
			Type elementType = BinaryUtility.ToType(elementTypeCode) ?? reader.ReadType();
			int count = reader.ReadInt32();
			var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType), count);

			for (int i = 0; i < count; i++)
				list.Add(reader.ReadObject());

			return list;
		}

		static List<T> ReadObjectList<T>(this BinaryReader reader, BinaryUtility.TypeCodes elementTypeCode)
		{
			int count = reader.ReadInt32();
			var list = new List<T>(count);

			for (int i = 0; i < count; i++)
				list.Add((T)reader.ReadObject(elementTypeCode));

			return list;
		}
	}
}