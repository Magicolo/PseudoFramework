using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public class StateMachineLateUpdateCaller : StateMachineCaller
	{

		void LateUpdate()
		{
			if (machine.IsActive)
			{
				machine.OnLateUpdate();
			}
		}
	}
}