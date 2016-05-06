using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;


namespace Pseudo.Architect
{
	public abstract class ArchitectControlerBase : MonoBehaviour
	{

		public ArchitectControler Architect { get; protected set; }
		public ArchitectBehavior ArchitectBehavior { get; protected set; }
		protected ArchitectLinker Linker { get { return Architect.Linker; } }


		protected virtual void Awake()
		{
			ArchitectBehavior = GetComponentInParent<ArchitectBehavior>();
			Architect = this.ArchitectBehavior.Architect;
			Architect.Linker = ArchitectBehavior.Linker;
		}

	}
}