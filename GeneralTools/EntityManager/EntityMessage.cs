using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public struct EntityMessage
	{
		public MessageEnum Message;
		public MessagePropagation Propagation;

		public EntityMessage(MessageEnum message, MessagePropagation propagation)
		{
			Message = message;
			Propagation = propagation;
		}
	}
}
