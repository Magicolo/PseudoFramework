using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Pool
{
	public class PoolCopierSetter : IPoolSetter
	{
		readonly ICopier copier;
		readonly FieldInfo field;
		readonly object source;
		readonly bool isUnityObject;

		public PoolCopierSetter(ICopier copier, FieldInfo field, object source)
		{
			this.copier = copier;
			this.field = field;
			this.source = source;
			isUnityObject = source is UnityEngine.Object;
		}

		public void SetValue(object instance)
		{
			if (instance == null)
				return;

			var target = field.GetValue(instance);

			if (target == null)
			{
				if (isUnityObject)
					return;
				else
					field.SetValue(instance, target = TypePoolManager.Create(source.GetType()));
			}

			copier.CopyTo(source, target);
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(source));
		}
	}
}