using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Tests
{
	[Copy]
	public class PoolableTest : PMonoBehaviour, ICopyable<PoolableTest>
	{
		public float Float;
		[CopyTo]
		public CircleZone Zone;

		public override void OnCreate()
		{
			base.OnCreate();

			Float = PRandom.Range(1f, 100f);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			PDebug.LogMethod(this, GetHashCode(), Float, Zone.LocalCircle);
		}

		public void Copy(PoolableTest reference)
		{
			Float = reference.Float;
			CopyUtility.CopyTo(reference.Zone, ref Zone);
		}
	}
}