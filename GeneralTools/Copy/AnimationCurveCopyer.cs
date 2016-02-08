using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	public class AnimationCurveCopyer : Copyer<AnimationCurve>
	{
		public override void CopyTo(AnimationCurve source, AnimationCurve target)
		{
			target.Copy(source);
		}
	}
}
