using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class EntityBase : PMonoBehaviour, IPoolable, ICopyable<EntityBase>
	{
		public LocalTimeChannel TimeSettings;

		public void OnCreate()
		{
			TimeSettings = Pool<LocalTimeChannel>.Create(TimeSettings);
		}

		public void OnRecycle()
		{
			Pool<LocalTimeChannel>.Recycle(ref TimeSettings);
		}

		public void Copy(EntityBase reference)
		{
			TimeSettings = reference.TimeSettings;
		}
	}
}