using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class SingletonScope : IInjectionScope
	{
		object instance;
		bool created;

		public object GetInstance(IInjectionFactory factory, InjectionContext context)
		{
			if (!created)
			{
				instance = factory.Create(context);
				created = true;
			}

			return instance;
		}
	}
}
