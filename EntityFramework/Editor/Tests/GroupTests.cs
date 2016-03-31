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
