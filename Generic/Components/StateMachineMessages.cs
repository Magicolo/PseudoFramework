using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[MessageEnum]
	public enum StateMachineMessages
	{
		OnStateEnter,
		OnStateExit
	}

	public class StateMachineMessagesComparer : IEqualityComparer<StateMachineMessages>
	{
		public bool Equals(StateMachineMessages x, StateMachineMessages y)
		{
			return x == y;
		}

		public int GetHashCode(StateMachineMessages obj)
		{
			return (int)obj;
		}
	}
}
