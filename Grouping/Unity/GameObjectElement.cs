using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Grouping.Internal
{
	public class GameObjectElement : ElementBase<GameObject>, IGameObjectElement
	{
		public int ComponentCount;

		public GameObjectElement(GameObject element) : base(element) { }

		public virtual bool Validate(Type type)
		{
			return element.GetComponent(type) != null;
		}
	}
}
