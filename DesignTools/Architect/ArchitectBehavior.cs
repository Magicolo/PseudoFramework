using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class ArchitectBehavior : MonoBehaviour
	{

		public Architect Architect;
		public GUISkin skin;

		public ArchitectBehavior()
		{
			Architect = new Architect();
		}

		public void NewMap()
		{

		}

		///
		public void Test(string go, string go1)
		{ }
	}
}
