using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Zones/Disk Zone")]
	public class DiskZone : Zone2DBase
	{
		[SerializeField]
		Disk disk = new Disk(0f, 0f, 0.5f, 1f);

		public Disk LocalDisk { get { return disk; } set { disk = value; } }
		public Disk WorldDisk { get { return new Disk(disk.Position.ToVector3() + transform.position, disk.InnerRadius, disk.OuterRadius); } }

#if UNITY_EDITOR
		[SerializeField]
		bool draw = true;
		[SerializeField]
		Color color = new Color(1f, 0f, 0f, 0.5f);

		void OnDrawGizmos()
		{
			if (!draw || !enabled || !gameObject.activeInHierarchy)
				return;

			var position = transform.position + disk.Position.ToVector3();
			UnityEditor.Handles.color = color;
			UnityEditor.Handles.DrawWireDisc(position, Vector3.back, disk.OuterRadius);
			UnityEditor.Handles.color = color.SetValues(color.a / 4f, Channels.A);
			UnityEditor.Handles.DrawSolidDisc(position, Vector3.back, disk.OuterRadius);

			UnityEditor.Handles.color = color.HueShift(0.5f);
			UnityEditor.Handles.DrawWireDisc(position, Vector3.back, disk.InnerRadius);
			UnityEditor.Handles.color = color.HueShift(0.5f).SetValues(color.a / 4f, Channels.A);
			UnityEditor.Handles.DrawSolidDisc(position, Vector3.back, disk.InnerRadius);
		}
#endif

		public override bool Contains(Vector3 point)
		{
			return WorldDisk.Contains(point);
		}

		public override bool Contains(Rect rect)
		{
			return rect.IsContained(WorldDisk);
		}

		public override bool Contains(IShape2D shape)
		{
			return shape.IsContained(WorldDisk);
		}

		public override bool Contains(Zone2DBase zone)
		{
			return zone.IsContained(WorldDisk);
		}

		public override bool IsContained(Rect rect)
		{
			return rect.Contains(WorldDisk);
		}

		public override bool IsContained(IShape2D shape)
		{
			return shape.Contains(WorldDisk);
		}

		public override bool IsContained(Zone2DBase zone)
		{
			return zone.Contains(WorldDisk);
		}

		public override bool Overlaps(Rect rect)
		{
			return rect.Overlaps(WorldDisk);
		}

		public override bool Overlaps(IShape2D shape)
		{
			return shape.Overlaps(WorldDisk);
		}

		public override bool Overlaps(Zone2DBase zone)
		{
			return zone.Overlaps(WorldDisk);
		}

		public override Vector2 GetRandomLocalPoint()
		{
			return LocalDisk.GetRandomPoint();
		}

		public override Vector3 GetRandomWorldPoint()
		{
			Vector3 randomPosition = WorldDisk.GetRandomPoint();
			randomPosition.z = transform.position.z;

			return randomPosition;
		}
	}
}