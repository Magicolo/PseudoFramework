using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Schema
{
	public class PushValueOutlet<TValue> : ValueOutlet<TValue>
	{
		TValue value;

		public void PushValue(TValue value)
		{
			this.value = value;
		}

		public override TValue PullValue()
		{
			return value;
		}
	}
}
