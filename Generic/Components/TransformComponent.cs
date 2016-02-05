using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo
{
	public class TransformComponent : IComponent
	{
		public Transform Transform;

		public bool Active { get; set; }
		public IEntity Entity { get; set; }
		public IEntityManager EntityManager { get; set; }
	}
}