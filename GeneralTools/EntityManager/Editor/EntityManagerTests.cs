using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using System;
using Zenject;
using Pseudo;

namespace Tests
{
	public class EntityManagerTests
	{
		EntityManager entityManager;

		[SetUp]
		public void Setup()
		{
			entityManager = new EntityManager();
		}

		[TearDown]
		public void TearDown()
		{
			entityManager = null;
		}

		#region Entity General
		[Test]
		public void AddComponent()
		{
			var entity = entityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			Assert.That(entity.Components.Count == 1);
		}

		[Test]
		public void RemoveComponent()
		{
			var entity = entityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			Assert.That(entity.Components.Count == 1);
			entity.RemoveComponent(component);
			Assert.That(entity.Components.Count == 0);
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
			Assert.That(entity.Components.Count == 3);
			entity.RemoveComponents<DummyComponent1>();
			Assert.That(entity.Components.Count == 0);

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entity.AddComponent(component3);
			Assert.That(entity.Components.Count == 3);
			entity.RemoveComponents(typeof(DummyComponent1));
			Assert.That(entity.Components.Count == 0);
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
			Assert.That(entity.Components.Count == 3);
			entity.RemoveAllComponents();
			Assert.That(entity.Components.Count == 0);
		}

		[Test]
		public void GetComponent()
		{
			var entity = entityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			Assert.That(entity.GetComponent<DummyComponent1>() == component);
			Assert.That(entity.GetComponent(typeof(DummyComponent1)) == component);
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

			Assert.That(entity.GetComponents<DummyComponent1>().Count == 2);
			Assert.That(entity.GetComponents(typeof(DummyComponent2)).Count == 1);
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

			Assert.That(entity.Components.Count == 1);
			Assert.That(entity.GetComponents(component.GetType()).Count == 1);
		}
		#endregion

		#region Entity Group Match
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
			Assert.That(entityGroup.Count == 2);
		}

		[Test]
		public void GroupMatchAny()
		{
			GroupMatchSetup();

			var entityGroup = entityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.Any);
			Assert.That(entityGroup.Count == 4);
		}

		[Test]
		public void GroupMatchNone()
		{
			GroupMatchSetup();

			var entityGroup = entityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.None);
			Assert.That(entityGroup.Count == 1);
		}

		[Test]
		public void GroupMatchExact()
		{
			GroupMatchSetup();

			var entityGroup = entityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.Exact);
			Assert.That(entityGroup.Count == 1);
		}

		[Test]
		public void GroupChangeUpdate()
		{
			var entity = entityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(1, 2, 3)));
			var entityGroup = entityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.All);

			Assert.That(entityGroup.Count == 1);

			entity.Groups = EntityGroups.Nothing;

			Assert.That(entityGroup.Count == 0);
		}
		#endregion

		#region Entity Component Group Match
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

			Assert.That(entityGroup1.Count == 2);
			Assert.That(entityGroup2.Count == 3);
			Assert.That(entityGroup3.Count == 4);
			Assert.That(entityGroup4.Count == 1);
		}

		[Test]
		public void ComponentGroupMatchAny()
		{
			ComponentGroupMatchSetup();

			var entityGroup1 = entityManager.Entities.Filter(typeof(DummyComponent1), EntityMatches.Any);
			var entityGroup2 = entityManager.Entities.Filter(typeof(DummyComponent2), EntityMatches.Any);
			var entityGroup3 = entityManager.Entities.Filter(typeof(DummyComponent3), EntityMatches.Any);
			var entityGroup4 = entityManager.Entities.Filter(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.Any);

			Assert.That(entityGroup1.Count == 2);
			Assert.That(entityGroup2.Count == 3);
			Assert.That(entityGroup3.Count == 4);
			Assert.That(entityGroup4.Count == 7);
		}

		[Test]
		public void ComponentGroupMatchNone()
		{
			ComponentGroupMatchSetup();

			var entityGroup1 = entityManager.Entities.Filter(typeof(DummyComponent1), EntityMatches.None);
			var entityGroup2 = entityManager.Entities.Filter(typeof(DummyComponent2), EntityMatches.None);
			var entityGroup3 = entityManager.Entities.Filter(typeof(DummyComponent3), EntityMatches.None);
			var entityGroup4 = entityManager.Entities.Filter(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.None);

			Assert.That(entityGroup1.Count == 5);
			Assert.That(entityGroup2.Count == 4);
			Assert.That(entityGroup3.Count == 3);
			Assert.That(entityGroup4.Count == 0);
		}

		[Test]
		public void ComponentGroupMatchExact()
		{
			ComponentGroupMatchSetup();

			var entityGroup1 = entityManager.Entities.Filter(typeof(DummyComponent1), EntityMatches.Exact);
			var entityGroup2 = entityManager.Entities.Filter(typeof(DummyComponent2), EntityMatches.Exact);
			var entityGroup3 = entityManager.Entities.Filter(typeof(DummyComponent3), EntityMatches.Exact);
			var entityGroup4 = entityManager.Entities.Filter(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.Exact);

			Assert.That(entityGroup1.Count == 1);
			Assert.That(entityGroup2.Count == 2);
			Assert.That(entityGroup3.Count == 3);
			Assert.That(entityGroup4.Count == 1);
		}

		[Test]
		public void ComponentGroupChangeUpdate()
		{
			var entity = entityManager.CreateEntity();
			entity.AddComponent(new DummyComponent1());
			entity.AddComponent(new DummyComponent2());
			entity.AddComponent(new DummyComponent3());

			var entityGroup = entityManager.Entities.Filter(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.Any);
			Assert.That(entityGroup.Count == 1);
			entity.RemoveAllComponents();
			Assert.That(entityGroup.Count == 0);
		}

		[Test]
		public void ComponentGroupMatchInheritance()
		{
			var entity = entityManager.CreateEntity();
			entity.AddComponent(Substitute.For<DummyComponent1>());

			var entityGroup = entityManager.Entities.Filter(typeof(DummyComponent1), EntityMatches.All);
			Assert.That(entityGroup.Count == 1);
		}
		#endregion

		public class DummyComponent1 : IComponent { }
		public class DummyComponent2 : IComponent { }
		public class DummyComponent3 : IComponent { }
	}
}