using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class MessageEnum : IEquatable<MessageEnum>, IEquatable<Enum>
	{
		public Enum Value
		{
			get
			{
				if (enumValue == null && ValueType != null)
					enumValue = (Enum)Enum.ToObject(ValueType, value);

				return enumValue;
			}
			set
			{
				if (value == null)
				{
					type = null;
					typeName = null;
					this.value = -1;
				}
				else
				{
					type = value.GetType();
					typeName = type.AssemblyQualifiedName;
					this.value = ((IConvertible)value).ToInt32(null);
				}

				enumValue = value;
			}
		}
		public Type ValueType
		{
			get
			{
				if (type == null && typeName != null)
					type = Type.GetType(typeName);

				return type;
			}
		}

		[SerializeField]
		int value;
		[SerializeField]
		string typeName;
		Type type;
		Enum enumValue;

		public MessageEnum(Enum value)
		{
			Value = value;
		}

		public bool Equals<T>(T obj)
		{
			if (enumValue is T)
				return TypeUtility.GetEqualityComparer<T>().Equals(obj, (T)(object)enumValue);
			else if (obj is MessageEnum)
				return Equals(obj as MessageEnum);
			else
				return false;
		}

		public bool Equals(Enum other)
		{
			return enumValue == other;
		}

		public bool Equals(MessageEnum other)
		{
			if (other == null)
				return false;
			else
				return type == other.type && value == other.value;
		}

		public override bool Equals(object obj)
		{
			if (obj is MessageEnum)
				return Equals((MessageEnum)obj);
			else if (obj is Enum)
				return Equals((Enum)obj);
			else
				return false;
		}

		public override int GetHashCode()
		{
			if (type == null)
				return -1;
			else
				return type.GetHashCode() ^ value.GetHashCode();
		}
	}
}
