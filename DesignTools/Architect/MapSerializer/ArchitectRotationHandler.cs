using UnityEngine;
using System.Collections.Generic;

namespace Pseudo
{
	class ArchitectRotationHandler
	{
		public const ulong FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
		public const ulong FLIPPED_VERTICALLY_FLAG = 0x40000000;
		public const ulong FLIPPED_DIAGONALLY_FLAG = 0x20000000;

		[System.Flags]
		public enum RotationFlag
		{
			Rotation90 = 1 << 31,
			Rotation180 = 1 << 30,
			FlipX = 1 << 29,
			FlipY = 1 << 28
		};

		public const ulong ROTATION_90_FLAG = 0x20000000 + 0x80000000;
		public const ulong ROTATION_180_FLAG = 0x40000000 + 0x80000000;
		public const ulong ROTATION_270_FLAG = 0x20000000 + 0x40000000;


		public static RotationFlag getRotationFlipFlags(Transform transform)
		{
			RotationFlag flags = 0;
			if (transform.localScale.x < 0)
				flags |= RotationFlag.FlipX;
			if (transform.localScale.y < 0)
				flags |= RotationFlag.FlipY;
			if (transform.localRotation.eulerAngles.z == 90f || transform.localRotation.eulerAngles.z == 270f)
				flags |= RotationFlag.Rotation90;
			else if (transform.localRotation.eulerAngles.z == 180f || transform.localRotation.eulerAngles.z == 270f)
				flags |= RotationFlag.Rotation180;
			return flags;
		}

		public static void ApplyFlipFlags(Transform transform, ulong flags)
		{
			if (flags == 0) return;
			bool horizontal = (flags & FLIPPED_HORIZONTALLY_FLAG) == FLIPPED_HORIZONTALLY_FLAG;
			bool vertical = (flags & FLIPPED_VERTICALLY_FLAG) == FLIPPED_VERTICALLY_FLAG;
			bool diagonal = (flags & FLIPPED_DIAGONALLY_FLAG) == FLIPPED_DIAGONALLY_FLAG;

			ApplyFlipFlags(transform, horizontal, vertical, diagonal);
		}

		public static void ApplyFlipFlags(Transform transform, int flags)
		{
			if (flags == 0) return;
			bool horizontal = (flags & (int)RotationFlag.FlipX) != 0;
			bool vertical = (flags & (int)RotationFlag.FlipY) != 0;
			bool rot90 = (flags & (int)RotationFlag.Rotation90) != 0;
			bool rot180 = (flags & (int)RotationFlag.Rotation180) != 0;
			float rotation = (rot90 ? 90 : 0) + (rot180 ? 180 : 0);
			ApplyRotationFlip(transform, rotation, horizontal, vertical);
		}

		public static void ApplyFlipFlags(Transform transform, bool horizontal, bool vertical, bool diagonal)
		{
			if (!horizontal & !vertical & diagonal) applyRotationFlip(transform, 90, new Vector3(-1, 1, 1));
			if (!horizontal & vertical & !diagonal) applyRotationFlip(transform, 180, new Vector3(-1, 1, 1));
			if (!horizontal & vertical & diagonal) applyRotationFlip(transform, 90, new Vector3(1, 1, 1));
			if (horizontal & !vertical & !diagonal) applyRotationFlip(transform, 0, new Vector3(-1, 1, 1));
			if (horizontal & !vertical & diagonal) applyRotationFlip(transform, 270, new Vector3(1, 1, 1));
			if (horizontal & vertical & !diagonal) applyRotationFlip(transform, 180, new Vector3(1, 1, 1));
			if (horizontal & vertical & diagonal) applyRotationFlip(transform, 270, new Vector3(-1, 1, 1));
		}

		static void applyRotationFlip(Transform transform, int rotation, Vector3 flip)
		{
			transform.Rotate(0, 0, rotation);
			Vector3 s = transform.localScale;
			transform.localScale = new Vector3(s.x * flip.x, s.y * flip.y, s.z * flip.z);
		}

		public static void ApplyRotationFlip(Transform transform, float rotation, bool flipX, bool flipY)
		{
			transform.Rotate(0, 0, rotation);
			Vector3 s = transform.localScale;
			float flipXFactor = flipX ? -1 : 1;
			float flipYFactor = flipY ? -1 : 1;
			transform.localScale = new Vector3(s.x * flipXFactor, s.y * flipYFactor, s.z);
		}

		public static int RemoveRotationFlags(int id)
		{
			return (id << 4) >> 4;
		}

		public static int GetRotationFlags(int id)
		{
			return (id >> 28) << 28;
		}
	}
}
