using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public class ApplicationUpdater : MonoBehaviour
	{
		public Action OnUpdate;

		void Update()
		{
			OnUpdate();
		}
	}
}
