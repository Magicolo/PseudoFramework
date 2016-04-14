using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.EntityFramework.Tests
{
	public class ComponentTests : EntityFrameworkTestsBase
	{
		[Test]
		public void AddComponent()
		{
			var entity = EntityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.Add(component);
			Assert.That(entity.Count, Is.EqualTo(1));
		}

		[Test]
		public void RemoveComponent()
		{
			var entity = EntityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.Add(component);
			Assert.That(entity.Count, Is.EqualTo(1));
			entity.Remove(component);
			Assert.That(entity.Count, Is.EqualTo(0));
		}

		[Test]
		public void RemoveComponents()
		{
			var entity = EntityManager.CreateEntity();
			var component1 = new DummyComponent1();
			var component2 = new DummyComponent1();
			var component3 = new DummyComponent1();

			entity.Add(component1);
			entity.Add(component2);
			entity.Add(component3);
			Assert.That(entity.Count, Is.EqualTo(3));
			entity.RemoveAll<DummyComponent1>();
			Assert.That(entity.Count, Is.EqualTo(0));

			entity.Add(component1);
			entity.Add(component2);
			entity.Add(component3);
			Assert.That(entity.Count, Is.EqualTo(3));
			entity.RemoveAll(typeof(DummyComponent1));
			Assert.That(entity.Count, Is.EqualTo(0));
		}

		[Test]
		public void RemoveAllComponents()
		{
			var entity = EntityManager.CreateEntity();
			var component1 = new DummyComponent1();
			var component2 = new DummyComponent2();
			var component3 = new DummyComponent3();

			entity.Add(component1);
			entity.Add(component2);
			entity.Add(component3);
			Assert.That(entity.Count, Is.EqualTo(3));
			entity.RemoveAll();
			Assert.That(entity.Count, Is.EqualTo(0));
		}

		[Test]
		public void GetComponent()
		{
			var entity = EntityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.Add(component);
			Assert.That(entity.Get<DummyComponent1>(), Is.EqualTo(component));
			Assert.That(entity.Get(typeof(DummyComponent1)), Is.EqualTo(component));
		}

		[Test]
		public void GetComponents()
		{
			var entity = EntityManager.CreateEntity();
			var component1 = new DummyComponent1();
			var component2 = new DummyComponent1();
			var component3 = new DummyComponent2();

			entity.Add(component1);
			entity.Add(component2);
			entity.Add(component3);

			Assert.That(entity.GetAll<DummyComponent1>().Count, Is.EqualTo(2));
			Assert.That(entity.GetAll(typeof(DummyComponent2)).Count, Is.EqualTo(1));
		}

		[Test]
		public void HasComponent()
		{
			var entity = EntityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.Add(component);
			Assert.That(entity.Has(component));
			Assert.That(entity.Has(component.GetType()));
			Assert.That(entity.Has<DummyComponent1>());
		}

		[Test]
		public void ComponentDuplicatesNotAllowed()
		{
			var entity = EntityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.Add(component);
			entity.Add(component);
			entity.Add(component);

			Assert.That(entity.Count, Is.EqualTo(1));
			Assert.That(entity.GetAll(component.GetType()).Count, Is.EqualTo(1));
		}

		[Test]
		public void GetComponentWithScope()
		{
			var entity1 = EntityManager.CreateEntity();
			var entity2 = EntityManager.CreateEntity();
			var entity3 = EntityManager.CreateEntity();
			var entity4 = EntityManager.CreateEntity();
			var component1 = new DummyComponent1();
			var component2 = new DummyComponent1();
			var component3 = new DummyComponent1();
			var component4 = new DummyComponent1();

			entity1.Add(component1);
			entity2.Add(component2);
			entity3.Add(component3);
			entity4.Add(component4);
			entity1.AddChild(entity2);
			entity1.AddChild(entity4);
			entity3.SetParent(entity2);

			Assert.That(entity1.Get<DummyComponent1>(HierarchyScopes.Self), Is.EqualTo(component1));
			Assert.That(entity1.Get<DummyComponent1>(HierarchyScopes.Children), Is.EqualTo(component2));
			Assert.That(entity1.Get<DummyComponent1>(HierarchyScopes.Ancestors), Is.Null);
			Assert.That(entity1.Get<DummyComponent1>(HierarchyScopes.Siblings), Is.Null);
			Assert.That(entity1.Get<DummyComponent1>(HierarchyScopes.Hierarchy), Is.EqualTo(component1));

			Assert.That(entity2.Get<DummyComponent1>(HierarchyScopes.Self), Is.EqualTo(component2));
			Assert.That(entity2.Get<DummyComponent1>(HierarchyScopes.Children), Is.EqualTo(component3));
			Assert.That(entity2.Get<DummyComponent1>(HierarchyScopes.Ancestors), Is.EqualTo(component1));
			Assert.That(entity2.Get<DummyComponent1>(HierarchyScopes.Siblings), Is.EqualTo(component4));
			Assert.That(entity2.Get<DummyComponent1>(HierarchyScopes.Hierarchy), Is.EqualTo(component1));

			Assert.That(entity3.Get<DummyComponent1>(HierarchyScopes.Self), Is.EqualTo(component3));
			Assert.That(entity3.Get<DummyComponent1>(HierarchyScopes.Children), Is.Null);
			Assert.That(entity3.Get<DummyComponent1>(HierarchyScopes.Ancestors), Is.EqualTo(component2));
			Assert.That(entity3.Get<DummyComponent1>(HierarchyScopes.Siblings), Is.Null);
			Assert.That(entity3.Get<DummyComponent1>(HierarchyScopes.Hierarchy), Is.EqualTo(component1));

			Assert.That(entity4.Get<DummyComponent1>(HierarchyScopes.Self), Is.EqualTo(component4));
			Assert.That(entity4.Get<DummyComponent1>(HierarchyScopes.Children), Is.Null);
			Assert.That(entity4.Get<DummyComponent1>(HierarchyScopes.Ancestors), Is.EqualTo(component1));
			Assert.That(entity4.Get<DummyComponent1>(HierarchyScopes.Siblings), Is.EqualTo(component2));
			Assert.That(entity4.Get<DummyComponent1>(HierarchyScopes.Hierarchy), Is.EqualTo(component1));
		}
	}
}
