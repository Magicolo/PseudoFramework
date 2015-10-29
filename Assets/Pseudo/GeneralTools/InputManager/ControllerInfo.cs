using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[System.Serializable]
	public abstract class ControllerInfo : INamable
	{
		[SerializeField]
		string name = "";
		public string Name { get { return name; } set { name = value; } }

		protected ControllerInfo(string name)
		{
			this.name = name;
		}
	}
}