using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public abstract class ComponentBase : IComponentOld, IPoolable
	{
		public IEntityOld Entity { get; set; }
		public bool Active
		{
			get { return active; }
			set { active = value; }
		}

		[SerializeField, HideInInspector]
		bool active = true;

		public virtual void OnCreate() { }

		public virtual void OnRecycle() { }
	}
}