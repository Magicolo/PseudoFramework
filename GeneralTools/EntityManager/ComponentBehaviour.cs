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
			get { return active; }
			set { enabled = value; }
		}
		public IEntity Entity { get; set; }
		public EntityBehaviour EntityHolder { get { return cachedEntityHolder.Value; } }

		bool active;
		readonly Lazy<EntityBehaviour> cachedEntityHolder;

		protected ComponentBehaviour()
		{
			cachedEntityHolder = new Lazy<EntityBehaviour>(GetComponent<EntityBehaviour>);
		}

		protected virtual void OnEnable()
		{
			active = true;
		}

		protected virtual void OnDisable()
		{
			active = false;
		}
	}
}