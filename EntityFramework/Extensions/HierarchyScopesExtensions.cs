using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.EntityFramework
{
	public static class HierarchyScopesExtentions
	{
		public static bool Contains(this HierarchyScopes scope, HierarchyScopes other)
		{
			return (scope & other) == other;
		}
	}
}
