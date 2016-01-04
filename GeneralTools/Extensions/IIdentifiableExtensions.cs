using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Pseudo.Internal;

namespace Pseudo
{
	public static class IIdentifiableExtensions
	{
		public static int[] GetIds(this IList<IIdentifiable> identifiables)
		{
			int[] ids = new int[identifiables.Count];

			for (int i = 0; i < identifiables.Count; i++)
				ids[i] = identifiables[i].Id;

			return ids;
		}
	}
}