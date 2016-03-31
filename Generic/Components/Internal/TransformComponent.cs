using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEngine.Assertions;
using Pseudo.EntityFramework;

namespace Pseudo
{
	public class TransformComponent : ComponentBase
	{
		public readonly Transform Transform;

		public TransformComponent(Transform transform)
		{
			Transform = transform;
		}
	}
}