using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using System;
using Pseudo;
using Pseudo.Internal.Entity;
using Pseudo.Internal;
using Pseudo.Internal.Communication;

namespace Pseudo.Tests
{
	public class EntityManagerTests
	{
		EntityManager entityManager;

		[SetUp]
		public void Setup()
		{
			entityManager = new EntityManager(new MessageManager());
		}

		[TearDown]
		public void TearDown()
		{
			entityManager = null;
		}

		#region Component
		[Test]
		public void AddComponent()
		{
			var entity = entityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			Assert.That(entity.Components.Count, Is.EqualTo(1));
		}

		[Test]
		public void RemoveComponent()
		{
			var entity = entityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			Assert.That(entity.Components.Count, Is.EqualTo(1));
			entity.RemoveComponent(component);
			Assert.That(entity.Components.Count, Is.EqualTo(0));
		}

		[Test]
		public void RemoveComponents()
		{
			var entity = entityManager.CreateEntity();
			var component1 = new DummyComponent1();
			var component2 = new DummyComponent1();
			var component3 = new DummyComponent1();

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entity.AddComponent(component3);
			Assert.That(entity.Components.Count, Is.EqualTo(3));
			entity.RemoveComponents<DummyComponent1>();
			Assert.That(entity.Components.Count, Is.EqualTo(0));

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entity.AddComponent(component3);
			Assert.That(entity.Components.Count, Is.EqualTo(3));
			entity.RemoveComponents(typeof(DummyComponent1));
			Assert.That(entity.Components.Count, Is.EqualTo(0));
		}

		[Test]
		public void RemoveAllComponents()
		{
			var entity = entityManager.CreateEntity();
			var component1 = new DummyComponent1();
			var component2 = new DummyComponent2();
			var component3 = new DummyComponent3();

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entity.AddComponent(component3);
			Assert.That(entity.Components.Count, Is.EqualTo(3));
			entity.RemoveAllComponents();
			Assert.That(entity.Components.Count, Is.EqualTo(0));
		}

		[Test]
		public void GetComponent()
		{
			var entity = entityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			Assert.That(entity.GetComponent<DummyComponent1>(), Is.EqualTo(component));
			Assert.That(entity.GetComponent(typeof(DummyComponent1)), Is.EqualTo(component));
		}

		[Test]
		public void GetComponents()
		{
			var entity = entityManager.CreateEntity();
			var component1 = new DummyComponent1();
			var component2 = new DummyComponent1();
			var component3 = new DummyComponent2();

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entity.AddComponent(component3);

			Assert.That(entity.GetComponents<DummyComponent1>().Count, Is.EqualTo(2));
			Assert.That(entity.GetComponents(typeof(DummyComponent2)).Count, Is.EqualTo(1));
		}

		[Test]
		public void HasComponent()
		{
			var entity = entityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			Assert.That(entity.HasComponent(component));
			Assert.That(entity.HasComponent(component.GetType()));
			Assert.That(entity.HasComponent<DummyComponent1>());
		}

		[Test]
		public void ComponentDuplicatesNotAllowed()
		{
			var entity = entityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			entity.AddComponent(component);
			entity.AddComponent(component);

			Assert.That(entity.Components.Count, Is.EqualTo(1));
			Assert.That(entity.GetComponents(component.GetType()).Count, Is.EqualTo(1));
		}
		#endregion

