using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	public partial class Entity
	{
		MessageManager messageManager = null;

		public void SendMessage(EntityMessage message)
		{
			SendMessage(message.Message.Value, (object)null, message.Scope);
		}

		public void SendMessage<TArg>(EntityMessage message, TArg argument)
		{
			SendMessage(message.Message.Value, argument, message.Scope);
		}

		public void SendMessage<TId>(TId identifier)
		{
			SendMessage(identifier, (object)null);
		}

		public void SendMessage<TId>(TId identifier, HierarchyScope scope)
		{
			SendMessage(identifier, (object)null, scope);
		}

		public void SendMessage<TId, TArg>(TId identifier, TArg argument)
		{
			for (int i = allComponents.Count - 1; i >= 0; i--)
			{
				var component = allComponents[i];

				if (component.Active)
					messageManager.Send(component, identifier, argument);
			}
		}

		public void SendMessage<TId, TArg>(TId identifier, TArg argument, HierarchyScope scope)
		{
			if (!Active)
				return;

			if ((scope & HierarchyScope.Global) != 0)
			{
				Root.SendMessage(identifier, argument, HierarchyScope.Local | HierarchyScope.Children);
				return;
			}

			if ((scope & HierarchyScope.Root) != 0)
				Root.SendMessage(identifier, argument);

			if ((scope & HierarchyScope.Local) != 0)
				SendMessage(identifier, argument);

			if ((scope & HierarchyScope.Siblings) != 0 && parent != null && parent.Children.Count > 0)
			{
				for (int i = 0; i < parent.Children.Count; i++)
				{
					var child = parent.Children[i];

					if (child != this)
						child.SendMessage(identifier, argument, HierarchyScope.Local);
				}
			}

			if ((scope & HierarchyScope.Parent) != 0 && parent != null)
				parent.SendMessage(identifier, argument, HierarchyScope.Local);

			if ((scope & HierarchyScope.Parents) != 0 && parent != null)
				parent.SendMessage(identifier, argument, HierarchyScope.Parents | HierarchyScope.Local);

			if ((scope & HierarchyScope.Children) != 0 && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
					children[i].SendMessage(identifier, argument, HierarchyScope.Children | HierarchyScope.Local);
			}
		}
	}
}
