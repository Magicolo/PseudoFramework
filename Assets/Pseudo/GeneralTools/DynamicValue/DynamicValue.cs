using Pseudo;
using Pseudo.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pseudo
{
	[Serializable]
	public class DynamicValue : IPoolable, ICopyable<DynamicValue>
	{
		public enum ValueTypes
		{
			Null,
			Int,
			Float,
			Bool,
			String,
			Vector2,
			Vector3,
			Vector4,
			Quaternion,
			Color,
			Rect,
			Bounds,
			AnimationCurve,
			Object,
		}

		public bool IsArray { get { return _isArray; } }

		object _valueCached;
		object _value;
		[SerializeField]
		ValueTypes _type;
		[SerializeField, Toggle("Array", "Array")]
		bool _isArray;
		[SerializeField, HideInInspector]
		byte[] _data;
		[SerializeField]
		UnityEngine.Object[] _objectValue;

		static readonly object _dummy = new object();
		static readonly MemoryStream _stream = new MemoryStream();
		static readonly BinaryReader _reader = new BinaryReader(_stream);
		static readonly BinaryWriter _writer = new BinaryWriter(_stream);

		public static readonly DynamicValue Default = new DynamicValue();

		public T GetValue<T>()
		{
			return (T)GetValue();
		}

		public object GetValue()
		{
			if (_valueCached == null)
			{
				if (_type == ValueTypes.Object)
					if (_isArray)
						_value = _objectValue;
					else if (_objectValue == null || _objectValue.Length == 0)
						_value = null;
					else
						_value = _objectValue[0];
				else
					Deserialize();

				_valueCached = _dummy;
			}

			return _value;
		}

		public void SetValue(object value)
		{
			_value = value;
			_isArray = value is Array;

			if (value == null)
			{
				if (_type != ValueTypes.Null || _type != ValueTypes.Object)
					_value = GetDefaultValue(_type, _isArray);
			}
			else
			{
				if ((_isArray && _value is UnityEngine.Object[]) || (!_isArray && _value is UnityEngine.Object))
					_type = ValueTypes.Object;
				else
					_type = TypeToEnum(_value.GetType());
			}

#if UNITY_EDITOR
			if (_type == ValueTypes.Object)
			{
				if (_isArray)
					_objectValue = (UnityEngine.Object[])_value;
				else
				{
					if (_objectValue == null)
						_objectValue = new UnityEngine.Object[1];
					else if (_objectValue.Length != 1)
						Array.Resize(ref _objectValue, 1);

					_objectValue[0] = (UnityEngine.Object)_value;
				}
			}
			else
				Serialize();
#endif
		}

		public ValueTypes GetValueType()
		{
			return _type;
		}

		public void SetValueType(ValueTypes type, bool isArray)
		{
			if (_type == type && _isArray == isArray)
				return;

			_type = type;
			_isArray = isArray;

			SetValue(GetDefaultValue(type, isArray));
		}

		void Serialize()
		{
			_data = Serialize(_type, _isArray, _value);
		}

		void Deserialize()
		{
			_value = Deserialize(_type, _isArray, _data);
		}

		public void OnCreate()
		{
		}

		public void OnRecycle()
		{
		}

		public void Copy(DynamicValue reference)
		{
			_valueCached = reference._valueCached;
			_value = reference._value;
			_type = reference._type;
			_isArray = reference._isArray;
			CopyHelper.CopyTo(reference._data, ref _data);
			CopyHelper.CopyTo(reference._objectValue, ref _objectValue);
		}

		public override string ToString()
		{
			return string.Format("{0}({1}{2}, {3})", GetType().Name, _type, _isArray ? "[]" : "", GetValue());
		}

		public static Type EnumToType(ValueTypes type)
		{
			switch (type)
			{
				default:
					return null;
				case ValueTypes.Int:
					return typeof(int);
				case ValueTypes.Float:
					return typeof(float);
				case ValueTypes.Bool:
					return typeof(bool);
				case ValueTypes.String:
					return typeof(string);
				case ValueTypes.Vector2:
					return typeof(Vector2);
				case ValueTypes.Vector3:
					return typeof(Vector3);
				case ValueTypes.Vector4:
					return typeof(Vector4);
				case ValueTypes.Quaternion:
					return typeof(Quaternion);
				case ValueTypes.Color:
					return typeof(Color);
				case ValueTypes.Rect:
					return typeof(Rect);
				case ValueTypes.Bounds:
					return typeof(Bounds);
				case ValueTypes.AnimationCurve:
					return typeof(AnimationCurve);
				case ValueTypes.Object:
					return typeof(UnityEngine.Object);
			}
		}

		public static ValueTypes TypeToEnum(Type type)
		{
			type = type != null && type.IsArray ? type.GetElementType() : type;

			if (type == null)
				return ValueTypes.Null;
			else if (type == typeof(int))
				return ValueTypes.Int;
			else if (type == typeof(float))
				return ValueTypes.Float;
			else if (type == typeof(bool))
				return ValueTypes.Bool;
			else if (type == typeof(string))
				return ValueTypes.String;
			else if (type == typeof(Vector2))
				return ValueTypes.Vector2;
			else if (type == typeof(Vector3))
				return ValueTypes.Vector3;
			else if (type == typeof(Vector4))
				return ValueTypes.Vector4;
			else if (type == typeof(Quaternion))
				return ValueTypes.Quaternion;
			else if (type == typeof(Color))
				return ValueTypes.Color;
			else if (type == typeof(Rect))
				return ValueTypes.Rect;
			else if (type == typeof(Bounds))
				return ValueTypes.Bounds;
			else if (type == typeof(AnimationCurve))
				return ValueTypes.AnimationCurve;
			else if (type == typeof(UnityEngine.Object) || type.IsSubclassOf(typeof(UnityEngine.Object)))
				return ValueTypes.Object;

			Debug.LogError(string.Format("Type {0} is not supported.", type));

			return ValueTypes.Null;
		}

		public static object GetDefaultValue(ValueTypes type, bool isArray)
		{
			object defaultValue;

			switch (type)
			{
				default:
					defaultValue = null;
					break;
				case ValueTypes.Int:
					if (isArray)
						defaultValue = new int[0];
					else
						defaultValue = 0;
					break;
				case ValueTypes.Float:
					if (isArray)
						defaultValue = new float[0];
					else
						defaultValue = 0f;
					break;
				case ValueTypes.Bool:
					if (isArray)
						defaultValue = new bool[0];
					else
						defaultValue = false;
					break;
				case ValueTypes.String:
					if (isArray)
						defaultValue = new string[0];
					else
						defaultValue = string.Empty;
					break;
				case ValueTypes.Vector2:
					if (isArray)
						defaultValue = new Vector2[0];
					else
						defaultValue = Vector2.zero;
					break;
				case ValueTypes.Vector3:
					if (isArray)
						defaultValue = new Vector3[0];
					else
						defaultValue = Vector3.zero;
					break;
				case ValueTypes.Vector4:
					if (isArray)
						defaultValue = new Vector4[0];
					else
						defaultValue = Vector4.zero;
					break;
				case ValueTypes.Quaternion:
					if (isArray)
						defaultValue = new Quaternion[0];
					else
						defaultValue = Quaternion.identity;
					break;
				case ValueTypes.Color:
					if (isArray)
						defaultValue = new Color[0];
					else
						defaultValue = Color.clear;
					break;
				case ValueTypes.Rect:
					if (isArray)
						defaultValue = new Rect[0];
					else
						defaultValue = new Rect();
					break;
				case ValueTypes.Bounds:
					if (isArray)
						defaultValue = new Bounds[0];
					else
						defaultValue = new Bounds();
					break;
				case ValueTypes.AnimationCurve:
					if (isArray)
						defaultValue = new AnimationCurve[0];
					else
						defaultValue = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
					break;
				case ValueTypes.Object:
					if (isArray)
						defaultValue = new UnityEngine.Object[0];
					else
						defaultValue = null;
					break;
			}

			return defaultValue;
		}

		public static byte[] Serialize(ValueTypes type, bool isArray, object value)
		{
			_stream.Position = 0L;

			switch (type)
			{
				case ValueTypes.Int:
					if (isArray)
						_writer.Write((int[])value);
					else
						_writer.Write((int)value);
					break;
				case ValueTypes.Float:
					if (isArray)
						_writer.Write((float[])value);
					else
						_writer.Write((float)value);
					break;
				case ValueTypes.Bool:
					if (isArray)
						_writer.Write((bool[])value);
					else
						_writer.Write((bool)value);
					break;
				case ValueTypes.String:
					if (isArray)
						_writer.Write((string[])value);
					else
						_writer.Write((string)value);
					break;
				case ValueTypes.Vector2:
					if (isArray)
						_writer.Write((Vector2[])value);
					else
						_writer.Write((Vector2)value);
					break;
				case ValueTypes.Vector3:
					if (isArray)
						_writer.Write((Vector3[])value);
					else
						_writer.Write((Vector3)value);
					break;
				case ValueTypes.Vector4:
					if (isArray)
						_writer.Write((Vector4[])value);
					else
						_writer.Write((Vector4)value);
					break;
				case ValueTypes.Quaternion:
					if (isArray)
						_writer.Write((Quaternion[])value);
					else
						_writer.Write((Quaternion)value);
					break;
				case ValueTypes.Color:
					if (isArray)
						_writer.Write((Color[])value);
					else
						_writer.Write((Color)value);
					break;
				case ValueTypes.Rect:
					if (isArray)
						_writer.Write((Rect[])value);
					else
						_writer.Write((Rect)value);
					break;
				case ValueTypes.Bounds:
					if (isArray)
						_writer.Write((Bounds[])value);
					else
						_writer.Write((Bounds)value);
					break;
				case ValueTypes.AnimationCurve:
					if (isArray)
						_writer.Write((AnimationCurve[])value);
					else
						_writer.Write((AnimationCurve)value);
					break;
			}

			return _stream.ToArray();
		}

		public static object Deserialize(ValueTypes type, bool isArray, byte[] bytes)
		{
			if (type == ValueTypes.Null)
				return null;
			else if (bytes == null || bytes.Length == 0)
				return GetDefaultValue(type, isArray);

			_stream.Position = 0L;
			_stream.Write(bytes, 0, bytes.Length);
			_stream.Position = 0L;

			object value = null;

			switch (type)
			{
				case ValueTypes.Int:
					if (isArray)
						value = _reader.ReadInt32Array();
					else
						value = _reader.ReadInt32();
					break;
				case ValueTypes.Float:
					if (isArray)
						value = _reader.ReadSingleArray();
					else
						value = _reader.ReadSingle();
					break;
				case ValueTypes.Bool:
					if (isArray)
						value = _reader.ReadBooleanArray();
					else
						value = _reader.ReadBoolean();
					break;
				case ValueTypes.String:
					if (isArray)
						value = _reader.ReadStringArray();
					else
						value = _reader.ReadString();
					break;
				case ValueTypes.Vector2:
					if (isArray)
						value = _reader.ReadVector2Array();
					else
						value = _reader.ReadVector2();
					break;
				case ValueTypes.Vector3:
					if (isArray)
						value = _reader.ReadVector3Array();
					else
						value = _reader.ReadVector3();
					break;
				case ValueTypes.Vector4:
					if (isArray)
						value = _reader.ReadVector4Array();
					else
						value = _reader.ReadVector4();
					break;
				case ValueTypes.Quaternion:
					if (isArray)
						value = _reader.ReadQuaternionArray();
					else
						value = _reader.ReadQuaternion();
					break;
				case ValueTypes.Color:
					if (isArray)
						value = _reader.ReadColorArray();
					else
						value = _reader.ReadColor();
					break;
				case ValueTypes.Rect:
					if (isArray)
						value = _reader.ReadRectArray();
					else
						value = _reader.ReadRect();
					break;
				case ValueTypes.Bounds:
					if (isArray)
						value = _reader.ReadBoundsArray();
					else
						value = _reader.ReadBounds();
					break;
				case ValueTypes.AnimationCurve:
					if (isArray)
						value = _reader.ReadAnimationCurveArray();
					else
						value = _reader.ReadAnimationCurve();
					break;
			}

			return value;
		}
	}
}