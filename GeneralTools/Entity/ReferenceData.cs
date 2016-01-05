using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	[Serializable]
	public class ReferenceData
	{
		public int Index;
		public string Path;
		public UnityEngine.Object Reference;

		public string[] PathSplit
		{
			get { return pathSplit ?? (pathSplit = Path.Split('.')); }
		}

		string[] pathSplit;
	}
}