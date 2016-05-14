using Pseudo;
using Pseudo.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pseudo
{
	[Serializable]
	public class DynamicValue : ICopyable<DynamicValue>, ISerializationCallbackReceiver
	{
		public enum ValueTypes : byte
		{
			Null = 0,
			Bool = TypeCode.Boolean,
			Int = TypeCode.Int32,
			Float = TypeCode.Single,
			Char = TypeCode.Char,
			String = TypeCode.String,
			Vector2 = 100,
			Vector3 = 101,
			Vector4 = 102,
			Color = 103,
			Quaternion = 104,
			Rect = 105,
			Bounds = 106,
			AnimationCurve = 107,
			Object = 150,
		}

		public object Value
		{
			get { return value; }
			set { SetValue(value); }
		}
		public ValueTypes Type
		{
			get { return valueType; }
			set { SetType(value, isArray); }
		}
		public bool IsArray
		{
			get { return isArray; }
			set { SetType(valueType, value); }
		}

		object value;
		[SerializeField]
		ValueTypes valueType;
		[SerializeField, Toggle("Array", "Array")]
		bool isArray;
		[SerializeField, HideInInspector]
		string data;
		[SerializeField]
		UnityEngine.Object[] objectValue;

		public void SetType(ValueTypes type, bool isArray)
		{
			if (this.valueType == type && this.isArray == isArray)
				return;

			this.valueType = type;
			this.isArray = isArray;

			SetValue(GetDefaultValue(type, isArray));
		}

		public void Serialize()
		{
			if (valueType == ValueTypes.Object)
			{
				if (isArray)
					objectValue = (UnityEngine.Object[])value;
				else
				{
					if (objectValue == null)
						objectValue = new UnityEngine.Object[1];
					else if (objectValue.Length != 1)
						Array.Resize(ref objectValue, 1);

					objectValue[0] = (UnityEngine.Object)value;
				}
			}
			else
				data = JsonUtility.ToJson(value);
		}

		public void Deserialize()
		{
			if (valueType == ValueTypes.Object)
			{
				if (isArray)
					value = objectValue;
				else if (objectValue != null && objectValue.Length > 0)
					value = objectValue[0];
			}
			else if (!string.IsNullOrEmpty(data))
			{
				var type = ToType(valueType, isArray);

				if (type != null)
					value = JsonUtility.FromJson(data, type);
			}
			else
				value = GetDefaultValue(valueType, isArray);
		}

		public void Copy(DynamicValue reference)
		{
			value = reference.value;
			valueType = reference.valueType;
			isArray = reference.isArray;
			data = reference.data;
			objectValue = (UnityEngine.Object[])reference.objectValue.Clone();
		}

		public void CopyTo(DynamicValue instance)
		{
			instance.Copy(this);
		}

		void SetValue(object value)
		{
			this.value = value;
			isArray = value is Array;

			if (value == null)
			{
				if (valueType != ValueTypes.Null || valueType != ValueTypes.Object)
					value = GetDefaultValue(valueType, isArray);
			}
			else
			{
				if (value is UnityEngine.Object[] || value is UnityEngine.Object)
					valueType = ValueTypes.Object;
				else
					valueType = ToValueType(value.GetType());
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			Deserialize();
		}

		public override string ToString()
		{
			return string.Format("{0}({1}{2}, {3})", GetType().Name, valueType, isArray ? "[]" : "", PDebug.ToString(Value));
		}

		public static Type ToType(ValueTypes valueType, bool isArray)
		{
			Type type = null;

			switch (valueType)
			{
				case ValueTypes.Bool:
					type = ToType<bool>(isArray);
					break;
				case ValueTypes.Int:
					type = ToType<int>(isArray);
					break;
				case ValueTypes.Float:
					type = ToType<float>(isArray);
					break;
				case ValueTypes.Char:
					type = ToType<char>(isArray);
					break;
				case ValueTypes.String:
					type = ToType<string>(isArray);
					break;
				case ValueTypes.Vector2:
					type = ToType<Vector2>(isArray);
					break;
				case ValueTypes.Vector3:
					type = ToType<Vector3>(isArray);
					break;
				case ValueTypes.Vector4:
					type = ToType<Vector4>(isArray);
					break;
				case ValueTypes.Color:
					type = ToType<Color>(isArray);
					break;
				case ValueTypes.Quaternion:
					type = ToType<Quaternion>(isArray);
					break;
				case ValueTypes.Rect:
					type = ToType<Rect>(isArray);
					break;
				case ValueTypes.Bounds:
					type = ToType<Bounds>(isArray);
					break;
				case ValueTypes.AnimationCurve:
					type = ToType<AnimationCurve>(isArray);
					break;
			}

			return type;
		}

		public static ValueTypes ToValueType(Type type)
		{
			if (type == null)
				return ValueTypes.Null;
			else if (type.IsArray)
				return ToValueType(type.GetElementType());
			else if (type == typeof(bool))
				return ValueTypes.Bool;
			else if (type == typeof(int))
				return ValueTypes.Int;
			else if (type == typeof(float))
				return ValueTypes.Float;
			else if (type == typeof(char))
				return ValueTypes.Char;
			else if (type == typeof(string))
				return ValueTypes.String;
			else if (type == typeof(Vector2))
				return ValueTypes.Vector2;
			else if (type == typeof(Vector3))
				return ValueTypes.Vector3;
			else if (type == typeof(Vector4))
				return ValueTypes.Vector4;
			else if (type == typeof(Color))
				return ValueTypes.Color;
			else if (type == typeof(Quaternion))
				return ValueTypes.Quaternion;
			else if (type == typeof(Rect))
				return ValueTypes.Rect;
			else if (type == typeof(Bounds))
				return ValueTypes.Bounds;
			else if (type == typeof(AnimationCurve))
				return ValueTypes.AnimationCurve;
			else if (typeof(UnityEngine.Object).IsAssignableFrom(type))
				return ValueTypes.Object;

			return ValueTypes.Null;
		}

		static Type ToType<T>(bool isArray)
		{
			return isArray ? typeof(T[]) : typeof(T);
		}

		static object GetDefaultValue(ValueTypes valueType, bool isArray)
		{
			object defaultValue = null;

			switch (valueType)
			{
				case ValueTypes.Bool:
					defaultValue = GetDefaultValue<bool>(isArray);
					break;
				case ValueTypes.Int:
					defaultValue = GetDefaultValue<int>(isArray);
					break;
				case ValueTypes.Float:
					defaultValue = GetDefaultValue<float>(isArray);
					break;
				case ValueTypes.Char:
					defaultValue = GetDefaultValue<char>(isArray);
					break;
				case ValueTypes.String:
					defaultValue = GetDefaultValue<string>(isArray);
					break;
				case ValueTypes.Vector2:
					defaultValue = GetDefaultValue<Vector2>(isArray);
					break;
				case ValueTypes.Vector3:
					defaultValue = GetDefaultValue<Vector3>(isArray);
					break;
				case ValueTypes.Vector4:
					defaultValue = GetDefaultValue<Vector4>(isArray);
					break;
				case ValueTypes.Color:
					defaultValue = GetDefaultValue<Color>(isArray);
					break;
				case ValueTypes.Quaternion:
					defaultValue = GetDefaultValue<Quaternion>(isArray);
					break;
				case ValueTypes.Rect:
					defaultValue = GetDefaultValue<Rect>(isArray);
					break;
				case ValueTypes.Bounds:
					defaultValue = GetDefaultValue<Bounds>(isArray);
					break;
				case ValueTypes.AnimationCurve:
					defaultValue = GetDefaultValue<AnimationCurve>(isArray);
					break;
			}

			return defaultValue;
		}

		static object GetDefaultValue<T>(bool isArray)
		{
			if (isArray)
				return new T[0];
			else if (typeof(T) == typeof(string))
				return string.Empty;
			else
				return Activator.CreateInstance(typeof(T), true);
		}
	}
}