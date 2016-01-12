using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using Pseudo.Internal.Entity3;
using System;

namespace Tests
{
	public class EntityTests
	{
		ISystemManager systemManager;
		IEntityManager entityManager;

		[SetUp]
		public void EntityTestsSetup()
		{
			systemManager = new SystemManager();
			entityManager = new EntityManager();
		}

		[TearDown]
		public void EntityTestsTearDown()
		{
			systemManager = null;
			entityManager = null;
		}

		#region Entity General
		[Test]
		public void EntityAddComponent()
		{
			var entity = entityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			Assert.That(entity.AllComponents.Count == 1);
		}

		[Test]
		public void EntityRemoveComponent()
		{
			var entity = entityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			Assert.That(entity.AllComponents.Count == 1);
			entity.RemoveComponent(component);
			Assert.That(entity.AllComponents.Count == 0);
		}

		[Test]
		public void EntityRemoveComponents()
		{
			var entity = entityManager.CreateEntity();
			var component1 = new DummyComponent1();
			var component2 = new DummyComponent1();
			var component3 = new DummyComponent1();

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entity.AddComponent(component3);
			Assert.That(entity.AllComponents.Count == 3);
			entity.RemoveComponents<DummyComponent1>();
			Assert.That(entity.AllComponents.Count == 0);

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entity.AddComponent(component3);
			Assert.That(entity.AllComponents.Count == 3);
			entity.RemoveComponents(typeof(DummyComponent1));
			Assert.That(entity.AllComponents.Count == 0);
		}

		[Test]
		public void EntityRemoveAllComponents()
		{
			var entity = entityManager.CreateEntity();
			var component1 = new DummyComponent1();
			var component2 = new DummyComponent2();
			var component3 = new DummyComponent3();

			entity.AddComponent(component1);
			entity.AddComponent(component2);
			entity.AddComponent(component3);
			Assert.That(entity.AllComponents.Count == 3);
			entity.RemoveAllComponents();
			Assert.That(entity.AllComponents.Count == 0);
		}

		[Test]
		public void EntityGetComponent()
		{
			var entity = entityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			Assert.That(entity.GetComponent<DummyComponent1>() == component);
			Assert.That(entity.GetComponent(typeof(DummyComponent1)) == component);
		}

		[Test]
		public void EntityGetComponents()
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
		public void EntityHasComponent()
		{
			var entity = entityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			Assert.That(entity.HasComponent(component));
			Assert.That(entity.HasComponent(component.GetType()));
			Assert.That(entity.HasComponent<DummyComponent1>());
		}

		[Test]
		public void EntityComponentDuplicatesNotAllowed()
		{
			var entity = entityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			entity.AddComponent(component);
			entity.AddComponent(component);

			Assert.That(entity.AllComponents.Count == 1);
			Assert.That(entity.GetComponents(component.GetType()).Count == 1);
		}
		#endregion

		#region Entity Group Match
		void EntityGroupMatchSetup()
		{
			entityManager.CreateEntity(Pseudo.ByteFlag.Nothing);
			entityManager.CreateEntity(Pseudo.ByteFlag.Everything);
			entityManager.CreateEntity(new Pseudo.ByteFlag(1));
			entityManager.CreateEntity(new Pseudo.ByteFlag(1, 2));
			entityManager.CreateEntity(new Pseudo.ByteFlag(2));
			entityManager.CreateEntity(new Pseudo.ByteFlag(2, 3));
			entityManager.CreateEntity(new Pseudo.ByteFlag(3));
		}

		[Test]
		public void EntityGroupMatchAll()
		{
			EntityGroupMatchSetup();

			var entityGroup = entityManager.GetEntityGroup(new Pseudo.ByteFlag(1), EntityMatches.All);
			Assert.That(entityGroup.Entities.Count == 3);
		}

		[Test]
		public void EntityGroupMatchAny()
		{
			EntityGroupMatchSetup();

			var entityGroup = entityManager.GetEntityGroup(new Pseudo.ByteFlag(1, 2), EntityMatches.Any);
			Assert.That(entityGroup.Entities.Count == 5);
		}

		[Test]
		public void EntityGroupMatchNone()
		{
			EntityGroupMatchSetup();

			var entityGroup = entityManager.GetEntityGroup(new Pseudo.ByteFlag(1, 2), EntityMatches.None);
			Assert.That(entityGroup.Entities.Count == 2);
		}

		[Test]
		public void EntityGroupMatchExact()
		{
			EntityGroupMatchSetup();

			var entityGroup = entityManager.GetEntityGroup(new Pseudo.ByteFlag(1, 2), EntityMatches.Exact);
			Assert.That(entityGroup.Entities.Count == 1);
		}

		[Test]
		public void EntityGroupChangeUpdate()
		{
			var entity = entityManager.CreateEntity(new Pseudo.ByteFlag(1, 2, 3));
			var entityGroup = entityManager.GetEntityGroup(new Pseudo.ByteFlag(1, 2), EntityMatches.All);

			Assert.That(entityGroup.Entities.Count == 1);

			entity.Groups = Pseudo.ByteFlag.Nothing;

			Assert.That(entityGroup.Entities.Count == 0);
		}
		#endregion

		#region Entity Component Group Match
		void EntityComponentGroupMatchSetup()
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
		public void EntityComponentGroupMatchAll()
		{
			EntityComponentGroupMatchSetup();

			var entityGroup1 = entityManager.GetEntityGroup(typeof(DummyComponent1), EntityMatches.All);
			var entityGroup2 = entityManager.GetEntityGroup(typeof(DummyComponent2), EntityMatches.All);
			var entityGroup3 = entityManager.GetEntityGroup(typeof(DummyComponent3), EntityMatches.All);
			var entityGroup4 = entityManager.GetEntityGroup(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.All);

			Assert.That(entityGroup1.Entities.Count == 2);
			Assert.That(entityGroup2.Entities.Count == 3);
			Assert.That(entityGroup3.Entities.Count == 4);
			Assert.That(entityGroup4.Entities.Count == 1);
		}

