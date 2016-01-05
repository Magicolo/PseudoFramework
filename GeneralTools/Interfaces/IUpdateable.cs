using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public interface IUpdateable
	{
		bool Active { get; set; }
		float UpdateRate { get; }

		void Update();
	}
}