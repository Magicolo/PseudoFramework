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
	[Serializable,Copy]
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

		public static readonly Pool<DynamicValue> Pool = new Pool<DynamicValue>(() => new DynamicValue());
		public static readonly DynamicValue Default = new DynamicValue();

		static readonly object dummy = new object();
		static readonly MemoryStream stream = new MemoryStream();
		static readonly BinaryReader reader = new BinaryReader(stream);
		static readonly BinaryWriter writer = new BinaryWriter(stream);

		public bool IsArray { get { return isArray; } }

		object valueCached;
		object value;
		[SerializeField]
		ValueTypes type;
		[SerializeField, Toggle("Array", "Array")]
		bool isArray;
		[SerializeField, HideInInspector]
		byte[] data;
		[SerializeField]
		UnityEngine.Object[] objectValue;

		public T GetValue<T>()
		{
			return (T)GetValue();
		}

		public object GetValue()
		{
			if (valueCached == null)
			{
				if (type == ValueTypes.Object)
					if (isArray)
						value = objectValue;
					else if (objectValue == null || objectValue.Length == 0)
						value = null;
					else
						value = objectValue[0];
				else
					Deserialize();

				valueCached = dummy;
			}

			return value;
		}

		public void SetValue(object value)
		{
			this.value = value;
			isArray = value is Array;

			if (value == null)
			{
				if (type != ValueTypes.Null || type != ValueTypes.Object)
					this.value = GetDefaultValue(type, isArray);
			}
			else
			{
				if ((isArray && this.value is UnityEngine.Object[]) || (!isArray && this.value is UnityEngine.Object))
					type = ValueTypes.Object;
				else
					type = TypeToEnum(this.value.GetType());
			}

#if UNITY_EDITOR
			if (type == ValueTypes.Object)
			{
				if (isArray)
					objectValue = (UnityEngine.Object[])this.value;
				else
				{
					if (objectValue == null)
						objectValue = new UnityEngine.Object[1];
					else if (objectValue.Length != 1)
						Array.Resize(ref objectValue, 1);

					objectValue[0] = (UnityEngine.Object)this.value;
				}
			}
			else
				Serialize();
#endif
		}

		public ValueTypes GetValueType()
		{
			return type;
		}

		public void SetValueType(ValueTypes type, bool isArray)
		{
			if (this.type == type && this.isArray == isArray)
				return;

			this.type = type;
			this.isArray = isArray;

			SetValue(GetDefaultValue(type, isArray));
		}

		void Serialize()
		{
			data = Serialize(type, isArray, value);
		}

		void Deserialize()
		{
			value = Deserialize(type, isArray, data);
		}

		public void OnCreate()
		{
		}

		public void OnRecycle()
		{
		}

		public void Copy(DynamicValue reference)
		{
			valueCached = reference.valueCached;
			value = reference.value;
			type = reference.type;
			isArray = reference.isArray;
			CopyUtility.CopyTo(reference.data, ref data);
			CopyUtility.CopyTo(reference.objectValue, ref objectValue);
		}

		public override string ToString()
		{
			return string.Format("{0}({1}{2}, {3})", GetType().Name, type, isArray ? "[]" : "", GetValue());
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
			stream.Position = 0L;

			switch (type)
			{
				case ValueTypes.Int:
					if (isArray)
						writer.Write((int[])value);
					else
						writer.Write((int)value);
					break;
				case ValueTypes.Float:
					if (isArray)
						writer.Write((float[])value);
					else
						writer.Write((float)value);
					break;
				case ValueTypes.Bool:
					if (isArray)
						writer.Write((bool[])value);
					else
						writer.Write((bool)value);
					break;
				case ValueTypes.String:
					if (isArray)
						writer.Write((string[])value);
					else
						writer.Write((string)value);
					break;
				case ValueTypes.Vector2:
					if (isArray)
						writer.Write((Vector2[])value);
					else
						writer.Write((Vector2)value);
					break;
				case ValueTypes.Vector3:
					if (isArray)
						writer.Write((Vector3[])value);
					else
						writer.Write((Vector3)value);
					break;
				case ValueTypes.Vector4:
					if (isArray)
						writer.Write((Vector4[])value);
					else
						writer.Write((Vector4)value);
					break;
				case ValueTypes.Quaternion:
					if (isArray)
						writer.Write((Quaternion[])value);
					else
						writer.Write((Quaternion)value);
					break;
				case ValueTypes.Color:
					if (isArray)
						writer.Write((Color[])value);
					else
						writer.Write((Color)value);
					break;
				case ValueTypes.Rect:
					if (isArray)
						writer.Write((Rect[])value);
					else
						writer.Write((Rect)value);
					break;
				case ValueTypes.Bounds:
					if (isArray)
						writer.Write((Bounds[])value);
					else
						writer.Write((Bounds)value);
					break;
				case ValueTypes.AnimationCurve:
					if (isArray)
						writer.Write((AnimationCurve[])value);
					else
						writer.Write((AnimationCurve)value);
					break;
			}

			return stream.ToArray();
		}

		public static object Deserialize(ValueTypes type, bool isArray, byte[] bytes)
		{
			if (type == ValueTypes.Null)
				return null;
			else if (bytes == null || bytes.Length == 0)
				return GetDefaultValue(type, isArray);

			stream.Position = 0L;
			stream.Write(bytes, 0, bytes.Length);
			stream.Position = 0L;

			object value = null;

			switch (type)
			{
				case ValueTypes.Int:
					if (isArray)
						value = reader.ReadInt32Array();
					else
						value = reader.ReadInt32();
					break;
				case ValueTypes.Float:
					if (isArray)
						value = reader.ReadSingleArray();
					else
						value = reader.ReadSingle();
					break;
				case ValueTypes.Bool:
					if (isArray)
						value = reader.ReadBooleanArray();
					else
						value = reader.ReadBoolean();
					break;
				case ValueTypes.String:
					if (isArray)
						value = reader.ReadStringArray();
					else
						value = reader.ReadString();
					break;
				case ValueTypes.Vector2:
					if (isArray)
						value = reader.ReadVector2Array();
					else
						value = reader.ReadVector2();
					break;
				case ValueTypes.Vector3:
					if (isArray)
						value = reader.ReadVector3Array();
					else
						value = reader.ReadVector3();
					break;
				case ValueTypes.Vector4:
					if (isArray)
						value = reader.ReadVector4Array();
					else
						value = reader.ReadVector4();
					break;
				case ValueTypes.Quaternion:
					if (isArray)
						value = reader.ReadQuaternionArray();
					else
						value = reader.ReadQuaternion();
					break;
				case ValueTypes.Color:
					if (isArray)
						value = reader.ReadColorArray();
					else
						value = reader.ReadColor();
					break;
				case ValueTypes.Rect:
					if (isArray)
						value = reader.ReadRectArray();
					else
						value = reader.ReadRect();
					break;
				case ValueTypes.Bounds:
					if (isArray)
						value = reader.ReadBoundsArray();
					else
						value = reader.ReadBounds();
					break;
				case ValueTypes.AnimationCurve:
					if (isArray)
						value = reader.ReadAnimationCurveArray();
					else
						value = reader.ReadAnimationCurve();
					break;
			}

			return value;
		}
	}
}