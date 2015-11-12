using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using System.IO;

public static class BinaryExtensions
{
	public static void Write(this BinaryWriter writer, int[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, float[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, double[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, long[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, bool[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, char[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, string[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, Vector2 value)
	{
		writer.Write(value.x);
		writer.Write(value.y);
	}

	public static void Write(this BinaryWriter writer, Vector2[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, Vector3 value)
	{
		writer.Write(value.x);
		writer.Write(value.y);
		writer.Write(value.z);
	}

	public static void Write(this BinaryWriter writer, Vector3[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, Vector4 value)
	{
		writer.Write(value.x);
		writer.Write(value.y);
		writer.Write(value.z);
		writer.Write(value.w);
	}

	public static void Write(this BinaryWriter writer, Vector4[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, Quaternion value)
	{
		writer.Write(value.x);
		writer.Write(value.y);
		writer.Write(value.z);
		writer.Write(value.w);
	}

	public static void Write(this BinaryWriter writer, Quaternion[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, Color value)
	{
		writer.Write(value.r);
		writer.Write(value.g);
		writer.Write(value.b);
		writer.Write(value.a);
	}

	public static void Write(this BinaryWriter writer, Color[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, Rect value)
	{
		writer.Write(value.xMin);
		writer.Write(value.yMin);
		writer.Write(value.width);
		writer.Write(value.height);
	}

	public static void Write(this BinaryWriter writer, Rect[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, Bounds value)
	{
		writer.Write(value.center);
		writer.Write(value.size);
	}

	public static void Write(this BinaryWriter writer, Bounds[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, AnimationCurve value)
	{
		writer.Write(value.keys);
		writer.Write((int)value.preWrapMode);
		writer.Write((int)value.postWrapMode);
	}

	public static void Write(this BinaryWriter writer, AnimationCurve[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static void Write(this BinaryWriter writer, Keyframe value)
	{
		writer.Write(value.time);
		writer.Write(value.value);
		writer.Write(value.inTangent);
		writer.Write(value.outTangent);
	}

	public static void Write(this BinaryWriter writer, Keyframe[] value)
	{
		writer.Write(value.Length);

		for (int i = 0; i < value.Length; i++)
			writer.Write(value[i]);
	}

	public static int[] ReadInt32Array(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		int[] value = new int[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadInt32();

		return value;
	}

	public static float[] ReadSingleArray(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		float[] value = new float[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadSingle();

		return value;
	}

	public static double[] ReadDoubleArray(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		double[] value = new double[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadDouble();

		return value;
	}

	public static long[] ReadInt64Array(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		long[] value = new long[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadInt64();

		return value;
	}

	public static bool[] ReadBooleanArray(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		bool[] value = new bool[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadBoolean();

		return value;
	}

	public static char[] ReadCharArray(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		char[] value = new char[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadChar();

		return value;
	}

	public static string[] ReadStringArray(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		string[] value = new string[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadString();

		return value;
	}

	public static Vector2 ReadVector2(this BinaryReader reader)
	{
		return new Vector2(reader.ReadSingle(), reader.ReadSingle());
	}

	public static Vector2[] ReadVector2Array(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		Vector2[] value = new Vector2[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadVector2();

		return value;
	}

	public static Vector3 ReadVector3(this BinaryReader reader)
	{
		return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
	}

	public static Vector3[] ReadVector3Array(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		Vector3[] value = new Vector3[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadVector3();

		return value;
	}

	public static Vector4 ReadVector4(this BinaryReader reader)
	{
		return new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
	}

	public static Vector4[] ReadVector4Array(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		Vector4[] value = new Vector4[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadVector4();

		return value;
	}

	public static Quaternion ReadQuaternion(this BinaryReader reader)
	{
		return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
	}

	public static Quaternion[] ReadQuaternionArray(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		Quaternion[] value = new Quaternion[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadQuaternion();

		return value;
	}

	public static Color ReadColor(this BinaryReader reader)
	{
		return new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
	}

	public static Color[] ReadColorArray(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		Color[] value = new Color[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadColor();

		return value;
	}

	public static Rect ReadRect(this BinaryReader reader)
	{
		return new Rect(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
	}

	public static Rect[] ReadRectArray(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		Rect[] value = new Rect[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadRect();

		return value;
	}

	public static Bounds ReadBounds(this BinaryReader reader)
	{
		return new Bounds(reader.ReadVector3(), reader.ReadVector3());
	}

	public static Bounds[] ReadBoundsArray(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		Bounds[] value = new Bounds[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadBounds();

		return value;
	}

	public static AnimationCurve ReadAnimationCurve(this BinaryReader reader)
	{
		AnimationCurve value = new AnimationCurve(reader.ReadKeyframeArray());
		value.preWrapMode = (WrapMode)reader.ReadInt32();
		value.postWrapMode = (WrapMode)reader.ReadInt32();

		return value;
	}

	public static AnimationCurve[] ReadAnimationCurveArray(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		AnimationCurve[] value = new AnimationCurve[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadAnimationCurve();

		return value;
	}

	public static Keyframe ReadKeyframe(this BinaryReader reader)
	{
		return new Keyframe(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
	}

	public static Keyframe[] ReadKeyframeArray(this BinaryReader reader)
	{
		int count = reader.ReadInt32();
		Keyframe[] value = new Keyframe[count];

		for (int i = 0; i < count; i++)
			value[i] = reader.ReadKeyframe();

		return value;
	}
}
