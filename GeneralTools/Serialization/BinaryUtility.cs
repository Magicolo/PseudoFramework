using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public static class BinaryUtility
	{
		static Dictionary<short, IBinarySerializer> idSerializers;
		static Dictionary<short, IBinarySerializer> IdSerializers
		{
			get
			{
				if (idSerializers == null)
					InitializeSerializers();

				return idSerializers;
			}
		}

		static Dictionary<Type, IBinarySerializer> typeSerializers;
		static Dictionary<Type, IBinarySerializer> TypeSerializers
		{
			get
			{
				if (typeSerializers == null)
					InitializeSerializers();

				return typeSerializers;
			}
		}

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
		}

		public static bool IsSupported(object value)
		{
			TypeCodes typeCode = ToTypeCode(value);

			switch (typeCode)
			{
				case TypeCodes.Other:
					return TypeSerializers.ContainsKey(value.GetType());
				case TypeCodes.Array:
					var array = (Array)value;

					for (int i = 0; i < array.Length; i++)
					{
						if (!IsSupported(array.GetValue(i)))
							return false;
					}
					break;
			}

			return true;
		}

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
			else
				return TypeCodes.Other;
		}

		public static TypeCodes ToTypeCode(object value)
		{
			return ToTypeCode(value == null ? null : value.GetType());
		}

		public static IBinarySerializer GetSerializer(Type type)
		{
			IBinarySerializer serializer;
			TypeSerializers.TryGetValue(type, out serializer);

			return serializer;
		}

		public static IBinarySerializer GetSerializer(short typeIdentifier)
		{
			IBinarySerializer serializer;
			IdSerializers.TryGetValue(typeIdentifier, out serializer);

			return serializer;
		}

		public static BinarySerializer<T> GetSerializer<T>()
		{
			return (BinarySerializer<T>)GetSerializer(typeof(T));
		}

		static void InitializeSerializers()
		{
			typeSerializers = new Dictionary<Type, IBinarySerializer>();
			idSerializers = new Dictionary<short, IBinarySerializer>();

			var types = typeof(IBinarySerializer).GetAssignableTypes();

			for (int i = 0; i < types.Length; i++)
			{
				Type type = types[i];

				if (type.IsInterface || type.IsGenericType)
					continue;

				var serializer = (IBinarySerializer)Activator.CreateInstance(type);
				typeSerializers[type.BaseType.GetGenericArguments()[0]] = serializer;
				idSerializers[serializer.TypeIdentifier] = serializer;
			}
		}
	}
}