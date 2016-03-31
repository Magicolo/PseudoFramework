using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using NUnit.Framework;

namespace Pseudo.EntityFramework.Tests
{
	public class HierarchyTests : EntityFrameworkTestsBase
	{
		[Test]
		public void HierarchyAddChild()
		{
			var entity1 = EntityManager.CreateEntity();
			var entity2 = EntityManager.CreateEntity();
			var entity3 = EntityManager.CreateEntity();

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
			var entity1 = EntityManager.CreateEntity();
			var entity2 = EntityManager.CreateEntity();
			var entity3 = EntityManager.CreateEntity();

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
	}
}
