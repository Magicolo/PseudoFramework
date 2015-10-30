using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class LocalGravityChannel : GravityChannelBase, ICopyable<LocalGravityChannel>
	{
		protected override Vector3 GetCurrentGravity()
		{
			return GravityManager.GetGravity(channel);
		}

		public void Copy(LocalGravityChannel reference)
		{
			base.Copy(reference);

		}
	}
}