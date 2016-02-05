using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo
{
	public class GameObjectComponent : IComponent
	{
		public GameObject GameObject;

		public bool Active { get; set; }
		public IEntity Entity { get; set; }
		public IEntityManager EntityManager { get; set; }
	}
}