using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class AnimatorExtentions
	{
		public static bool IsPlaying(this Animator animator)
		{
			return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0);
		}
	}
}