		#region Component With Scope
		[Test]
		public void GetComponentWithScope()
		{
			var entity1 = entityManager.CreateEntity();
			var entity2 = entityManager.CreateEntity();
			var entity3 = entityManager.CreateEntity();
			var entity4 = entityManager.CreateEntity();
			var component1 = new DummyComponent1();
			var component2 = new DummyComponent1();
			var component3 = new DummyComponent1();
			var component4 = new DummyComponent1();

			entity1.AddComponent(component1);
			entity2.AddComponent(component2);
			entity3.AddComponent(component3);
			entity4.AddComponent(component4);
			entity1.AddChild(entity2);
			entity1.AddChild(entity4);
			entity3.SetParent(entity2);

			Assert.That(entity1.GetComponent<DummyComponent1>(HierarchyScope.Local), Is.EqualTo(component1));
			Assert.That(entity1.GetComponent<DummyComponent1>(HierarchyScope.Children), Is.EqualTo(component2));
			Assert.That(entity1.GetComponent<DummyComponent1>(HierarchyScope.Parents), Is.Null);
			Assert.That(entity1.GetComponent<DummyComponent1>(HierarchyScope.Siblings), Is.Null);
			Assert.That(entity1.GetComponent<DummyComponent1>(HierarchyScope.Global), Is.EqualTo(component1));

			Assert.That(entity2.GetComponent<DummyComponent1>(HierarchyScope.Local), Is.EqualTo(component2));
			Assert.That(entity2.GetComponent<DummyComponent1>(HierarchyScope.Children), Is.EqualTo(component3));
			Assert.That(entity2.GetComponent<DummyComponent1>(HierarchyScope.Parents), Is.EqualTo(component1));
			Assert.That(entity2.GetComponent<DummyComponent1>(HierarchyScope.Siblings), Is.EqualTo(component4));
			Assert.That(entity2.GetComponent<DummyComponent1>(HierarchyScope.Global), Is.EqualTo(component1));

			Assert.That(entity3.GetComponent<DummyComponent1>(HierarchyScope.Local), Is.EqualTo(component3));
			Assert.That(entity3.GetComponent<DummyComponent1>(HierarchyScope.Children), Is.Null);
			Assert.That(entity3.GetComponent<DummyComponent1>(HierarchyScope.Parents), Is.EqualTo(component2));
			Assert.That(entity3.GetComponent<DummyComponent1>(HierarchyScope.Siblings), Is.Null);
			Assert.That(entity3.GetComponent<DummyComponent1>(HierarchyScope.Global), Is.EqualTo(component1));

			Assert.That(entity4.GetComponent<DummyComponent1>(HierarchyScope.Local), Is.EqualTo(component4));
			Assert.That(entity4.GetComponent<DummyComponent1>(HierarchyScope.Children), Is.Null);
			Assert.That(entity4.GetComponent<DummyComponent1>(HierarchyScope.Parents), Is.EqualTo(component1));
			Assert.That(entity4.GetComponent<DummyComponent1>(HierarchyScope.Siblings), Is.EqualTo(component2));
			Assert.That(entity4.GetComponent<DummyComponent1>(HierarchyScope.Global), Is.EqualTo(component1));
		}
		#endregion

