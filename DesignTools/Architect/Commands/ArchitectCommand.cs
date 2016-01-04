using UnityEngine;
using System.Collections.Generic;


namespace Pseudo
{
	public abstract class ArchitectCommand
	{
		protected Architect architect;

		protected ArchitectCommand(Architect architect)
		{
			this.architect = architect;
		}

		public abstract bool Do();
		public abstract void Undo();
	}
}
