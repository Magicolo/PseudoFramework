using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public abstract class Zone2DBase : PMonoBehaviour
	{
		public abstract bool Contains(Vector3 point);
		public abstract bool Contains(Rect rect);
		public abstract bool Contains(IShape2D shape);
		public abstract bool Contains(Zone2DBase zone);
		public abstract bool IsContained(Rect rect);
		public abstract bool IsContained(IShape2D shape);
		public abstract bool IsContained(Zone2DBase zone);
		public abstract bool Overlaps(Rect rect);
		public abstract bool Overlaps(IShape2D shape);
		public abstract bool Overlaps(Zone2DBase zone);
		public abstract Vector2 GetRandomLocalPoint();
		public abstract Vector3 GetRandomWorldPoint();
	}
}