		#region Group Match
		void GroupMatchSetup()
		{
			entityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(1)));
			entityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(1, 2)));
			entityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(2)));
			entityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(2, 3)));
			entityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(3)));
		}

		[Test]
		public void GroupMatchAll()
		{
			GroupMatchSetup();

			var entityGroup = entityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1)), EntityMatches.All);
			Assert.That(entityGroup.Count, Is.EqualTo(2));
		}

		[Test]
		public void GroupMatchAny()
		{
			GroupMatchSetup();

			var entityGroup = entityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.Any);
			Assert.That(entityGroup.Count, Is.EqualTo(4));
		}

		[Test]
		public void GroupMatchNone()
		{
			GroupMatchSetup();

			var entityGroup = entityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.None);
			Assert.That(entityGroup.Count, Is.EqualTo(1));
		}

		[Test]
		public void GroupMatchExact()
		{
			GroupMatchSetup();

			var entityGroup = entityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.Exact);
			Assert.That(entityGroup.Count, Is.EqualTo(1));
		}

		[Test]
		public void GroupChangeUpdate()
		{
			var entity = entityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(1, 2, 3)));
			var entityGroup = entityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.All);

			Assert.That(entityGroup.Count, Is.EqualTo(1));

			entity.Groups = EntityGroups.Nothing;

			Assert.That(entityGroup.Count, Is.EqualTo(0));
		}
		#endregion

		#region Component Group Match
		void ComponentGroupMatchSetup()
		{
			entityManager.CreateEntity().AddComponent(new DummyComponent1());
			entityManager.CreateEntity().AddComponent(new DummyComponent2());
			entityManager.CreateEntity().AddComponent(new DummyComponent2());
			entityManager.CreateEntity().AddComponent(new DummyComponent3());
			entityManager.CreateEntity().AddComponent(new DummyComponent3());
			entityManager.CreateEntity().AddComponent(new DummyComponent3());

			var entity = entityManager.CreateEntity();
			entity.AddComponent(new DummyComponent1());
			entity.AddComponent(new DummyComponent2());
			entity.AddComponent(new DummyComponent3());
		}

		[Test]
		public void ComponentGroupMatchAll()
		{
			ComponentGroupMatchSetup();

			var entityGroup1 = entityManager.Entities.Filter(typeof(DummyComponent1), EntityMatches.All);
			var entityGroup2 = entityManager.Entities.Filter(typeof(DummyComponent2), EntityMatches.All);
			var entityGroup3 = entityManager.Entities.Filter(typeof(DummyComponent3), EntityMatches.All);
			var entityGroup4 = entityManager.Entities.Filter(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.All);

			Assert.That(entityGroup1.Count, Is.EqualTo(2));
			Assert.That(entityGroup2.Count, Is.EqualTo(3));
			Assert.That(entityGroup3.Count, Is.EqualTo(4));
			Assert.That(entityGroup4.Count, Is.EqualTo(1));
		}

		[Test]
		public void ComponentGroupMatchAny()
		{
			ComponentGroupMatchSetup();

			var entityGroup1 = entityManager.Entities.Filter(typeof(DummyComponent1), EntityMatches.Any);
			var entityGroup2 = entityManager.Entities.Filter(typeof(DummyComponent2), EntityMatches.Any);
			var entityGroup3 = entityManager.Entities.Filter(typeof(DummyComponent3), EntityMatches.Any);
			var entityGroup4 = entityManager.Entities.Filter(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.Any);

			Assert.That(entityGroup1.Count, Is.EqualTo(2));
			Assert.That(entityGroup2.Count, Is.EqualTo(3));
			Assert.That(entityGroup3.Count, Is.EqualTo(4));
			Assert.That(entityGroup4.Count, Is.EqualTo(7));
		}

		[Test]
		public void ComponentGroupMatchNone()
		{
			ComponentGroupMatchSetup();

			var entityGroup1 = entityManager.Entities.Filter(typeof(DummyComponent1), EntityMatches.None);
			var entityGroup2 = entityManager.Entities.Filter(typeof(DummyComponent2), EntityMatches.None);
			var entityGroup3 = entityManager.Entities.Filter(typeof(DummyComponent3), EntityMatches.None);
			var entityGroup4 = entityManager.Entities.Filter(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.None);

			Assert.That(entityGroup1.Count, Is.EqualTo(5));
			Assert.That(entityGroup2.Count, Is.EqualTo(4));
			Assert.That(entityGroup3.Count, Is.EqualTo(3));
			Assert.That(entityGroup4.Count, Is.EqualTo(0));
		}

		[Test]
		public void ComponentGroupMatchExact()
		{
			ComponentGroupMatchSetup();

			var entityGroup1 = entityManager.Entities.Filter(typeof(DummyComponent1), EntityMatches.Exact);
			var entityGroup2 = entityManager.Entities.Filter(typeof(DummyComponent2), EntityMatches.Exact);
			var entityGroup3 = entityManager.Entities.Filter(typeof(DummyComponent3), EntityMatches.Exact);
			var entityGroup4 = entityManager.Entities.Filter(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.Exact);

			Assert.That(entityGroup1.Count, Is.EqualTo(1));
			Assert.That(entityGroup2.Count, Is.EqualTo(2));
			Assert.That(entityGroup3.Count, Is.EqualTo(3));
			Assert.That(entityGroup4.Count, Is.EqualTo(1));
		}

		[Test]
		public void ComponentGroupChangeUpdate()
		{
			var entity = entityManager.CreateEntity();
			entity.AddComponent(new DummyComponent1());
			entity.AddComponent(new DummyComponent2());
			entity.AddComponent(new DummyComponent3());

			var entityGroup = entityManager.Entities.Filter(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.Any);
			Assert.That(entityGroup.Count, Is.EqualTo(1));
			entity.RemoveAllComponents();
			Assert.That(entityGroup.Count, Is.EqualTo(0));
		}

		[Test]
		public void ComponentGroupMatchInheritance()
		{
			var entity = entityManager.CreateEntity();
			entity.AddComponent(Substitute.For<DummyComponent1>());

			var entityGroup = entityManager.Entities.Filter(typeof(DummyComponent1), EntityMatches.All);
			Assert.That(entityGroup.Count, Is.EqualTo(1));
		}
		#endregion

		#region Messages
		[Test]
		public void MessageNoArgument()
		{
			var entity = entityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entity.AddComponent(component3);
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
			var entity = entityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entity.AddComponent(component3);
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
			var entity = entityManager.CreateEntity();
			var component = Substitute.For<DummyComponent1>();

			entity.AddComponent(component);
			entity.SendMessage("Boba", component);

			component.Received(1).MessageInheritance(component);
			component.Received(1).OnMessage("Boba");
		}

		[Test]
		public void MessageWrongArgumentNumber()
		{
			var entity = entityManager.CreateEntity();
			var component = Substitute.For<DummyComponent1>();

			entity.AddComponent(component);
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
			var entity = entityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entity.AddComponent(component3);
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
			var entity = entityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entity.AddComponent(component3);

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
			var entity = entityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entity.AddComponent(component3);
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
			var entity = entityManager.CreateEntity();
			var component = Substitute.For<DummyComponent2>();

			entity.AddComponent(component);
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
			var entity1 = entityManager.CreateEntity();
			var entity2 = entityManager.CreateEntity();
			var entity3 = entityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity1.AddChild(entity2);
			entity3.SetParent(entity2);

			entity1.AddComponent(component1);
			entity2.AddComponent(component2);
			entity3.AddComponent(component3);

			entity1.SendMessage(0, HierarchyScope.Children | HierarchyScope.Local);
			entity2.SendMessage(1, 1, HierarchyScope.Children);

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
			var entity1 = entityManager.CreateEntity();
			var entity2 = entityManager.CreateEntity();
			var entity3 = entityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity1.AddChild(entity2);
			entity3.SetParent(entity2);

			entity1.AddComponent(component1);
			entity2.AddComponent(component2);
			entity3.AddComponent(component3);

			component1.Active = false;
			entity2.Active = false;

			entity1.SendMessage(0, HierarchyScope.Children | HierarchyScope.Local);
			entity2.SendMessage(1, 1, HierarchyScope.Children | HierarchyScope.Local);

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
			var entity1 = entityManager.CreateEntity();
			var entity2 = entityManager.CreateEntity();
			var entity3 = entityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity1.AddChild(entity2);
			entity3.SetParent(entity2);

			entity1.AddComponent(component1);
			entity2.AddComponent(component2);
			entity3.AddComponent(component3);

			entity1.SendMessage(0, HierarchyScope.Parents | HierarchyScope.Local);
			entity2.SendMessage(1, 1, HierarchyScope.Parents);

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
			var entity1 = entityManager.CreateEntity();
			var entity2 = entityManager.CreateEntity();
			var entity3 = entityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity1.AddChild(entity2);
			entity3.SetParent(entity1);

			entity1.AddComponent(component1);
			entity2.AddComponent(component2);
			entity3.AddComponent(component3);

			entity2.SendMessage(0, HierarchyScope.Siblings | HierarchyScope.Local);
			entity3.SendMessage(1, 1, HierarchyScope.Siblings);

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
			var entity1 = entityManager.CreateEntity();
			var entity2 = entityManager.CreateEntity();
			var entity3 = entityManager.CreateEntity();
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var component3 = Substitute.For<DummyComponent3>();

			entity1.AddChild(entity2);
			entity3.SetParent(entity2);

			entity1.AddComponent(component1);
			entity2.AddComponent(component2);
			entity3.AddComponent(component3);

			entityManager.Entities.BroadcastMessage(0);
			entityManager.Entities.BroadcastMessage(1, 1);

			component1.Received(1).MessageNoArgument();
			component1.Received(1).MessageOneArgument(1);
			component1.Received(3).OnMessage(0);

			component2.Received(1).MessageNoArgument();
			component2.Received(1).MessageOneArgument(1);
			component2.Received(3).OnMessage(0);

			component3.Received(1).MessageNoArgument();
			component3.Received(1).MessageOneArgument(1);
		}
		#endregion

		#region Group Messages
		[Test]
		public void GroupSendMessage()
		{
			var entity = entityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(1, 2, 3)));
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var entityGroup = entityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.All);

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entityGroup.BroadcastMessage(0);

			component1.Received(1).MessageNoArgument();
			component1.Received(1).OnMessage(0);
			component2.Received(1).MessageNoArgument();
			component2.Received(1).OnMessage(0);
		}

		[Test]
		public void GroupSendMessageInactive()
		{
			var entity = entityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(1, 2, 3)));
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var entityGroup = entityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.All);

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entity.Active = false;
			entityGroup.BroadcastMessage(0);

			component1.Received(0).MessageNoArgument();
			component1.Received(0).OnMessage(0);
			component2.Received(0).MessageNoArgument();
			component2.Received(0).OnMessage(0);
		}

		[Test]
		public void GroupSendMessageComponentInactive()
		{
			var entity = entityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(1, 2, 3)));
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var entityGroup = entityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.All);

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			component2.Active = false;
			entityGroup.BroadcastMessage(0);

			component1.Received(1).MessageNoArgument();
			component1.Received(1).OnMessage(0);
			component2.Received(0).MessageNoArgument();
			component2.Received(0).OnMessage(0);
		}
		#endregion

		#region Hierarchy
		[Test]
		public void HierarchyAddChild()
		{
			var entity1 = entityManager.CreateEntity();
			var entity2 = entityManager.CreateEntity();
			var entity3 = entityManager.CreateEntity();

			entity1.AddChild(entity2);
			entity3.SetParent(entity2);

			Assert.That(entity1.Parent, Is.Null);
			Assert.That(entity2.Parent, Is.EqualTo(entity1));
			Assert.That(entity3.Parent, Is.EqualTo(entity2));

			Assert.That(entity1.Children.First(), Is.EqualTo(entity2));
			Assert.That(entity2.Children.First(), Is.EqualTo(entity3));
			Assert.That(entity3.Children.First(), Is.Null);
		}

		[Test]
		public void HierarchyRemoveChild()
		{
			var entity1 = entityManager.CreateEntity();
			var entity2 = entityManager.CreateEntity();
			var entity3 = entityManager.CreateEntity();

			entity1.AddChild(entity2);
			entity3.SetParent(entity2);

			entity1.RemoveAllChildren();
			entity2.RemoveChild(entity3);

			Assert.That(entity1.Parent, Is.Null);
			Assert.That(entity2.Parent, Is.Null);
			Assert.That(entity3.Parent, Is.Null);

			Assert.That(entity1.Children.Count, Is.EqualTo(0));
			Assert.That(entity2.Children.Count, Is.EqualTo(0));
			Assert.That(entity3.Children.Count, Is.EqualTo(0));
		}
		#endregion

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
}