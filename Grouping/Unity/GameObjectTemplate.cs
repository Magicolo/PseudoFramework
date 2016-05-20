using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Grouping.Internal
{
	public class GameObjectTemplate : Template<IGameObjectElement>
	{
		public GameObjectTemplate(params Type[] filter)
			: base(filter.Convert(t => new TemplateElement<IGameObjectElement>(e => e.Validate(t)))) { }
	}
}
