using UnityEngine;
using System.Collections;

public class DynamicValueDrawerDummy : ScriptableObject
{
	public bool Bool;
	public int Int;
	public float Float;
	public char Char;
	public string String;
	public Vector2 Vector2 = Vector2.zero;
	public Vector3 Vector3 = Vector3.zero;
	public Vector4 Vector4 = Vector4.zero;
	public Color Color = Color.white;
	public Quaternion Quaternion = Quaternion.identity;
	public Rect Rect = new Rect();
	public Bounds Bounds = new Bounds();
	public AnimationCurve AnimationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
	public Object Object;
	public bool[] BoolArray = { };
	public int[] IntArray = { };
	public float[] FloatArray = { };
	public char[] CharArray = { };
	public string[] StringArray = { };
	public Vector2[] Vector2Array = { };
	public Vector3[] Vector3Array = { };
	public Vector4[] Vector4Array = { };
	public Color[] ColorArray = { };
	public Quaternion[] QuaternionArray = { };
	public Rect[] RectArray = { };
	public Bounds[] BoundsArray = { };
	public AnimationCurve[] AnimationCurveArray = { };
	public Object[] ObjectArray = { };
}