		[Test]
		public void EntityComponentGroupMatchAny()
		{
			EntityComponentGroupMatchSetup();

			var entityGroup1 = entityManager.GetEntityGroup(typeof(DummyComponent1), EntityMatches.Any);
			var entityGroup2 = entityManager.GetEntityGroup(typeof(DummyComponent2), EntityMatches.Any);
			var entityGroup3 = entityManager.GetEntityGroup(typeof(DummyComponent3), EntityMatches.Any);
			var entityGroup4 = entityManager.GetEntityGroup(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.Any);

			Assert.That(entityGroup1.Entities.Count == 2);
			Assert.That(entityGroup2.Entities.Count == 3);
			Assert.That(entityGroup3.Entities.Count == 4);
			Assert.That(entityGroup4.Entities.Count == 7);
		}

		[Test]
		public void EntityComponentGroupMatchNone()
		{
			EntityComponentGroupMatchSetup();

			var entityGroup1 = entityManager.GetEntityGroup(typeof(DummyComponent1), EntityMatches.None);
			var entityGroup2 = entityManager.GetEntityGroup(typeof(DummyComponent2), EntityMatches.None);
			var entityGroup3 = entityManager.GetEntityGroup(typeof(DummyComponent3), EntityMatches.None);
			var entityGroup4 = entityManager.GetEntityGroup(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.None);

			Assert.That(entityGroup1.Entities.Count == 5);
			Assert.That(entityGroup2.Entities.Count == 4);
			Assert.That(entityGroup3.Entities.Count == 3);
			Assert.That(entityGroup4.Entities.Count == 0);
		}

		[Test]
		public void EntityComponentGroupMatchExact()
		{
			EntityComponentGroupMatchSetup();

			var entityGroup1 = entityManager.GetEntityGroup(typeof(DummyComponent1), EntityMatches.Exact);
			var entityGroup2 = entityManager.GetEntityGroup(typeof(DummyComponent2), EntityMatches.Exact);
			var entityGroup3 = entityManager.GetEntityGroup(typeof(DummyComponent3), EntityMatches.Exact);
			var entityGroup4 = entityManager.GetEntityGroup(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.Exact);

			Assert.That(entityGroup1.Entities.Count == 1);
			Assert.That(entityGroup2.Entities.Count == 2);
			Assert.That(entityGroup3.Entities.Count == 3);
			Assert.That(entityGroup4.Entities.Count == 1);
		}

		[Test]
		public void EntityComponentGroupChangeUpdate()
		{
			var entity = entityManager.CreateEntity();
			entity.AddComponent(new DummyComponent1());
			entity.AddComponent(new DummyComponent2());
			entity.AddComponent(new DummyComponent3());

			var entityGroup = entityManager.GetEntityGroup(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.Any);
			Assert.That(entityGroup.Entities.Count == 1);
			entity.RemoveAllComponents();
			Assert.That(entityGroup.Entities.Count == 0);
		}
		#endregion

		#region Systems General
		[Test]
		public void SystemAdd()
		{
			var system = Substitute.For<ISystem>();

			systemManager.AddSystem(system);
			Assert.That(systemManager.AllSystems.Count == 1);
		}

		[Test]
		public void SystemRemove()
		{
			var system = Substitute.For<ISystem>();

			systemManager.AddSystem(system);
			Assert.That(systemManager.AllSystems.Count == 1);
			systemManager.RemoveSystem(system);
			Assert.That(systemManager.AllSystems.Count == 0);
		}

		[Test]
		public void SystemRemoveAll()
		{
			var system = Substitute.For<ISystem>();

			systemManager.AddSystem(system);
			Assert.That(systemManager.AllSystems.Count == 1);
			systemManager.RemoveAllSystems();
			Assert.That(systemManager.AllSystems.Count == 0);
		}
		#endregion

		#region Systems Update
		[Test]
		public void SystemUpdate()
		{
			var system = Substitute.For<ISystem, Pseudo.IUpdateable>();
			var updateable = (Pseudo.IUpdateable)system;
			updateable.Active = true;

			systemManager.AddSystem(system);
			systemManager.Update();
			systemManager.FixedUpdate();
			systemManager.LateUpdate();

			updateable.Received(1).Update();
		}

		[Test]
		public void SystemFixedUpdate()
		{
			var system = Substitute.For<ISystem, Pseudo.IFixedUpdateable>();
			var fixedUpdateable = (Pseudo.IFixedUpdateable)system;
			fixedUpdateable.Active = true;

			systemManager.AddSystem(system);
			systemManager.Update();
			systemManager.FixedUpdate();
			systemManager.LateUpdate();

			fixedUpdateable.Received(1).FixedUpdate();
		}

		[Test]
		public void SystemLateUpdate()
		{
			var system = Substitute.For<ISystem, Pseudo.ILateUpdateable>();
			var lateUpdateable = (Pseudo.ILateUpdateable)system;
			lateUpdateable.Active = true;

			systemManager.AddSystem(system);
			systemManager.Update();
			systemManager.FixedUpdate();
			systemManager.LateUpdate();

			lateUpdateable.Received(1).LateUpdate();
		}
		#endregion

		public class DummyComponent1 : IComponent { }
		public class DummyComponent2 : IComponent { }
		public class DummyComponent3 : IComponent { }
	}
}