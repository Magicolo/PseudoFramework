using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Architect
{
	[Serializable]
	public class AutoActionTestThing : ArchitectControlerBase
	{

		float t;
		void Start() 
		{
			t = Time.time + 0.5f;
		}
		void Update()
		{
			if (Time.time > t)
			{
				this.enabled = false;
				Architect.CreateNewMap("test", 10, 10);
			}
		}
	}
}
