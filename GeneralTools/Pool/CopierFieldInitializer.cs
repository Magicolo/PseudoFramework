using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public class CopierFieldInitializer : IFieldInitializer
	{
		readonly ICopier copier;
		readonly object source;

		public CopierFieldInitializer(ICopier copier, object source)
		{
			this.copier = copier;
			this.source = source;
		}

		public void InitializeFields(object instance)
		{
			copier.CopyTo(source, instance);
		}
	}
}
