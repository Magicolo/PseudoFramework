using UnityEngine;
using System.Collections.Generic;


namespace Pseudo
{
	public abstract class ArchitectCommand
	{
		protected ArchitectOld architect;

		protected ArchitectCommand(ArchitectOld architect)
		{
			this.architect = architect;
		}

		public abstract bool Do();
		public abstract void Undo();
	}
}
