using UnityEngine;
using System.Collections;

public class PDynamicValueDrawerDummy : ScriptableObject
{
	public int Int = 0;
	public float Float = 0f;
	public bool Bool = false;
	public char Char = '\0';
	public string String = string.Empty;
	public LayerMask LayerMask = new LayerMask();
	public Vector2 Vector2 = Vector2.zero;
	public Vector3 Vector3 = Vector3.zero;
	public Vector4 Vector4 = Vector4.zero;
	public Quaternion Quaternion = Quaternion.identity;
	public Color Color = Color.white;
	public Rect Rect = new Rect();
	public Bounds Bounds = new Bounds();
	public AnimationCurve AnimationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
	public Object Object;
	public int[] IntArray = { };
	public float[] FloatArray = { };
	public bool[] BoolArray = { };
	public char[] CharArray = { };
	public string[] StringArray = { };
	public LayerMask[] LayerMaskArray = { };
	public Vector2[] Vector2Array = { };
	public Vector3[] Vector3Array = { };
	public Vector4[] Vector4Array = { };
	public Quaternion[] QuaternionArray = { };
	public Color[] ColorArray = { };
	public Rect[] RectArray = { };
	public Bounds[] BoundsArray = { };
	public AnimationCurve[] AnimationCurveArray = { };
	public Object[] ObjectArray = { };
}
