using UnityEngine;
using System.Collections.Generic;
using System;

namespace Pseudo
{
	[System.Serializable]
	public class ArchitectRotationFlip : IEquatable<ArchitectRotationFlip>, IEquatable<Transform>, ICloneable<ArchitectRotationFlip>
	{
		public float Angle;
		public bool FlipX;
		public bool FlipY;

		public float PositiveNormalisezAngle { get { return (Angle + 360) % 360; } }

		public ArchitectRotationFlip(float angle, bool flipX, bool flipY)
		{
			Angle = angle;
			FlipX = flipX;
			FlipY = flipY;
		}

		public void ApplyTo(Transform transform)
		{
			transform.localRotation = Quaternion.Euler(0, 0, Angle);
			Vector3 s = transform.localScale;
			float flipXFactor = FlipX ? -1 : 1;
			float flipYFactor = FlipY ? -1 : 1;
			transform.localScale = new Vector3(s.x * flipXFactor, s.y * flipYFactor, s.z);
		}

		public ArchitectRotationFlip Clone()
		{
			return new ArchitectRotationFlip(Angle, FlipX, FlipY);
		}

		public bool Equals(Transform transform)
		{
			return Mathf.Approximately(PositiveNormalisezAngle, transform.rotation.eulerAngles.z) && FlipX == transform.localScale.x < 0 && FlipY == transform.localScale.y < 0;
		}

		public bool Equals(ArchitectRotationFlip other)
		{
			return PositiveNormalisezAngle == other.PositiveNormalisezAngle && FlipX == other.FlipX && FlipY == other.FlipY;
		}

		public override string ToString()
		{
			return string.Format("{0}(Angle:{1}, FlipX:{2}, FlipY:{3})", GetType().Name, PositiveNormalisezAngle, FlipX, FlipY);
		}

		public static ArchitectRotationFlip FromTransform(Transform transform)
		{
			bool flipX = transform.localScale.x < 0;
			bool flipY = transform.localScale.y < 0;
			return new ArchitectRotationFlip(transform.localRotation.eulerAngles.z, flipX, flipY);
		}
	}
}
