using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class RecycleOnMessage : ComponentBehaviour, IMessageable
	{
		public EntityBehaviour Recycle;
		public MessageEnum Message;

		bool recycle;

		void LateUpdate()
		{
			if (recycle)
			{
				recycle = false;
				Entity.Manager.RecycleEntity(Recycle);
			}
		}

		void IMessageable.OnMessage<TId>(TId message)
		{
			recycle |= Message.Equals(message) && Recycle != null;
		}
	}
}