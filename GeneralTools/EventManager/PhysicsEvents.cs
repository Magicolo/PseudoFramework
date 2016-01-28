using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class PhysicsEvents : PEnumFlag<PhysicsEvents>
	{
		public static readonly PhysicsEvents OnTriggerEnter = new PhysicsEvents(1);
		public static readonly PhysicsEvents OnTriggerStay = new PhysicsEvents(2);
		public static readonly PhysicsEvents OnTriggerExit = new PhysicsEvents(3);
		public static readonly PhysicsEvents OnCollisionEnter = new PhysicsEvents(4);
		public static readonly PhysicsEvents OnCollisionStay = new PhysicsEvents(5);
		public static readonly PhysicsEvents OnCollisionExit = new PhysicsEvents(6);
		public static readonly PhysicsEvents OnTriggerEnter2D = new PhysicsEvents(7);
		public static readonly PhysicsEvents OnTriggerStay2D = new PhysicsEvents(8);
		public static readonly PhysicsEvents OnTriggerExit2D = new PhysicsEvents(9);
		public static readonly PhysicsEvents OnCollisionEnter2D = new PhysicsEvents(10);
		public static readonly PhysicsEvents OnCollisionStay2D = new PhysicsEvents(11);
		public static readonly PhysicsEvents OnCollisionExit2D = new PhysicsEvents(12);

		protected PhysicsEvents(params byte[] values) : base(values) { }
	}
}