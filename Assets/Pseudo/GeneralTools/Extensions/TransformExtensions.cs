using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class TransformExtensions
	{
		#region Position
		public static void SetPosition(this Transform transform, Vector3 position, Axes axes = Axes.XYZ)
		{
			transform.position = transform.position.SetValues(position, axes);
		}

		public static void SetPosition(this Transform transform, float position, Axes axes = Axes.XYZ)
		{
			transform.SetPosition(new Vector3(position, position, position), axes);
		}

		public static void Translate(this Transform transform, Vector3 translation, Axes axes = Axes.XYZ)
		{
			transform.SetPosition(transform.position + translation, axes);
		}

		public static void Translate(this Transform transform, float translation, Axes axes = Axes.XYZ)
		{
			transform.Translate(new Vector3(translation, translation, translation), axes);
		}

		public static void TranslateTowards(this Transform transform, Vector3 targetPosition, float acceleration, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XYZ)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					transform.SetPosition(transform.position.Lerp(targetPosition, acceleration, axes), axes);
					break;
				case InterpolationModes.Linear:
					transform.SetPosition(transform.position.LerpLinear(targetPosition, acceleration, axes), axes);
					break;
			}
		}

		public static void TranslateTowards(this Transform transform, float targetPosition, float acceleration, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XYZ)
		{
			transform.TranslateTowards(new Vector3(targetPosition, targetPosition, targetPosition), acceleration, interpolation, axes);
		}

		public static void OscillatePosition(this Transform transform, Vector3 frequency, Vector3 amplitude, Vector3 center, float time, Axes axes = Axes.XYZ)
		{
			transform.SetPosition(transform.position.Oscillate(frequency, amplitude, center, time, transform.GetInstanceID() / 1000, axes), axes);
		}

		public static void OscillatePosition(this Transform transform, float frequency, float amplitude, float center, float time, Axes axes = Axes.XYZ)
		{
			transform.OscillatePosition(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), time, axes);
		}

		public static void SetLocalPosition(this Transform transform, Vector3 position, Axes axes = Axes.XYZ)
		{
			transform.localPosition = transform.localPosition.SetValues(position, axes);
		}

		public static void SetLocalPosition(this Transform transform, float position, Axes axes = Axes.XYZ)
		{
			transform.SetLocalPosition(new Vector3(position, position, position), axes);
		}

		public static void TranslateLocal(this Transform transform, Vector3 translation, Axes axes = Axes.XYZ)
		{
			transform.SetLocalPosition(transform.localPosition + translation, axes);
		}

		public static void TranslateLocal(this Transform transform, float translation, Axes axes = Axes.XYZ)
		{
			transform.TranslateLocal(new Vector3(translation, translation, translation), axes);
		}

		public static void TranslateLocalTowards(this Transform transform, Vector3 targetPosition, float acceleration, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XYZ)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					transform.SetLocalPosition(transform.localPosition.Lerp(targetPosition, acceleration, axes), axes);
					break;
				case InterpolationModes.Linear:
					transform.SetLocalPosition(transform.localPosition.LerpLinear(targetPosition, acceleration, axes), axes);
					break;
			}
		}

		public static void TranslateLocalTowards(this Transform transform, float targetPosition, float acceleration, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XYZ)
		{
			transform.TranslateLocalTowards(new Vector3(targetPosition, targetPosition, targetPosition), acceleration, interpolation, axes);
		}

		public static void OscillateLocalPosition(this Transform transform, Vector3 frequency, Vector3 amplitude, Vector3 center, float time, Axes axes = Axes.XYZ)
		{
			transform.SetLocalPosition(transform.localPosition.Oscillate(frequency, amplitude, center, time, transform.GetInstanceID() / 1000f, axes), axes);
		}

		public static void OscillateLocalPosition(this Transform transform, float frequency, float amplitude, float center, float time, Axes axes = Axes.XYZ)
		{
			transform.OscillateLocalPosition(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), time, axes);
		}
		#endregion

		#region Rotation
		public static void SetEulerAngles(this Transform transform, Vector3 angles, Axes axes = Axes.XYZ)
		{
			transform.eulerAngles = transform.eulerAngles.SetValues(angles, axes);
		}

		public static void SetEulerAngles(this Transform transform, float angle, Axes axes = Axes.XYZ)
		{
			transform.SetEulerAngles(new Vector3(angle, angle, angle), axes);
		}

		public static void Rotate(this Transform transform, Vector3 rotation, Axes axes = Axes.XYZ)
		{
			transform.SetEulerAngles(transform.eulerAngles + rotation, axes);
		}

		public static void Rotate(this Transform transform, float rotation, Axes axes = Axes.XYZ)
		{
			transform.Rotate(new Vector3(rotation, rotation, rotation), axes);
		}

		public static void RotateTowards(this Transform transform, Vector3 targetAngles, float deltaTime, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XYZ)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					transform.SetEulerAngles(transform.eulerAngles.LerpAngles(targetAngles, deltaTime, axes), axes);
					break;
				case InterpolationModes.Linear:
					transform.SetEulerAngles(transform.eulerAngles.LerpAnglesLinear(targetAngles, deltaTime, axes), axes);
					break;
			}
		}

		public static void RotateTowards(this Transform transform, float targetAngle, float deltaTime, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XYZ)
		{
			transform.RotateTowards(new Vector3(targetAngle, targetAngle, targetAngle), deltaTime, interpolation, axes);
		}

		public static void OscillateEulerAngles(this Transform transform, Vector3 frequency, Vector3 amplitude, Vector3 center, float time, Axes axes = Axes.XYZ)
		{
			transform.SetEulerAngles(transform.eulerAngles.Oscillate(frequency, amplitude, center, time, transform.GetInstanceID() / 1000, axes), axes);
		}

		public static void OscillateEulerAngles(this Transform transform, float frequency, float amplitude, float center, float time, Axes axes = Axes.XYZ)
		{
			transform.OscillateEulerAngles(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), time, axes);
		}

		public static void SetLocalEulerAngles(this Transform transform, Vector3 angles, Axes axes = Axes.XYZ)
		{
			transform.localEulerAngles = transform.localEulerAngles.SetValues(angles, axes);
		}

		public static void SetLocalEulerAngles(this Transform transform, float angle, Axes axes = Axes.XYZ)
		{
			transform.SetLocalEulerAngles(new Vector3(angle, angle, angle), axes);
		}

		public static void RotateLocal(this Transform transform, Vector3 rotation, Axes axes = Axes.XYZ)
		{
			transform.SetLocalEulerAngles(transform.localEulerAngles + rotation, axes);
		}

		public static void RotateLocal(this Transform transform, float rotation, Axes axes = Axes.XYZ)
		{
			transform.RotateLocal(new Vector3(rotation, rotation, rotation), axes);
		}

		public static void RotateLocalTowards(this Transform transform, Vector3 targetAngles, float deltaTime, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XYZ)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					transform.SetLocalEulerAngles(transform.localEulerAngles.LerpAngles(targetAngles, deltaTime, axes), axes);
					break;
				case InterpolationModes.Linear:
					transform.SetLocalEulerAngles(transform.localEulerAngles.LerpAnglesLinear(targetAngles, deltaTime, axes), axes);
					break;
			}
		}

		public static void RotateLocalTowards(this Transform transform, float targetAngle, float deltaTime, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XYZ)
		{
			transform.RotateLocalTowards(new Vector3(targetAngle, targetAngle, targetAngle), deltaTime, interpolation, axes);
		}

		public static void OscillateLocalEulerAngles(this Transform transform, Vector3 frequency, Vector3 amplitude, Vector3 center, float time, Axes axes = Axes.XYZ)
		{
			transform.SetLocalEulerAngles(transform.localEulerAngles.Oscillate(frequency, amplitude, center, time, transform.GetInstanceID() / 1000, axes), axes);
		}

		public static void OscillateLocalEulerAngles(this Transform transform, float frequency, float amplitude, float center, float time, Axes axes = Axes.XYZ)
		{
			transform.OscillateLocalEulerAngles(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), time, axes);
		}
		#endregion

		#region Scale
		public static void SetScale(this Transform transform, Vector3 scale, Axes axes = Axes.XYZ)
		{
			transform.localScale = transform.localScale.SetValues(transform.localScale.Div(transform.lossyScale, axes).Mult(scale, axes), axes);
		}

		public static void SetScale(this Transform transform, float scale, Axes axes = Axes.XYZ)
		{
			transform.SetScale(new Vector3(scale, scale, scale), axes);
		}

		public static void Scale(this Transform transform, Vector3 scale, Axes axes = Axes.XYZ)
		{
			transform.SetScale(transform.localScale + scale, axes);
		}

		public static void Scale(this Transform transform, float scale, Axes axes = Axes.XYZ)
		{
			transform.SetScale(new Vector3(scale, scale, scale), axes);
		}

		public static void ScaleTowards(this Transform transform, Vector3 targetScale, float acceleration, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XYZ)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					transform.SetScale(transform.lossyScale.Lerp(targetScale, acceleration, axes), axes);
					break;
				case InterpolationModes.Linear:
					transform.SetScale(transform.lossyScale.LerpLinear(targetScale, acceleration, axes), axes);
					break;
			}
		}

		public static void ScaleTowards(this Transform transform, float targetScale, float acceleration, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XYZ)
		{
			transform.ScaleTowards(new Vector3(targetScale, targetScale, targetScale), acceleration, interpolation, axes);
		}

		public static void OscillateScale(this Transform transform, Vector3 frequency, Vector3 amplitude, Vector3 center, float time, Axes axes = Axes.XYZ)
		{
			transform.SetScale(transform.lossyScale.Oscillate(frequency, amplitude, center, time, transform.GetInstanceID() / 1000, axes), axes);
		}

		public static void OscillateScale(this Transform transform, float frequency, float amplitude, float center, float time, Axes axes = Axes.XYZ)
		{
			transform.OscillateScale(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), time, axes);
		}

		public static void FlipScale(this Transform transform, Axes axes = Axes.XYZ)
		{
			transform.SetScale(transform.lossyScale.SetValues(-transform.lossyScale, axes), axes);
		}

		public static void SetLocalScale(this Transform transform, Vector3 scale, Axes axes = Axes.XYZ)
		{
			transform.localScale = transform.localScale.SetValues(scale, axes);
		}

		public static void SetLocalScale(this Transform transform, float scale, Axes axes = Axes.XYZ)
		{
			transform.SetLocalScale(new Vector3(scale, scale, scale), axes);
		}

		public static void ScaleLocal(this Transform transform, Vector3 scale, Axes axes = Axes.XYZ)
		{
			transform.SetLocalScale(transform.localScale + scale, axes);
		}

		public static void ScaleLocal(this Transform transform, float scale, Axes axes = Axes.XYZ)
		{
			transform.ScaleLocal(new Vector3(scale, scale, scale), axes);
		}

		public static void ScaleLocalTowards(this Transform transform, Vector3 targetScale, float acceleration, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XYZ)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					transform.SetLocalScale(transform.localScale.Lerp(targetScale, acceleration, axes), axes);
					break;
				case InterpolationModes.Linear:
					transform.SetLocalScale(transform.localScale.LerpLinear(targetScale, acceleration, axes), axes);
					break;
			}
		}

		public static void ScaleLocalTowards(this Transform transform, float targetScale, float acceleration, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XYZ)
		{
			transform.ScaleLocalTowards(new Vector3(targetScale, targetScale, targetScale), acceleration, interpolation, axes);
		}

		public static void OscillateLocalScale(this Transform transform, Vector3 frequency, Vector3 amplitude, Vector3 center, float time, Axes axes = Axes.XYZ)
		{
			transform.SetLocalScale(transform.localScale.Oscillate(frequency, amplitude, center, time, transform.GetInstanceID() / 1000, axes), axes);
		}

		public static void OscillateLocalScale(this Transform transform, float frequency, float amplitude, float center, float time, Axes axes = Axes.XYZ)
		{
			transform.OscillateLocalScale(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), time, axes);
		}

		public static void FlipLocalScale(this Transform transform, Axes axes = Axes.XYZ)
		{
			transform.SetLocalScale(transform.localScale.SetValues(-transform.localScale, axes), axes);
		}
		#endregion

		public static Transform[] GetParents(this Transform child)
		{
			List<Transform> parents = new List<Transform>();

			Transform parent = child.parent;

			while (parent != null)
			{
				parents.Add(parent);
				parent = parent.parent;
			}

			return parents.ToArray();
		}

		public static Transform[] GetChildren(this Transform parent)
		{
			Transform[] children = new Transform[parent.childCount];

			for (int i = 0; i < parent.childCount; i++)
				children[i] = parent.GetChild(i);

			return children;
		}

		public static Transform[] GetChildrenRecursive(this Transform parent)
		{
			List<Transform> childrenRecursive = new List<Transform>();
			Transform[] children = parent.GetChildren();

			for (int i = 0; i < children.Length; i++)
			{
				Transform child = children[i];
				childrenRecursive.Add(child);

				if (child.childCount > 0)
					childrenRecursive.AddRange(child.GetChildrenRecursive());
			}

			return childrenRecursive.ToArray();
		}

		public static Transform FindChild(this Transform parent, string childName)
		{
			return parent.FindChild(child => child.name == childName);
		}

		public static Transform FindChild(this Transform parent, Predicate<Transform> predicate)
		{
			Transform[] children = parent.GetChildren();

			for (int i = 0; i < children.Length; i++)
			{
				Transform child = children[i];

				if (predicate(child))
					return child;
			}

			return null;
		}

		public static Transform FindChildRecursive(this Transform parent, string childName)
		{
			return parent.FindChildRecursive(child => child.name == childName);
		}

		public static Transform FindChildRecursive(this Transform parent, Predicate<Transform> predicate)
		{
			Transform[] children = parent.GetChildrenRecursive();

			for (int i = 0; i < children.Length; i++)
			{
				Transform child = children[i];

				if (predicate(child))
					return child;
			}

			return null;
		}

		public static Transform[] FindChildren(this Transform parent, string childName)
		{
			return parent.FindChildren(child => child.name == childName);
		}

		public static Transform[] FindChildren(this Transform parent, Predicate<Transform> predicate)
		{
			List<Transform> validChildren = new List<Transform>();
			Transform[] children = parent.GetChildren();

			for (int i = 0; i < children.Length; i++)
			{
				Transform child = children[i];

				if (predicate(child))
					validChildren.Add(child);
			}

			return validChildren.ToArray();
		}

		public static Transform[] FindChildrenRecursive(this Transform parent, string childName)
		{
			return parent.FindChildrenRecursive(child => child.name == childName);
		}

		public static Transform[] FindChildrenRecursive(this Transform parent, Predicate<Transform> predicate)
		{
			List<Transform> validChildren = new List<Transform>();
			Transform[] children = parent.GetChildrenRecursive();

			for (int i = 0; i < children.Length; i++)
			{
				Transform child = children[i];

				if (predicate(child))
					validChildren.Add(child);
			}

			return validChildren.ToArray();
		}

		public static Transform AddChild(this Transform parent, string childName, PrimitiveType primitiveType)
		{
			GameObject child = GameObject.CreatePrimitive(primitiveType);

			child.name = childName;
			child.transform.parent = parent;
			child.transform.Reset();

			return child.transform;
		}

		public static Transform AddChild(this Transform parent, string childName)
		{
			GameObject child = new GameObject(childName);

			child.transform.parent = parent;
			child.transform.Reset();

			return child.transform;
		}

		public static Transform FindOrAddChild(this Transform parent, string childName, PrimitiveType primitiveType)
		{
			Transform child = parent.FindChild(childName);

			if (child == null)
				child = parent.AddChild(childName, primitiveType);

			return child;
		}

		public static Transform FindOrAddChild(this Transform parent, string childName)
		{
			Transform child = parent.FindChild(childName);

			if (child == null)
				child = parent.AddChild(childName);

			return child;
		}

		public static void SortChildren(this Transform parent)
		{
			Transform[] children = parent.GetChildren();
			string[] childrendNames = new string[children.Length];

			for (int i = 0; i < children.Length; i++)
				childrendNames[i] = children[i].name;

			Array.Sort(childrendNames, children);

			for (int i = 0; i < children.Length; i++)
			{
				Transform child = children[i];

				child.parent = null;
				child.parent = parent;
			}
		}

		public static void SortChildrenRecursive(this Transform parent)
		{
			Transform[] children = parent.GetChildren();
			parent.SortChildren();

			for (int i = 0; i < children.Length; i++)
			{
				Transform child = children[i];

				if (child.childCount > 0)
					child.SortChildrenRecursive();
			}
		}

		public static void SetChildrenActive(this Transform parent, bool value)
		{
			Transform[] children = parent.GetChildren();

			for (int i = 0; i < children.Length; i++)
				children[i].gameObject.SetActive(value);
		}

		public static void DestroyChildren(this Transform parent)
		{
			Transform[] children = parent.GetChildren();

			for (int i = 0; i < children.Length; i++)
				children[i].gameObject.Destroy();
		}

		public static void Reset(this Transform transform)
		{
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}
	}
}
