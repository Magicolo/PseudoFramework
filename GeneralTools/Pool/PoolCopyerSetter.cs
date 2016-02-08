using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Pool
{
	public class PoolCopyerSetter : IPoolSetter
	{
		readonly ICopyer copyer;
		readonly object source;

		public PoolCopyerSetter(ICopyer copyer, object source)
		{
			this.copyer = copyer;
			this.source = source;
		}

		public void SetValue(object instance)
		{
			if (instance == null)
				return;

			copyer.CopyTo(source, instance);
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(source));
		}
	}
}