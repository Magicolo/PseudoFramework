using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Schema
{
	public class IteratorNode : ExecutionNodeBase
	{
		readonly ValueInlet<int> inlet = new ValueInlet<int>();
		readonly PushValueOutlet<int> outlet = new PushValueOutlet<int>();

		public override ExecutionResults Execute()
		{
			int iteration = inlet.PullValue();
			var nextNode = GetNextNode();

			if (nextNode != null)
			{
				for (int i = 0; i < iteration; i++)
				{
					outlet.PushValue(i);
					var result = nextNode.Execute();

					if (result != ExecutionResults.Continue)
						break;
				}
			}

			outlet.PushValue(0);

			return ExecutionResults.Continue;
		}

		public override ValueInlet GetValueInlet(int index)
		{
			return inlet;
		}

		public override ValueOutlet GetValueOutlet(int index)
		{
			return outlet;
		}
	}
}
