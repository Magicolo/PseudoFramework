using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	[AddComponentMenu("")]
	public class StateMachineFixedUpdateCaller : StateMachineCaller
	{
		void FixedUpdate()
		{
			if (machine.IsActive)
				machine.OnFixedUpdate();
		}
	}
}