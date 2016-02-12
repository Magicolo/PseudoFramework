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

		public GameObject BaseMapRoot;

		public ArchitectBehavior()
		{
			Architect = new Architect();
		}

		void Awake()
		{
			BaseMapRoot = new GameObject("MapRoot");
		}
	}
}
