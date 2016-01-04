using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public interface ILateUpdateable
	{
		bool Active { get; set; }
		float LateUpdateRate { get; }

		void LateUpdate();
	}
}