using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class BehaviourEvents : PEnumFlag<BehaviourEvents>
	{
		public static readonly BehaviourEvents OnAwake = new BehaviourEvents(1);
		public static readonly BehaviourEvents OnEnable = new BehaviourEvents(2);
		public static readonly BehaviourEvents OnDisable = new BehaviourEvents(3);
		public static readonly BehaviourEvents OnStart = new BehaviourEvents(4);
		public static readonly BehaviourEvents OnLevelWasLoaded = new BehaviourEvents(5);

		protected BehaviourEvents(params byte[] values) : base(values) { }
	}
}
