using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Internal.Schema
{
	public class PullValueOutlet<TValue> : ValueOutlet<TValue>
	{
		readonly FunctionOut<TValue> getter;

		public PullValueOutlet(FunctionOut<TValue> getter)
		{
			this.getter = getter;
		}

		public override TValue PullValue()
		{
			return getter();
		}
	}
}
