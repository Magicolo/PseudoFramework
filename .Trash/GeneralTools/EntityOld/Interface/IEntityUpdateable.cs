using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.EntityOld
{
	public interface IEntityUpdateable
	{
		bool Active { get; }

		void ComponentUpdate();
		void ComponentLateUpdate();
		void ComponentFixedUpdate();
	}
}