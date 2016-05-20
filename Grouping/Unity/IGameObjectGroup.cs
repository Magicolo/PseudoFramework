using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Grouping
{
	public interface IGameObjectGroup : IGroup<IGameObjectElement>
	{
		MatchType Match { get; }
		Type[] Filter { get; }

		bool Contains(GameObject element);
	}
}
