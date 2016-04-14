using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.EntityFramework.Internal
{
	public class EntityGroup : IEntityGroup
	{
		public struct Enumerator : IEnumerator<IEntity>
		{
			public IEntity Current
			{
				get { return current; }
			}

			object IEnumerator.Current
			{
				get { return current; }
			}

			IEntityGroup group;
			IEntity current;
			int index;

			public Enumerator(IEntityGroup group) : this()
			{
				this.group = group;
			}

			public bool MoveNext()
			{
				if (index >= group.Count)
				{
					current = null;
					return false;
				}
				else
				{
					current = group[index];
					index++;
					return true;
				}
			}

			public void Reset()
			{
				current = null;
				index = 0;
			}

			public void Dispose()
			{
				group = null;
				Reset();
			}
		}

		static readonly EntityMatches[] matchValues = (EntityMatches[])Enum.GetValues(typeof(EntityMatches));

		public event Action<IEntity> OnEntityAdded = delegate { };
		public event Action<IEntity> OnEntityRemoved = delegate { };
		public int Count
		{
			get { return hashedEntities.Count; }
		}
		// Assumes that user checks the bounds of the group.
		public IEntity this[int index]
		{
			get { return entities[index]; }
		}

		readonly EntityMatchGroup[] subGroups = new EntityMatchGroup[matchValues.Length];
		readonly HashSet<IEntity> hashedEntities = new HashSet<IEntity>();
		IEntity[] entities = new IEntity[8];

		public IEntityGroup Filter(EntityMatch match)
		{
			return Filter(match.Groups, match.Match);
		}

		public IEntityGroup Filter(EntityGroups groups, EntityMatches match = EntityMatches.All)
		{
			return GetMatchGroup(match).GetGroupByEntityGroup(groups);
		}

		public IEntityGroup Filter(Type componentType, EntityMatches match = EntityMatches.All)
		{
			return GetMatchGroup(match).GetGroupByComponentIndices(ComponentUtility.GetComponentIndices(componentType));
		}

		public IEntityGroup Filter(Type[] componentTypes, EntityMatches match = EntityMatches.All)
		{
			return GetMatchGroup(match).GetGroupByComponentIndices(ComponentUtility.GetComponentIndices(componentTypes));
		}

		public void BroadcastMessage(EntityMessage message)
		{
			BroadcastMessage(message.Message.Value, (object)null, message.Scope);
		}

		public void BroadcastMessage<TArg>(EntityMessage message, TArg argument)
		{
			BroadcastMessage(message.Message.Value, argument, message.Scope);
		}

		public void BroadcastMessage<TId>(TId identifier)
		{
			BroadcastMessage(identifier, (object)null, HierarchyScopes.Self);
		}

		public void BroadcastMessage<TId>(TId identifier, HierarchyScopes scope)
		{
			BroadcastMessage(identifier, (object)null, scope);
		}

		public void BroadcastMessage<TId, TArg>(TId identifier, TArg argument)
		{
			BroadcastMessage(identifier, argument, HierarchyScopes.Self);
		}

		public void BroadcastMessage<TId, TArg>(TId identifier, TArg argument, HierarchyScopes scope)
		{
			for (int i = Count - 1; i >= 0; i--)
				entities[i].SendMessage(identifier, argument, scope);
		}

		public bool Contains(IEntity entity)
		{
			return hashedEntities.Contains(entity);
		}

		public int IndexOf(IEntity entity)
		{
			return Array.IndexOf(entities, entity);
		}

		public IEntity Find(Predicate<IEntity> match)
		{
			int index = FindIndex(match);

			return index == -1 ? null : entities[index];
		}

		public int FindIndex(Predicate<IEntity> match)
		{
			return Array.FindIndex(entities, 0, Count, match);
		}

		public IEntity[] ToArray()
		{
			return hashedEntities.ToArray();
		}

		public void CopyTo(IEntity[] array, int index = 0)
		{
			Array.Copy(entities, 0, array, index, Count);
		}

		public void Clear()
		{
			OnEntityAdded = delegate { };
			OnEntityRemoved = delegate { };
			hashedEntities.Clear();
			entities.Clear();

			for (int i = 0; i < subGroups.Length; i++)
			{
				var subGroup = subGroups[i];

				if (subGroup != null)
					subGroup.Clear();
			}
		}

		public void UpdateEntity(IEntity entity, bool isValid)
		{
			if (isValid)
				Add(entity);
			else
				Remove(entity);

			for (int i = 0; i < subGroups.Length; i++)
			{
				var subGroup = subGroups[i];

				if (subGroup != null)
					subGroup.UpdateEntity(entity, isValid);
			}
		}

		void Add(IEntity entity)
		{
			if (hashedEntities.Add(entity))
			{
				if (entities.Length < Count)
					Array.Resize(ref entities, Mathf.NextPowerOfTwo(Count));

				entities[Count - 1] = entity;
				OnEntityAdded(entity);
			}
		}

		void Remove(IEntity entity)
		{
			if (hashedEntities.Remove(entity))
			{
				int index = IndexOf(entity);
				entities[index] = null;

				for (int i = index + 1; i < Count + 1; i++)
					entities[i - 1] = entities[i];

				OnEntityRemoved(entity);
			}
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		EntityMatchGroup GetMatchGroup(EntityMatches match)
		{
			var matchGroup = subGroups[(int)match];

			if (matchGroup == null)
			{
				matchGroup = new EntityMatchGroup(this, match);
				subGroups[(int)match] = matchGroup;
			}

			return matchGroup;
		}

		IEnumerator<IEntity> IEnumerable<IEntity>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}