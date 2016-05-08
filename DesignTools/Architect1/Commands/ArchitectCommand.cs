using UnityEngine;
using System.Collections.Generic;


namespace Pseudo.Architect
{
	public abstract class ArchitectCommand
	{
		protected ArchitectCommand()
		{
		}

		public abstract bool Do();
		public abstract void Undo();
	}
}
