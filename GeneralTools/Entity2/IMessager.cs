using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Entity
{
	public interface IMessager
	{
		void SendMessage(object target);
		void SendMessage(object target, object argument);
	}
}