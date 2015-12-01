using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class EntityGroup
	{
		[Flags]
		public enum Groups : ulong
		{
			Player = 1 << 0,
			Enemy = 1 << 1,
		}

		public enum Matches
		{
			All,
			Any,
			None,
			Exact
		}

		public event Action<PEntity> OnEntityAdded;
		public event Action<PEntity> OnEntityRemoved;

		readonly List<PEntity> entities = new List<PEntity>(16);

		public List<PEntity> GetEntities()
		{
			return entities;
		}

		public void Add(PEntity entity)
		{
			if (!entities.Contains(entity))
			{
				entities.Add(entity);
				RaiseOnEntityAdded(entity);
			}
		}

		public void Remove(PEntity entity)
		{
			if (entities.Remove(entity))
				RaiseOnEntityRemoved(entity);
		}

		public void Clear()
		{
			entities.Clear();
		}

		protected virtual void RaiseOnEntityAdded(PEntity entity)
		{
			if (OnEntityAdded != null)
				OnEntityAdded(entity);
		}

		protected virtual void RaiseOnEntityRemoved(PEntity entity)
		{
			if (OnEntityRemoved != null)
				OnEntityRemoved(entity);
		}
	}
}