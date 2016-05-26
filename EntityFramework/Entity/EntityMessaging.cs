using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Communication;

namespace Pseudo.EntityFramework.Internal
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

		public void SendMessage<TId>(TId identifier, HierarchyScopes scope)
		{
			SendMessage(identifier, (object)null, scope);
		}

		public void SendMessage<TId, TArg>(TId identifier, TArg argument)
		{
			if (!Active)
				return;

			for (int i = allComponents.Count - 1; i >= 0; i--)
			{
				var component = allComponents[i];

				if (component.Active)
					messageManager.Send(component, identifier, argument);
			}
		}

		public void SendMessage<TId, TArg>(TId identifier, TArg argument, HierarchyScopes scope)
		{
			// Must be before Hierarchy.
			if (scope.Contains(HierarchyScopes.Root))
				Root.SendMessage(identifier, argument);

			// Must return if scope is Hierarchy to prevent duplicated work.
			if (scope.Contains(HierarchyScopes.Hierarchy))
			{
				Root.SendMessage(identifier, argument, HierarchyScopes.Descendants);
				return;
			}

			// Should be before Siblings, Children, Descendants, Parent and Ancestors for fastest resolution.
			if (scope.Contains(HierarchyScopes.Self))
				SendMessage(identifier, argument);

			if (scope.Contains(HierarchyScopes.Siblings) && parent != null && parent.Children.Count > 0)
			{
				for (int i = 0; i < parent.Children.Count; i++)
				{
					var child = parent.Children[i];

					if (child != this)
						child.SendMessage(identifier, argument, HierarchyScopes.Self);
				}
			}

			// Must be before Descendants.
			if (scope.Contains(HierarchyScopes.Children) && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
					children[i].SendMessage(identifier, argument, HierarchyScopes.Self);
			}

			if (scope.Contains(HierarchyScopes.Descendants) && children.Count > 0)
			{
				for (int i = 0; i < children.Count; i++)
					children[i].SendMessage(identifier, argument, HierarchyScopes.Descendants);
			}

			// Must be before Ancestors.
			if (scope.Contains(HierarchyScopes.Parent) && parent != null)
				parent.SendMessage(identifier, argument, HierarchyScopes.Self);

			if (scope.Contains(HierarchyScopes.Ancestors) && parent != null)
				parent.SendMessage(identifier, argument, HierarchyScopes.Ancestors);
		}
	}
}
