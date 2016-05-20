using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Groupingz
{
	public interface IGroupFactory<TElement> : IFactory<MatchType, IList<int>, IGroup<TElement>> { }
}
