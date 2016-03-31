using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Communication;

namespace Pseudo.EntityFramework
{
	[Serializable]
	public struct EntityMessage
	{
		public MessageEnum Message;
		[EnumFlags]
		public HierarchyScope Scope;

		public EntityMessage(MessageEnum message, HierarchyScope scope = HierarchyScope.Local)
		{
			Message = message;
			Scope = scope;
		}

		public EntityMessage(Enum message, HierarchyScope scope = HierarchyScope.Local)
		{
			Message = message;
			Scope = scope;
		}
	}
}
