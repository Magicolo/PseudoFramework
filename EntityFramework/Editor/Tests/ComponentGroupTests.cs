using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.EntityFramework.Tests
{
	public class ComponentGroupTests : EntityFrameworkTestsBase
	{
		public override void Setup()
		{
			base.Setup();

			EntityManager.CreateEntity().Add(new DummyComponent1());
			EntityManager.CreateEntity().Add(new DummyComponent2());
			EntityManager.CreateEntity().Add(new DummyComponent2());
			EntityManager.CreateEntity().Add(new DummyComponent3());
			EntityManager.CreateEntity().Add(new DummyComponent3());
			EntityManager.CreateEntity().Add(new DummyComponent3());

			var entity = EntityManager.CreateEntity();
			entity.Add(new DummyComponent1());
			entity.Add(new DummyComponent2());
			entity.Add(new DummyComponent3());
		}

		[Test]
		public void ComponentGroupMatchAll()
		{
			var entityGroup1 = EntityManager.Entities.Filter(typeof(DummyComponent1), EntityMatches.All);
			var entityGroup2 = EntityManager.Entities.Filter(typeof(DummyComponent2), EntityMatches.All);
			var entityGroup3 = EntityManager.Entities.Filter(typeof(DummyComponent3), EntityMatches.All);
			var entityGroup4 = EntityManager.Entities.Filter(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.All);

			Assert.That(entityGroup1.Count, Is.EqualTo(2));
			Assert.That(entityGroup2.Count, Is.EqualTo(3));
			Assert.That(entityGroup3.Count, Is.EqualTo(4));
			Assert.That(entityGroup4.Count, Is.EqualTo(1));
		}

		[Test]
		public void ComponentGroupMatchAny()
		{
			var entityGroup1 = EntityManager.Entities.Filter(typeof(DummyComponent1), EntityMatches.Any);
			var entityGroup2 = EntityManager.Entities.Filter(typeof(DummyComponent2), EntityMatches.Any);
			var entityGroup3 = EntityManager.Entities.Filter(typeof(DummyComponent3), EntityMatches.Any);
			var entityGroup4 = EntityManager.Entities.Filter(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.Any);

			Assert.That(entityGroup1.Count, Is.EqualTo(2));
			Assert.That(entityGroup2.Count, Is.EqualTo(3));
			Assert.That(entityGroup3.Count, Is.EqualTo(4));
			Assert.That(entityGroup4.Count, Is.EqualTo(7));
		}

		[Test]
		public void ComponentGroupMatchNone()
		{
			var entityGroup1 = EntityManager.Entities.Filter(typeof(DummyComponent1), EntityMatches.None);
			var entityGroup2 = EntityManager.Entities.Filter(typeof(DummyComponent2), EntityMatches.None);
			var entityGroup3 = EntityManager.Entities.Filter(typeof(DummyComponent3), EntityMatches.None);
			var entityGroup4 = EntityManager.Entities.Filter(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.None);

			Assert.That(entityGroup1.Count, Is.EqualTo(5));
			Assert.That(entityGroup2.Count, Is.EqualTo(4));
			Assert.That(entityGroup3.Count, Is.EqualTo(3));
			Assert.That(entityGroup4.Count, Is.EqualTo(0));
		}

		[Test]
		public void ComponentGroupMatchExact()
		{
			var entityGroup1 = EntityManager.Entities.Filter(typeof(DummyComponent1), EntityMatches.Exact);
			var entityGroup2 = EntityManager.Entities.Filter(typeof(DummyComponent2), EntityMatches.Exact);
			var entityGroup3 = EntityManager.Entities.Filter(typeof(DummyComponent3), EntityMatches.Exact);
			var entityGroup4 = EntityManager.Entities.Filter(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.Exact);

			Assert.That(entityGroup1.Count, Is.EqualTo(1));
			Assert.That(entityGroup2.Count, Is.EqualTo(2));
			Assert.That(entityGroup3.Count, Is.EqualTo(3));
			Assert.That(entityGroup4.Count, Is.EqualTo(1));
		}

		[Test]
		public void ComponentGroupChangeUpdate()
		{
			var entity = EntityManager.CreateEntity();
			entity.Add(new DummyComponent1());
			entity.Add(new DummyComponent2());
			entity.Add(new DummyComponent3());

			var entityGroup = EntityManager.Entities.Filter(new[] { typeof(DummyComponent1), typeof(DummyComponent2), typeof(DummyComponent3) }, EntityMatches.Any);
			Assert.That(entityGroup.Count, Is.EqualTo(8));
			entity.RemoveAll();
			Assert.That(entityGroup.Count, Is.EqualTo(7));
		}

		[Test]
		public void ComponentGroupMatchInheritance()
		{
			var entityGroup = EntityManager.Entities.Filter(typeof(IComponent), EntityMatches.All);
			Assert.That(entityGroup.Count, Is.EqualTo(7));
		}
	}
}
