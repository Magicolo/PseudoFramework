using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using System;
using Pseudo;
using Pseudo.EntityFramework.Internal;
using Pseudo.Internal;
using Pseudo.Communication;

namespace Pseudo.EntityFramework.Tests
{
	public abstract class EntityFrameworkTestsBase
	{
		public EntityManager EntityManager;

		[SetUp]
		public virtual void Setup()
		{
			EntityManager = new EntityManager(new MessageManager());
		}

		[TearDown]
		public virtual void TearDown()
		{
			EntityManager = null;
		}
	}

	public class DummyComponent1 : ComponentBase, IMessageable<int>, IMessageable<string>
	{
		[Message(0)]
		public void MessageNoArgument() { }
		[Message(1)]
		public void MessageOneArgument(int arg) { }
		[Message("Boba")]
		public void MessageInheritance(IComponent component) { }
		[Message("Fett")]
		public void MessageConflict() { }

		public void OnMessage(int message) { }

		public void OnMessage(string message) { }
	}

	public class DummyComponent2 : ComponentBase, IMessageable
	{
		[Message(0)]
		public void MessageNoArgument() { }
		[Message(1)]
		public void MessageOneArgument(int arg) { }
		[Message("Fett")]
		public void MessageConflict(int arg) { }

		public void OnMessage<TId>(TId message) { }
	}

	public class DummyComponent3 : ComponentBase
	{
		[Message(0)]
		public void MessageNoArgument() { }
		[Message(1)]
		public void MessageOneArgument(int arg) { }
		[Message("Fett")]
		public void MessageConflict(string arg) { }
	}
}