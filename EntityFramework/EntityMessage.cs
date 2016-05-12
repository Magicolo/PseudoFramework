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
		public Message Message;
		[SerializeField, EnumFlags(typeof(HierarchyScopes))]
		int scope;

		public HierarchyScopes Scope
		{
			get { return (HierarchyScopes)scope; }
			set { scope = (int)value; }
		}

		public EntityMessage(Message message, HierarchyScopes scope = HierarchyScopes.Self)
		{
			Message = message;
			this.scope = (int)scope;
		}

		public EntityMessage(Enum message, HierarchyScopes scope = HierarchyScopes.Self) : this((Message)message, scope) { }
	}
}
