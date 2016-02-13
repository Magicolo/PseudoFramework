using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[RequireComponent(typeof(EntityBehaviour))]
	public abstract class ComponentBehaviour : PMonoBehaviour, IComponent
	{
		public bool Active
		{
			get { return enabled; }
			set
			{
				if (Entity != null && enabled != value)
				{
					enabled = value;

					if (enabled)
						OnActivated();
					else
						OnDeactivated();
				}
			}
		}
		public IEntity Entity { get; set; }

		public virtual void OnActivated() { }
		public virtual void OnDeactivated() { }
	}
}