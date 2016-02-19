using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Copy
{
	public class AnimationCurveCopier : Copier<AnimationCurve>
	{
		public override void CopyTo(AnimationCurve source, AnimationCurve target)
		{
			if (source == null || target == null)
				return;

			target.Copy(source);
		}
	}
}