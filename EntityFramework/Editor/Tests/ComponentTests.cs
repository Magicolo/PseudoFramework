﻿using NUnit.Framework;
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

			entity.AddComponent(component);
			Assert.That(entity.Components.Count, Is.EqualTo(1));
		}

		[Test]
		public void RemoveComponent()
		{
			var entity = EntityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			Assert.That(entity.Components.Count, Is.EqualTo(1));
			entity.RemoveComponent(component);
			Assert.That(entity.Components.Count, Is.EqualTo(0));
		}

		[Test]
		public void RemoveComponents()
		{
			var entity = EntityManager.CreateEntity();
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
			var entity = EntityManager.CreateEntity();
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
			var entity = EntityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			Assert.That(entity.GetComponent<DummyComponent1>(), Is.EqualTo(component));
			Assert.That(entity.GetComponent(typeof(DummyComponent1)), Is.EqualTo(component));
		}

		[Test]
		public void GetComponents()
		{
			var entity = EntityManager.CreateEntity();
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
			var entity = EntityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			Assert.That(entity.HasComponent(component));
			Assert.That(entity.HasComponent(component.GetType()));
			Assert.That(entity.HasComponent<DummyComponent1>());
		}

		[Test]
		public void ComponentDuplicatesNotAllowed()
		{
			var entity = EntityManager.CreateEntity();
			var component = new DummyComponent1();

			entity.AddComponent(component);
			entity.AddComponent(component);
			entity.AddComponent(component);

			Assert.That(entity.Components.Count, Is.EqualTo(1));
			Assert.That(entity.GetComponents(component.GetType()).Count, Is.EqualTo(1));
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
	}
}
