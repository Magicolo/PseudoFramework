using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.EntityFramework.Internal
{
	public partial class Entity
	{
		public IEntity Root
		{
			get { return GetRoot(); }
		}
		public IEntity Parent
		{
			get { return parent; }
		}
		public IList<IEntity> Children
		{
			get { return readonlyChildren; }
		}

		IEntity parent;

		readonly List<IEntity> children;
		readonly IList<IEntity> readonlyChildren;

		public void SetParent(IEntity entity)
		{
			if (parent == entity)
				return;

			if (parent != null)
				parent.RemoveChild(this);

			parent = entity;

			if (parent != null)
				parent.AddChild(this);
		}

		public bool HasChild(IEntity entity)
		{
			return children.Contains(entity);
		}

		public void AddChild(IEntity entity)
		{
			Assert.IsNotNull(entity);

			if (!HasChild(entity))
			{
				children.Add(entity);
				entity.SetParent(this);
			}
		}

		public void RemoveChild(IEntity entity)
		{
			Assert.IsNotNull(entity);

			if (HasChild(entity))
			{
				children.Remove(entity);
				entity.SetParent(null);
			}
		}

		public void RemoveAllChildren()
		{
			for (int i = children.Count - 1; i >= 0; i--)
				RemoveChild(children[i]);
		}

		IEntity GetRoot()
		{
			IEntity root = this;

			while (root.Parent != null)
				root = root.Parent;

			return root;
		}
	}
}
