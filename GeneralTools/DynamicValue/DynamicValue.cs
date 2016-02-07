using Pseudo;
using Pseudo.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Pseudo
{
	[Serializable]
	public class DynamicValue : ICopyable, ISerializationCallbackReceiver
	{
		public enum ValueTypes : byte
		{
			Null = 0,
			Bool = 1,
			Int = 2,
			Float = 3,
			Char = 4,
			String = 5,
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

		static readonly MemoryStream stream = new MemoryStream();
		static readonly BinaryReader reader = new BinaryReader(stream);
		static readonly BinaryWriter writer = new BinaryWriter(stream);

		public bool IsArray { get { return isArray; } }

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
			return value;
		}

		public void SetValue(object value)
		{
			this.value = value;
			isArray = value is Array;

			if (value == null)
			{
				if (type != ValueTypes.Null || type != ValueTypes.Object)
					value = GetDefaultValue(type, isArray);
			}
			else
			{
				if (value is UnityEngine.Object[] || value is UnityEngine.Object)
					type = ValueTypes.Object;
				else if (isArray)
					type = (ValueTypes)BinaryUtility.ToTypeCode(value.GetType().GetElementType());
				else
					type = (ValueTypes)BinaryUtility.ToTypeCode(value.GetType());
			}
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

		public virtual void Copy(object reference)
		{
			var castedReference = (DynamicValue)reference;
			value = castedReference.value;
			type = castedReference.type;
			isArray = castedReference.isArray;
			CopyUtility.CopyTo(castedReference.data, ref data);
			CopyUtility.CopyTo(castedReference.objectValue, ref objectValue);
		}

		public override string ToString()
		{
			return string.Format("{0}({1}{2}, {3})", GetType().Name, type, isArray ? "[]" : "", PDebug.ToString(GetValue()));
		}

		public static object GetDefaultValue(ValueTypes type, bool isArray)
		{
			object defaultValue = null;

			switch (type)
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

		public static object GetDefaultValue<T>(bool isArray)
		{
			if (isArray)
				return new T[0];
			else if (typeof(T).IsValueType)
				return FormatterServices.GetUninitializedObject(typeof(T));
			else
				return Activator.CreateInstance<T>();
		}

		public static byte[] Serialize(object value, bool isArray)
		{
			stream.Position = 0L;
			stream.SetLength(1L);

			if (isArray)
				writer.Write((Array)value);
			else
				writer.Write(value);

			return stream.ToArray();
		}

		public static object Deserialize(byte[] bytes, bool isArray)
		{
			stream.Position = 0L;
			stream.Write(bytes, 0, bytes.Length);
			stream.Position = 0L;

			object value = null;

			try
			{
				if (isArray)
					value = reader.ReadArray();
				else
					value = reader.ReadObject();
			}
			catch { }

			return value;
		}

		public void OnBeforeSerialize()
		{
			if (type == ValueTypes.Object)
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
				data = Serialize(value, isArray);
		}

		public void OnAfterDeserialize()
		{
			if (type == ValueTypes.Object)
			{
				if (isArray)
					value = objectValue;
				else if (objectValue != null && objectValue.Length > 0)
					value = objectValue[0];
			}
			else
				value = Deserialize(data, isArray);
		}
	}
}