using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Pseudo.Internal;

namespace Pseudo
{
	public abstract partial class PMonoBehaviour : MonoBehaviour
	{
		protected virtual void OnValidate()
		{
#if UNITY_EDITOR
			Editor.Internal.InspectorUtility.OnValidate(this);
#endif
		}
	}
}

