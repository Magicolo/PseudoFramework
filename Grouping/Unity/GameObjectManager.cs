using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Grouping.Internal;

namespace Pseudo.Grouping
{
	public class GameObjectManager
	{
		struct GroupKey : IEquatable<GroupKey>
		{
			public readonly MatchType Match;
			public readonly Type[] Filter;

			public GroupKey(MatchType match, Type[] filter)
			{
				Match = match;
				Filter = filter;
			}

			public bool Equals(GroupKey other)
			{
				return Match == other.Match && Filter.ContentEquals(other.Filter);
			}

			public override int GetHashCode()
			{
				int hash = (int)Match;

				for (int i = 0; i < Filter.Length; i++)
					hash ^= Filter[i].GetHashCode() * 397;

				return hash;
			}
		}

		struct ElementKey : IEquatable<ElementKey>
		{
			public readonly GameObject GameObject;
			public readonly Type GroupType;

			public ElementKey(GameObject gameObject, Type groupType)
			{
				GameObject = gameObject;
				GroupType = groupType;
			}

			public bool Equals(ElementKey other)
			{
				return GameObject == other.GameObject && GroupType == other.GroupType;
			}

			public override int GetHashCode()
			{
				return (GameObject.GetHashCode() * 517) ^ (GroupType.GetHashCode() * 397);
			}
		}

		readonly List<IGameObjectGroup> groups = new List<IGameObjectGroup>();
		readonly List<GameObjectElement> elements = new List<GameObjectElement>();
		readonly Dictionary<GroupKey, IGameObjectGroup> groupKeyToGroup = new Dictionary<GroupKey, IGameObjectGroup>();
		readonly Dictionary<GameObject, GameObjectElement> gameObjectToElement = new Dictionary<GameObject, GameObjectElement>();

		public IGameObjectGroup GetGroup(MatchType match, params Type[] filter)
		{
			return GetGroup(new GroupKey(match, filter));
		}

		public void Register(GameObject gameObject)
		{
			var element = GetElement(gameObject);
			element.ComponentCount++;

			UpdateElement(element);
		}

		public void Unregister(GameObject gameObject)
		{
			GameObjectElement element;

			if (gameObjectToElement.TryGetValue(gameObject, out element))
			{
				element.ComponentCount--;
				UpdateElement(element);
			}
		}

		void UpdateElement(GameObjectElement element)
		{
			if (element.ComponentCount > 0)
			{
				for (int i = 0; i < groups.Count; i++)
					groups[i].Update(element);
			}
			else
			{
				for (int i = 0; i < groups.Count; i++)
					groups[i].Remove(element);

				elements.Remove(element);
				gameObjectToElement.Remove(element.Element);
			}
		}

		void UpdateGroup(IGameObjectGroup group)
		{
			for (int i = 0; i < elements.Count; i++)
				group.Update(elements[i]);
		}

		GameObjectElement GetElement(GameObject gameObject)
		{
			GameObjectElement element;

			if (!gameObjectToElement.TryGetValue(gameObject, out element))
			{
				element = CreateElement(gameObject);
				elements.Add(element);
				gameObjectToElement[gameObject] = element;
			}

			return element;
		}

		GameObjectElement CreateElement(GameObject gameObject)
		{
			return new GameObjectElement(gameObject);
		}

		IGameObjectGroup GetGroup(GroupKey key)
		{
			IGameObjectGroup group;

			if (!groupKeyToGroup.TryGetValue(key, out group))
			{
				group = CreateGroup(key);
				groups.Add(group);
				groupKeyToGroup[key] = group;
			}

			return group;
		}

		IGameObjectGroup CreateGroup(GroupKey key)
		{
			var group = new GameObjectGroup(key.Match, key.Filter);
			UpdateGroup(group);

			return group;
		}
	}
}
