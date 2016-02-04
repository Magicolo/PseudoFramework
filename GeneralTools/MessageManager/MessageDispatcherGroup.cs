using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using UnityEngine.Assertions;

namespace Pseudo.Internal
{
	public class MessageDispatcherGroup<TId> : IMessageDispatcherGroup
	{
		readonly Dictionary<Type, IMessageDispatcher> typeToDispatcher = new Dictionary<Type, IMessageDispatcher>();

		TId identifier;

		public MessageDispatcherGroup(TId identifier)
		{
			this.identifier = identifier;
		}

		public void Send<TArg1, TArg2, TArg3>(object target, TArg1 argument1, TArg2 argument2, TArg3 argument3)
		{
			Assert.IsNotNull(target);

			var dispatcher = GetDispatcher(target.GetType());

			if (dispatcher is MessageDispatcherBase<TArg1, TArg2, TArg3>)
				((MessageDispatcherBase<TArg1, TArg2, TArg3>)dispatcher).Send(target, argument1, argument2, argument3);
			else if (dispatcher is MessageDispatcherBase<TArg1, TArg2>)
				((MessageDispatcherBase<TArg1, TArg2>)dispatcher).Send(target, argument1, argument2);
			else if (dispatcher is MessageDispatcherBase<TArg1>)
				((MessageDispatcherBase<TArg1>)dispatcher).Send(target, argument1);
			else if (dispatcher is MessageDispatcherBase)
				((MessageDispatcherBase)dispatcher).Send(target);
			else if (dispatcher != null)
				dispatcher.Send(target, argument1, argument2, argument3);

			if (target is IMessageable)
				((IMessageable)target).OnMessage(identifier);

			if (target is IMessageable<TId>)
				((IMessageable<TId>)target).OnMessage(identifier);
		}

		IMessageDispatcher GetDispatcher(Type type)
		{
			IMessageDispatcher dispatcher;

			if (!typeToDispatcher.TryGetValue(type, out dispatcher))
			{
				dispatcher = CreateDispatcher(type);
				typeToDispatcher[type] = dispatcher;
			}

			return dispatcher;
		}

		IMessageDispatcher CreateDispatcher(Type type)
		{
			var method = MessageUtility.GetValidMethod(type, identifier);

			if (method == null)
				return null;

			var types = MessageUtility.GetParameterTypes(method);

			Type actionType = null;
			Type dispatcherType = null;

			switch (types.Length)
			{
				case 1:
					actionType = typeof(Action<>).MakeGenericType(types);
					dispatcherType = typeof(MessageDispatcher<>).MakeGenericType(types);
					break;
				case 2:
					actionType = typeof(Action<,>).MakeGenericType(types);
					dispatcherType = typeof(MessageDispatcher<,>).MakeGenericType(types);
					break;
				case 3:
					actionType = typeof(Action<,,>).MakeGenericType(types);
					dispatcherType = typeof(MessageDispatcher<,,>).MakeGenericType(types);
					break;
				case 4:
					actionType = typeof(Action<,,,>).MakeGenericType(types);
					dispatcherType = typeof(MessageDispatcher<,,,>).MakeGenericType(types);
					break;
			}

			if (actionType == null || dispatcherType == null)
				return null;

			var action = Delegate.CreateDelegate(actionType, method);
			var dispatcher = (IMessageDispatcher)Activator.CreateInstance(dispatcherType, action);

			return dispatcher;
		}
	}
}
