using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Injection;
using Pseudo.EntityFramework;
using Pseudo.Communication;

namespace Pseudo
{
	public class RecycleOnMessage : ComponentBehaviour, IMessageable
	{
		public EntityBehaviour Recycle;
		public MessageEnum Message;

		bool recycle;
		[Inject]
		readonly IEntityManager entityManager = null;

		void LateUpdate()
		{
			if (recycle)
			{
				recycle = false;
				entityManager.RecycleEntity(Recycle);
			}
		}

		void IMessageable.OnMessage<TId>(TId message)
		{
			recycle |= Message.Equals(message) && Recycle != null;
		}
	}
}