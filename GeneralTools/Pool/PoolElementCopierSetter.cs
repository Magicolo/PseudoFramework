using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public class PoolElementCopierSetter : IPoolElementSetter
	{
		readonly ICopier copier;
		readonly object source;
		readonly bool isUnityObject;

		public PoolElementCopierSetter(ICopier copier, object source)
		{
			this.copier = copier;
			this.source = source;
			isUnityObject = source is UnityEngine.Object;
		}

		public void SetValue(IList array, int index)
		{
			if (array == null)
				return;

			if (array.Count > index)
			{
				var target = array[index];

				if (target == null)
				{
					if (isUnityObject)
						return;
					else
						target = array[index] = TypePoolManager.Create(source.GetType());
				}

				copier.CopyTo(source, target);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(source));
		}
	}
}