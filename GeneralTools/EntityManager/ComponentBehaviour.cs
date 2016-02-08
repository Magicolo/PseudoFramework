using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[RequireComponent(typeof(EntityBehaviour))]
	public abstract class ComponentBehaviour : PMonoBehaviour, IComponent, ICopyable<ComponentBehaviour>
	{
		public bool Active
		{
			get { return enabled; }
			set { enabled = value; }
		}
		public IEntity Entity { get; set; }

		public void Copy(ComponentBehaviour reference)
		{
			Entity = reference.Entity;
		}
	}
}