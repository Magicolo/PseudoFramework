using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.IO;

namespace Pseudo.Internal.Serialization
{
	public static class BinaryUtility
	{
		public enum TypeCodes : byte
		{
			Null = 0,
			Other = 1,
			Bool = 2,
			SByte = 3,
			Byte = 4,
			Int16 = 5,
			UInt16 = 6,
			Int32 = 7,
			UInt32 = 8,
			Int64 = 9,
			UInt64 = 10,
			Single = 11,
			Double = 12,
			Decimal = 13,
			Char = 14,
			String = 15,
			Type = 16,
			Vector2 = 100,
			Vector3 = 101,
			Vector4 = 102,
			Color = 103,
			Quaternion = 104,
			Rect = 105,
			Bounds = 106,
			AnimationCurve = 107,
			Keyframe = 108,
			Array = 200,
			List = 201
		}

		static readonly Dictionary<Type, IBinarySerializer> typeToSerializers = new Dictionary<Type, IBinarySerializer>();

		public static TypeCodes ToTypeCode(Type type)
		{
			if (type == null)
				return TypeCodes.Null;
			else if (type == typeof(bool))
				return TypeCodes.Bool;
			else if (type == typeof(sbyte))
				return TypeCodes.SByte;
			else if (type == typeof(byte))
				return TypeCodes.Byte;
			else if (type == typeof(short))
				return TypeCodes.Int16;
			else if (type == typeof(ushort))
				return TypeCodes.UInt16;
			else if (type == typeof(int))
				return TypeCodes.Int32;
			else if (type == typeof(uint))
				return TypeCodes.UInt32;
			else if (type == typeof(long))
				return TypeCodes.Int64;
			else if (type == typeof(ulong))
				return TypeCodes.UInt64;
			else if (type == typeof(float))
				return TypeCodes.Single;
			else if (type == typeof(double))
				return TypeCodes.Double;
			else if (type == typeof(decimal))
				return TypeCodes.Decimal;
			else if (type == typeof(char))
				return TypeCodes.Char;
			else if (type == typeof(string))
				return TypeCodes.String;
			else if (type == typeof(Type))
				return TypeCodes.Type;
			else if (type == typeof(Vector2))
				return TypeCodes.Vector2;
			else if (type == typeof(Vector3))
				return TypeCodes.Vector3;
			else if (type == typeof(Vector4))
				return TypeCodes.Vector4;
			else if (type == typeof(Color))
				return TypeCodes.Color;
			else if (type == typeof(Quaternion))
				return TypeCodes.Quaternion;
			else if (type == typeof(Rect))
				return TypeCodes.Rect;
			else if (type == typeof(Bounds))
				return TypeCodes.Bounds;
			else if (type == typeof(AnimationCurve))
				return TypeCodes.AnimationCurve;
			else if (type == typeof(Keyframe))
				return TypeCodes.Keyframe;
			else if (type.IsArray)
				return TypeCodes.Array;
			else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
				return TypeCodes.List;
			else
				return TypeCodes.Other;
		}

		public static TypeCodes ToTypeCode(object value)
		{
			return ToTypeCode(value == null ? null : value.GetType());
		}

		public static Type ToType(TypeCodes typeCode)
		{
			Type type = null;

			switch (typeCode)
			{
				case TypeCodes.Bool:
					type = typeof(bool);
					break;
				case TypeCodes.SByte:
					type = typeof(sbyte);
					break;
				case TypeCodes.Byte:
					type = typeof(byte);
					break;
				case TypeCodes.Int16:
					type = typeof(short);
					break;
				case TypeCodes.UInt16:
					type = typeof(ushort);
					break;
				case TypeCodes.Int32:
					type = typeof(int);
					break;
				case TypeCodes.UInt32:
					type = typeof(uint);
					break;
				case TypeCodes.Int64:
					type = typeof(long);
					break;
				case TypeCodes.UInt64:
					type = typeof(ulong);
					break;
				case TypeCodes.Single:
					type = typeof(float);
					break;
				case TypeCodes.Double:
					type = typeof(double);
					break;
				case TypeCodes.Decimal:
					type = typeof(decimal);
					break;
				case TypeCodes.Char:
					type = typeof(char);
					break;
				case TypeCodes.String:
					type = typeof(string);
					break;
				case TypeCodes.Type:
					type = typeof(Type);
					break;
				case TypeCodes.Vector2:
					type = typeof(Vector2);
					break;
				case TypeCodes.Vector3:
					type = typeof(Vector3);
					break;
				case TypeCodes.Vector4:
					type = typeof(Vector4);
					break;
				case TypeCodes.Color:
					type = typeof(Color);
					break;
				case TypeCodes.Quaternion:
					type = typeof(Quaternion);
					break;
				case TypeCodes.Rect:
					type = typeof(Rect);
					break;
				case TypeCodes.Bounds:
					type = typeof(Bounds);
					break;
				case TypeCodes.AnimationCurve:
					type = typeof(AnimationCurve);
					break;
				case TypeCodes.Keyframe:
					type = typeof(Keyframe);
					break;
			}

			return type;
		}

		public static bool IsGenericTypeCode(TypeCodes typeCode)
		{
			return typeCode == TypeCodes.Array || typeCode == TypeCodes.List;
		}

		public static bool IsReferenceTypeCode(TypeCodes typeCode)
		{
			return typeCode == TypeCodes.Type || typeCode == TypeCodes.AnimationCurve || typeCode == TypeCodes.Array || typeCode == TypeCodes.List;
		}

		public static IBinarySerializer GetSerializer(Type type)
		{
			IBinarySerializer serializer;

			if (!typeToSerializers.TryGetValue(type, out serializer))
			{
				serializer = CreateSerializer(type);
				typeToSerializers[type] = serializer;
			}

			return serializer;
		}

		public static IBinarySerializer<T> GetSerializer<T>()
		{
			return (IBinarySerializer<T>)GetSerializer(typeof(T));
		}

		static IBinarySerializer CreateSerializer(Type type)
		{
			if (typeof(IEnumerable).IsAssignableFrom(type))
				return null;

			Type serializerType;

			if (typeof(IBinarySerializable).IsAssignableFrom(type))
				serializerType = typeof(GenericBinarySerializer<>).MakeGenericType(type);
			else
				serializerType = TypeUtility.FindType(t =>
					!t.IsInterface &&
					!t.IsAbstract &&
					t.HasEmptyConstructor() &&
					t.GetGenericTypeDefinition() != typeof(ObjectBinarySerializer<>));

			if (serializerType == null)
				return (IBinarySerializer)Activator.CreateInstance(typeof(ObjectBinarySerializer<>).MakeGenericType(type));
			else
				return (IBinarySerializer)Activator.CreateInstance(serializerType);
		}
	}
}