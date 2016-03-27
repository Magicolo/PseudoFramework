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
			set { SetActive(value); }
		}
		public IEntity Entity
		{
			get { return entity; }
			set { entity = value; }
		}

		IEntity entity;
		bool active;

		void OnEnable()
		{
			SetActive(true);
		}

		void OnDisable()
		{
			SetActive(false);
		}

		void SetActive(bool active)
		{
			if (entity == null)
				this.active = active;
			else if (this.active != active)
			{
				this.active = active;

				if (this.active)
					OnActivated();
				else
					OnDeactivated();
			}
		}

		public virtual void OnAdded() { }
		public virtual void OnRemoved() { }
		public virtual void OnActivated() { }
		public virtual void OnDeactivated() { }
		public virtual void OnEntityActivated() { }
		public virtual void OnEntityDeactivated() { }
	}
}