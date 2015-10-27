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
		string _name = "";
		public string Name { get { return _name; } set { _name = value; } }

		protected ControllerInfo(string name)
		{
			_name = name;
		}
	}
}