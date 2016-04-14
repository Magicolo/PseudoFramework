using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.EntityFramework
{
	public class ComponentBase : IComponent
	{
		public bool Active { get; set; }
		public IEntity Entity { get; set; }

		public virtual void OnAdded() { }
		public virtual void OnRemoved() { }
		public virtual void OnActivated() { }
		public virtual void OnDeactivated() { }
		public virtual void OnEntityActivated() { }
		public virtual void OnEntityDeactivated() { }
	}
}
