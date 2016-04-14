using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.EntityFramework.Tests
{
	public class GroupTests : EntityFrameworkTestsBase
	{
		public override void Setup()
		{
			base.Setup();

			EntityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(1)));
			EntityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(1, 2)));
			EntityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(2)));
			EntityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(2, 3)));
			EntityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(3)));
		}

		[Test]
		public void GroupAdd()
		{
			Assert.That(EntityManager.Entities.Count, Is.EqualTo(5));

			var entity = EntityManager.CreateEntity();

			Assert.That(EntityManager.Entities.Count, Is.EqualTo(6));

			EntityManager.AddEntity(entity);

			Assert.That(EntityManager.Entities.Count, Is.EqualTo(6));
		}

		[Test]
		public void GroupRemove()
		{
			EntityManager.RemoveEntity(EntityManager.Entities.First());
			EntityManager.RemoveEntity(EntityManager.Entities.First());

			Assert.That(EntityManager.Entities.Count, Is.EqualTo(3));

			EntityManager.RemoveAllEntities();

			Assert.That(EntityManager.Entities.Count, Is.EqualTo(0));
		}

		[Test]
		public void GroupContains()
		{
			var entity = EntityManager.Entities.First();
			EntityManager.RemoveEntity(entity);

			Assert.That(!EntityManager.Entities.Contains(entity));
			Assert.AreNotEqual(EntityManager.Entities.First(), entity);
			Assert.That(EntityManager.Entities.Contains(EntityManager.Entities.Last()));
		}

		[Test]
		public void GroupForEach()
		{
			EntityManager.RemoveEntity(EntityManager.Entities.First());
			EntityManager.RemoveEntity(EntityManager.Entities.First());

			foreach (var entity in EntityManager.Entities)
				Assert.NotNull(entity);
		}

		[Test]
		public void GroupIndexOf()
		{
			var entity = EntityManager.Entities.Last();
			EntityManager.RemoveEntity(EntityManager.Entities.First());
			EntityManager.RemoveEntity(EntityManager.Entities.First());

			Assert.That(EntityManager.Entities.IndexOf(entity), Is.EqualTo(2));
		}

		[Test]
		public void GroupToArray()
		{
			EntityManager.RemoveEntity(EntityManager.Entities.First());
			EntityManager.RemoveEntity(EntityManager.Entities.Last());

			var entities = EntityManager.Entities.ToArray();

			Assert.That(entities.Length, Is.EqualTo(3));
			Assert.That(!entities.Contains(null));
			Assert.That(EntityManager.Entities.SequenceEqual(entities));
		}

		[Test]
		public void GroupMatchAll()
		{
			var entityGroup = EntityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1)), EntityMatches.All);

			Assert.That(entityGroup.Count, Is.EqualTo(2));
		}

		[Test]
		public void GroupMatchAny()
		{
			var entityGroup = EntityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.Any);
			Assert.That(entityGroup.Count, Is.EqualTo(4));
		}

		[Test]
		public void GroupMatchNone()
		{
			var entityGroup = EntityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.None);
			Assert.That(entityGroup.Count, Is.EqualTo(1));
		}

		[Test]
		public void GroupMatchExact()
		{
			var entityGroup = EntityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.Exact);
			Assert.That(entityGroup.Count, Is.EqualTo(1));
		}

		[Test]
		public void GroupChangeUpdate()
		{
			var entity = EntityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(1, 2, 3)));
			var entityGroup = EntityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.All);

			Assert.That(entityGroup.Count, Is.EqualTo(2));

			entity.Groups = EntityGroups.Nothing;

			Assert.That(entityGroup.Count, Is.EqualTo(1));
		}
	}
}
