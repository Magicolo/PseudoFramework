using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.EntityFramework.Tests
{
	public class GroupMessagesTests : EntityFrameworkTestsBase
	{
		[Test]
		public void GroupSendMessage()
		{
			var entity = EntityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(1, 2, 3)));
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var entityGroup = EntityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.All);

			entity.Add(component1);
			entity.Add(component2);
			entityGroup.BroadcastMessage(0);

			component1.Received(1).MessageNoArgument();
			component1.Received(1).OnMessage(0);
			component2.Received(1).MessageNoArgument();
			component2.Received(1).OnMessage(0);
		}

		[Test]
		public void GroupSendMessageInactive()
		{
			var entity = EntityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(1, 2, 3)));
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var entityGroup = EntityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.All);

			entity.Add(component1);
			entity.Add(component2);
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
			var entity = EntityManager.CreateEntity(EntityGroups.GetValue(new ByteFlag(1, 2, 3)));
			var component1 = Substitute.For<DummyComponent1>();
			var component2 = Substitute.For<DummyComponent2>();
			var entityGroup = EntityManager.Entities.Filter(EntityGroups.GetValue(new ByteFlag(1, 2)), EntityMatches.All);

			entity.Add(component1);
			entity.Add(component2);
			component2.Active = false;
			entityGroup.BroadcastMessage(0);

			component1.Received(1).MessageNoArgument();
			component1.Received(1).OnMessage(0);
			component2.Received(0).MessageNoArgument();
			component2.Received(0).OnMessage(0);
		}
	}
}
