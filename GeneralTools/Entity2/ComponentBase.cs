using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo2;

namespace Pseudo2
{
	public abstract class ComponentBase : IComponent
	{
		public IEntity Entity { get; set; }
	}
}