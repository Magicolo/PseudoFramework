using Pseudo.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace Pseudo
{
	/// <summary>
	/// Custom enum implementation to overcome C#'s enum limitations.
	/// *WARNING* Do not use default values on fields that are serialized by Unity.
	/// </summary>
	/// <typeparam name="TEnum">Type of the enum.</typeparam>
	/// <typeparam name="TValue">Type of the value held by the enum.</typeparam>
	public abstract class PEnum<TEnum, TValue> : PEnum, IEnum, IEquatable<PEnum<TEnum, TValue>>, IEquatable<TEnum>
		where TEnum : PEnum<TEnum, TValue>
		where TValue : IEquatable<TValue>
	{
		static TEnum[] values;
		static string[] names;
		static readonly FieldInfo[] fields = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static);
		static readonly Dictionary<TValue, TEnum> valueToEnum = new Dictionary<TValue, TEnum>();
		static readonly Dictionary<TValue, string> valueToName = new Dictionary<TValue, string>();
		static readonly IEqualityComparer<TValue> comparer = PEqualityComparer<TValue>.Default;
		static bool initialized;

		public TValue Value
		{
			get { return value; }
		}
		public string Name
		{
			get
			{
				Initialize();
				return name;
			}
		}

		Type IEnum.ValueType
		{
			get { return typeof(TValue); }
		}
		object IEnum.Value
		{
			get { return Value; }
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

		Array IEnum.GetValues()
		{
			return GetValues();
		}

		string[] IEnum.GetNames()
		{
			return GetNames();
		}

		public static TEnum[] GetValues()
		{
			Initialize();

			return values;
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

			return names;
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
			valueToName[value] = name;

			return enumValue;
		}

		protected static void Initialize()
		{
			if (initialized)
				return;

			var valueList = new List<TEnum>();
			var nameList = new List<string>();

			for (int i = 0; i < fields.Length; i++)
			{
				var field = fields[i];

				if (field.IsInitOnly && field.FieldType.Is<TEnum>())
				{
					var enumValue = field.GetValue(null) as TEnum;

					if (enumValue == null)
						continue;

					enumValue.name = field.Name;
					valueList.Add(enumValue);
					nameList.Add(field.Name);
					valueToEnum[enumValue.value] = enumValue;
					valueToName[enumValue.value] = field.Name;
				}
			}

			values = valueList.ToArray();
			names = nameList.ToArray();
			initialized = true;
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

	namespace Internal
	{
		public abstract class PEnum { }
	}
}