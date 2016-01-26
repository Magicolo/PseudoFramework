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
		readonly Lazy<EntityBehaviour> cachedEntity;
		public EntityBehaviour Entity { get { return cachedEntity.Value; } }

		protected ComponentBehaviour()
		{
			cachedEntity = new Lazy<EntityBehaviour>(GetComponent<EntityBehaviour>);
		}
	}
}