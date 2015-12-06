using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	public class EntityJanitor : PMonoBehaviour
	{
		void OnDestroy()
		{
			EntityUtility.ClearAllEntityGroups();
		}
	}
}