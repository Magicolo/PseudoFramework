using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.EntityFramework.Tests
{
	public class MessageTests : EntityFrameworkTestsBase
	{
		[Test]
		public void MessageNoArgument()
		{
			var entity = EntityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity.Add(component1);
			entity.Add(component2);
			entity.Add(component3);
			entity.SendMessage(0);

			component1.Received(1).MessageNoArgument();
			component2.Received(1).MessageNoArgument();
			component3.Received(1).MessageNoArgument();
			component1.Received(0).MessageInheritance(null);
			component1.Received(1).OnMessage(0);
			component1.Received(0).OnMessage("");
		}

		[Test]
		public void MessageOneArgument()
		{
			var entity = EntityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity.Add(component1);
			entity.Add(component2);
			entity.Add(component3);
			entity.SendMessage(1, 1);

			component1.Received(1).MessageOneArgument(1);
			component2.Received(1).MessageOneArgument(1);
			component3.Received(1).MessageOneArgument(1);
			component1.Received(0).MessageInheritance(null);
			component1.Received(1).OnMessage(1);
			component1.Received(0).OnMessage("");
		}

		[Test]
		public void MessageInheritance()
		{
			var entity = EntityManager.CreateEntity();
			var component = Substitute.For<DummyComponent1>();

			entity.Add(component);
			entity.SendMessage("Boba", component);

			component.Received(1).MessageInheritance(component);
			component.Received(1).OnMessage("Boba");
		}

		[Test]
		public void MessageWrongArgumentNumber()
		{
			var entity = EntityManager.CreateEntity();
			var component = Substitute.For<DummyComponent1>();

			entity.Add(component);
			entity.SendMessage(0);
			entity.SendMessage(0, 1);

			entity.SendMessage(1);
			entity.SendMessage(1, 1);

			component.Received(4).MessageNoArgument();
			component.Received(4).MessageOneArgument(1);
			component.Received(0).MessageInheritance(null);
			component.Received(12).OnMessage(0);
			component.Received(0).OnMessage("");
		}

		[Test]
		public void MessageConflictingArguments()
		{
			var entity = EntityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity.Add(component1);
			entity.Add(component2);
			entity.Add(component3);
			entity.SendMessage("Fett");

			component1.Received(1).MessageConflict();
			component2.Received(1).MessageConflict(1);
			component3.Received(1).MessageConflict("");
			component1.Received(0).MessageInheritance(null);
			component1.Received(0).OnMessage(0);
			component1.Received(1).OnMessage("Fett");
		}

		[Test]
		public void MessageInactiveComponent()
		{
			var entity = EntityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity.Add(component1);
			entity.Add(component2);
			entity.Add(component3);

			component1.Active = false;
			component2.Active = false;
			entity.SendMessage(0);

			component1.Received(0).MessageNoArgument();
			component2.Received(0).MessageNoArgument();
			component3.Received(1).MessageNoArgument();
			component1.Received(0).MessageInheritance(null);
			component1.Received(0).OnMessage(0);
			component1.Received(0).OnMessage("Fett");
		}

		[Test]
		public void MessageInactiveEntity()
		{
			var entity = EntityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity.Add(component1);
			entity.Add(component2);
			entity.Add(component3);
			entity.Active = false;
			entity.SendMessage(0);

			component1.Received(0).MessageNoArgument();
			component2.Received(0).MessageNoArgument();
			component3.Received(0).MessageNoArgument();
			component1.Received(0).MessageInheritance(null);
			component1.Received(0).OnMessage(0);
			component1.Received(0).OnMessage("Fett");
		}

		[Test]
		public void MessageReceiveAll()
		{
			var entity = EntityManager.CreateEntity();
			var component = Substitute.For<DummyComponent2>();

			entity.Add(component);
			entity.SendMessage(0);
			entity.SendMessage(0f);
			entity.SendMessage(0u);
			entity.SendMessage(true);
			entity.SendMessage("Jango");

			component.Received(5).OnMessage(0);
		}

		[Test]
		public void MessagePropagateDownwards()
		{
			var entity1 = EntityManager.CreateEntity();
			var entity2 = EntityManager.CreateEntity();
			var entity3 = EntityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity1.AddChild(entity2);
			entity3.SetParent(entity2);

			entity1.Add(component1);
			entity2.Add(component2);
			entity3.Add(component3);

			entity1.SendMessage(0, HierarchyScopes.Children | HierarchyScopes.Self);
			entity2.SendMessage(1, 1, HierarchyScopes.Children);

			component1.Received(1).MessageNoArgument();
			component1.Received(0).MessageOneArgument(1);
			component1.Received(1).OnMessage(0);

			component2.Received(1).MessageNoArgument();
			component2.Received(0).MessageOneArgument(1);
			component2.Received(2).OnMessage(0);

			component3.Received(1).MessageNoArgument();
			component3.Received(1).MessageOneArgument(1);
		}

		[Test]
		public void MessagePropagateDownwardsInactive()
		{
			var entity1 = EntityManager.CreateEntity();
			var entity2 = EntityManager.CreateEntity();
			var entity3 = EntityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity1.AddChild(entity2);
			entity3.SetParent(entity2);

			entity1.Add(component1);
			entity2.Add(component2);
			entity3.Add(component3);

			component1.Active = false;
			entity2.Active = false;

			entity1.SendMessage(0, HierarchyScopes.Children | HierarchyScopes.Self);
			entity2.SendMessage(1, 1, HierarchyScopes.Children | HierarchyScopes.Self);

			component1.Received(0).MessageNoArgument();
			component1.Received(0).MessageOneArgument(1);
			component1.Received(0).OnMessage(0);

			component2.Received(0).MessageNoArgument();
			component2.Received(0).MessageOneArgument(1);
			component2.Received(0).OnMessage(0);

			component3.Received(1).MessageNoArgument();
			component3.Received(1).MessageOneArgument(1);
		}

		[Test]
		public void MessagePropagateUpwards()
		{
			var entity1 = EntityManager.CreateEntity();
			var entity2 = EntityManager.CreateEntity();
			var entity3 = EntityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity1.AddChild(entity2);
			entity3.SetParent(entity2);

			entity1.Add(component1);
			entity2.Add(component2);
			entity3.Add(component3);

			entity1.SendMessage(0, HierarchyScopes.Ancestors | HierarchyScopes.Self);
			entity2.SendMessage(1, 1, HierarchyScopes.Ancestors);

			component1.Received(1).MessageNoArgument();
			component1.Received(1).MessageOneArgument(1);
			component1.Received(3).OnMessage(0);

			component2.Received(0).MessageNoArgument();
			component2.Received(0).MessageOneArgument(1);
			component2.Received(2).OnMessage(0);

			component3.Received(0).MessageNoArgument();
			component3.Received(0).MessageOneArgument(1);
		}

		[Test]
		public void MessagePropagateLateral()
		{
			var entity1 = EntityManager.CreateEntity();
			var entity2 = EntityManager.CreateEntity();
			var entity3 = EntityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity1.AddChild(entity2);
			entity3.SetParent(entity1);

			entity1.Add(component1);
			entity2.Add(component2);
			entity3.Add(component3);

			entity2.SendMessage(0, HierarchyScopes.Siblings | HierarchyScopes.Self);
			entity3.SendMessage(1, 1, HierarchyScopes.Siblings);

			component1.Received(0).MessageNoArgument();
			component1.Received(0).MessageOneArgument(1);
			component1.Received(0).OnMessage(0);

			component2.Received(2).MessageNoArgument();
			component2.Received(1).MessageOneArgument(1);
			component2.Received(2).OnMessage(0);

			component3.Received(1).MessageNoArgument();
			component3.Received(0).MessageOneArgument(1);
		}

		[Test]
		public void MessagePropagateGlobal()
		{
			var entity1 = EntityManager.CreateEntity();
			var entity2 = EntityManager.CreateEntity();
			var entity3 = EntityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity1.AddChild(entity2);
			entity3.SetParent(entity2);

			entity1.Add(component1);
			entity2.Add(component2);
			entity3.Add(component3);

			EntityManager.Entities.BroadcastMessage(0);
			EntityManager.Entities.BroadcastMessage(1, 1);

			component1.Received(1).MessageNoArgument();
			component1.Received(1).MessageOneArgument(1);
			component1.Received(3).OnMessage(0);

			component2.Received(1).MessageNoArgument();
			component2.Received(1).MessageOneArgument(1);
			component2.Received(3).OnMessage(0);

			component3.Received(1).MessageNoArgument();
			component3.Received(1).MessageOneArgument(1);
		}
	}
}
