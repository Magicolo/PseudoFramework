using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface IMessageDispatcher
	{
		void Send(object target, object argument1, object argument2, object argument3);
	}
}
