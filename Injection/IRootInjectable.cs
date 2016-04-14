using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public interface IRootInjectable
	{
		/// <summary>
		/// Called before an IRoot instance begins to IRoot.InjectAll().
		/// </summary>
		/// <param name="root"></param>
		void OnPreRootInject(IRoot root);
		/// <summary>
		/// Called after an IRoot instance is done with IRoot.InjectAll().
		/// </summary>
		/// <param name="root"></param>
		void OnPostRootInject(IRoot root);
	}
}
