using Pseudo.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Pseudo
{
	public abstract class PEnum<TEnum, TValue> : PEnum, IEquatable<PEnum<TEnum, TValue>>, IEquatable<TEnum>
		where TEnum : PEnum<TEnum, TValue>
		where TValue : IEquatable<TValue>
	{
		static readonly FieldInfo[] fields = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static);
		static readonly Dictionary<TValue, TEnum> valueToEnum = new Dictionary<TValue, TEnum>();
		static readonly Dictionary<TValue, string> valueToName = new Dictionary<TValue, string>();
		static readonly EqualityComparer<TValue> comparer = EqualityComparer<TValue>.Default;
		static bool initialized;

		public TValue Value
		{
			get { return value; }
		}
		public override string Name
		{
			get
			{
				Initialize();
				return name;
			}
		}
		protected sealed override Type ValueType
		{
			get { return typeof(TValue); }
		}
		protected sealed override object ObjectValue
		{
			get { return value; }
		}

		[SerializeField]
		TValue value;
		string name;

		protected PEnum(TValue value)
		{
			this.value = value;
			this.name = string.Empty;
		}

		public bool Equals(PEnum<TEnum, TValue> other)
		{
			return this == other;
		}

		public virtual bool Equals(TEnum other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is PEnum<TEnum, TValue>))
				return false;

			return Equals((PEnum<TEnum, TValue>)obj);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{0}.{1}", GetType().Name, Name);
		}

		public static TEnum[] GetValues()
		{
			Initialize();

			return valueToEnum.GetValueArray();
		}

		public static string GetName(TValue value)
		{
			Initialize();
			string name;

			valueToName.TryGetValue(value, out name);

			return name;
		}

		public static string[] GetNames()
		{
			Initialize();

			return valueToName.GetValueArray();
		}

		public static TEnum GetValue(TValue value)
		{
			Initialize();
			TEnum enumValue;

			if (!valueToEnum.TryGetValue(value, out enumValue))
				enumValue = CreateValue(value, string.Empty);

			return enumValue;
		}

		protected static TEnum CreateValue(TValue value, string name)
		{
			Initialize();
			var enumValue = (TEnum)FormatterServices.GetUninitializedObject(typeof(TEnum));
			enumValue.value = value;
			enumValue.name = name;
			valueToEnum[value] = enumValue;

			if (!string.IsNullOrEmpty(name))
				valueToName[value] = name;

			return enumValue;
		}

		protected static void Initialize()
		{
			if (initialized)
				return;

			for (int i = 0; i < fields.Length; i++)
			{
				var field = fields[i];

				if (field.IsPublic && field.IsStatic && field.IsInitOnly && typeof(TEnum).IsAssignableFrom(field.FieldType))
				{
					var enumValue = field.GetValue(null) as TEnum;

					if (enumValue == null)
						continue;

					enumValue.name = field.Name;
					valueToEnum[enumValue.value] = enumValue;
					valueToName[enumValue.value] = field.Name;
				}
			}

			initialized = true;
		}

		public static implicit operator TValue(PEnum<TEnum, TValue> obj)
		{
			return obj.value;
		}

		public static implicit operator PEnum<TEnum, TValue>(TValue obj)
		{
			return GetValue(obj);
		}

		public static implicit operator TEnum(PEnum<TEnum, TValue> obj)
		{
			return (TEnum)obj;
		}

		public static bool operator ==(PEnum<TEnum, TValue> a, PEnum<TEnum, TValue> b)
		{
			if (Equals(a, null) && Equals(b, null))
				return true;
			else if (Equals(a, null) || Equals(b, null))
				return false;
			else
				return comparer.Equals(a.value, b.value);
		}

		public static bool operator !=(PEnum<TEnum, TValue> a, PEnum<TEnum, TValue> b)
		{
			if (Equals(a, null) && Equals(b, null))
				return false;
			else if (Equals(a, null) || Equals(b, null))
				return true;
			else
				return !comparer.Equals(a.value, b.value);
		}
	}
}

namespace Pseudo.Internal
{
	public abstract class PEnum : IPEnum
	{
		public abstract string Name { get; }
		protected abstract Type ValueType { get; }
		protected abstract object ObjectValue { get; }

		Type IPEnum.ValueType
		{
			get { return ValueType; }
		}
		object IPEnum.Value
		{
			get { return ObjectValue; }
		}

		public static Array GetValues(Type enumType, Type valueType)
		{
			var type = typeof(PEnum<,>).MakeGenericType(enumType, valueType);

			return (Array)type.GetMethod("GetValues").Invoke(null, null);
		}

		public static string[] GetNames(Type enumType, Type valueType)
		{
			var type = typeof(PEnum<,>).MakeGenericType(enumType, valueType);

			return (string[])type.GetMethod("GetNames").Invoke(null, null);
		}
	}
}
