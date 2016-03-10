using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Schema
{
	public class EqualityNode<TValue> : NodeBase
	{
		readonly ValueInlet<TValue> inlet1;
		readonly ValueInlet<TValue> inlet2;
		readonly PullValueOutlet<bool> outlet;

		public EqualityNode()
		{
			inlet1 = new ValueInlet<TValue>();
			inlet2 = new ValueInlet<TValue>();
			outlet = new PullValueOutlet<bool>(() => PEqualityComparer<TValue>.Default.Equals(inlet1.PullValue(), inlet2.PullValue()));
		}

		public override ValueInlet GetValueInlet(int index)
		{
			switch (index)
			{
				default:
				case 0:
					return inlet1;
				case 1:
					return inlet2;
			}
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return outlet;
		}
	}
}
