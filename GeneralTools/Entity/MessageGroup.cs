﻿using UnityEngine;
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

		readonly Dictionary<Component, IMessager> messagers = new Dictionary<Component, IMessager>();

		string method;

		public MessageGroup(string method)
		{
			this.method = method;
		}

		public void SendMessage()
		{
			if (messagers.Count > 0)
			{
				var enumerator = messagers.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.SendMessage(enumerator.Current.Key);

				enumerator.Dispose();
			}
		}

		public void SendMessage<T>(T argument)
		{
			if (messagers.Count > 0)
			{
				var enumerator = messagers.GetEnumerator();

				while (enumerator.MoveNext())
					((MessagerBase<T>)enumerator.Current.Value).SendMessage(enumerator.Current.Key, argument);

				enumerator.Dispose();
			}
		}

		public void TryAddComponent(Component component)
		{
			if (!(component is PComponent) || messagers.ContainsKey(component))
				return;

			var messager = GetMessagerGroup(method).GetMessager(component.GetType());

			if (messager != null)
				messagers[component] = messager;
		}

		public void RemoveComponent(Component component)
		{
			if (!(component is PComponent))
				return;

			messagers.Remove(component);
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