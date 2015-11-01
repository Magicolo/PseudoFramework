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
		public virtual void OnCreate() { }

		public virtual void OnRecycle() { }

		public void Copy(EntityBase reference) { }
	}
}