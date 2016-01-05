using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Entity
{
	public class MessageGroup
	{
		static readonly Dictionary<string, MessagerGroup> messagerGroups = new Dictionary<string, MessagerGroup>();

		readonly Dictionary<object, IMessager> messagers = new Dictionary<object, IMessager>();
		readonly List<object> toAdd = new List<object>();
		readonly List<object> toRemove = new List<object>();

		string method;
		bool iterating;

		public MessageGroup(string method)
		{
			this.method = method;
		}

		public void SendMessage()
		{
			if (messagers.Count > 0)
			{
				iterating = true;
				var enumerator = messagers.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.SendMessage(enumerator.Current.Key);

				enumerator.Dispose();
				iterating = false;
				UpdateMessagers();
			}
		}

		public void SendMessage<T>(T argument)
		{
			if (messagers.Count > 0)
			{
				iterating = true;
				var enumerator = messagers.GetEnumerator();

				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Value is MessagerBase<T>)
						((MessagerBase<T>)enumerator.Current.Value).SendMessage(enumerator.Current.Key, argument);
					else
						enumerator.Current.Value.SendMessage(enumerator.Current.Key);
				}

				enumerator.Dispose();
				iterating = false;
				UpdateMessagers();
			}
		}

		public void TryAdd(object instance)
		{
			if (messagers.ContainsKey(instance))
				return;

			if (iterating)
				toAdd.Add(instance);
			else
			{
				var messager = GetMessagerGroup(method).GetMessager(instance.GetType());

				if (messager != null)
					messagers[instance] = messager;
			}
		}

		public void Remove(object instance)
		{
			if (iterating)
				toRemove.Add(instance);
			else
				messagers.Remove(instance);
		}

		void UpdateMessagers()
		{
			for (int i = 0; i < toAdd.Count; i++)
				TryAdd(toAdd[i]);

			for (int i = 0; i < toRemove.Count; i++)
				Remove(toRemove[i]);

			toAdd.Clear();
			toRemove.Clear();
		}

		MessagerGroup GetMessagerGroup(string method)
		{
			MessagerGroup messager;

			if (!messagerGroups.TryGetValue(method, out messager))
			{
				messager = new MessagerGroup(method);
				messagerGroups[method] = messager;
			}

			return messager;
		}
	}
}
