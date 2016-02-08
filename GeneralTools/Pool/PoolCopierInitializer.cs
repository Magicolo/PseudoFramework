using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public class PoolCopierInitializer : IPoolInitializer
	{
		readonly ICopier copier;
		readonly object source;

		public PoolCopierInitializer(ICopier copier, object source)
		{
			this.copier = copier;
			this.source = source;
		}

		public void Initialize(object instance)
		{
			copier.CopyTo(source, instance);
		}
	}
}
