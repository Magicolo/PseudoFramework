﻿using NUnit.Framework;
using Pseudo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
	public class EventTests
	{
		EventManager eventManager;

		[SetUp]
		public void EntityTestsSetup()
		{
			eventManager = new EventManager();
		}

		[TearDown]
		public void EntityTestsTearDown()
		{
			eventManager = null;
		}

		[Test]
		public void SubscribeUnsubscribe()
		{
			int triggerCount = 0;
			Action receiver = () => triggerCount++;
			Action<int> allReceiver = (int id) => triggerCount++;

			eventManager.Subscribe(EventsDummy.Zero, receiver);
			eventManager.SubscribeAll(allReceiver);
			eventManager.Unsubscribe(EventsDummy.Zero, receiver);
			eventManager.UnsubscribeAll(allReceiver);
			eventManager.Trigger(EventsDummy.Zero);
			eventManager.ResolveEvents();

			Assert.That(triggerCount == 0);
		}

		[Test]
		public void TriggerNoArgument()
		{
			int triggerCount = 0;
			eventManager.SubscribeAll((EventsDummy id) => triggerCount++);
			eventManager.Subscribe(EventsDummy.All, () => triggerCount++);
			eventManager.Subscribe(EventsDummy.Zero, () => triggerCount++);
			eventManager.Trigger(EventsDummy.Zero);
			eventManager.ResolveEvents();

			Assert.That(triggerCount == 3);
		}

		[Test]
		public void TriggerOneArgument()
		{
			int triggerCount = 0;
			eventManager.SubscribeAll((EventsDummy id) => triggerCount++);
			eventManager.Subscribe(EventsDummy.All, () => triggerCount++);
			eventManager.Subscribe(EventsDummy.One, (int arg) => triggerCount += arg);
			eventManager.Trigger(EventsDummy.One, 2);
			eventManager.ResolveEvents();

			Assert.That(triggerCount == 4);
		}

		[Test]
		public void TriggerTwoArguments()
		{
			int triggerCount = 0;
			eventManager.SubscribeAll((EventsDummy id) => triggerCount++);
			eventManager.Subscribe(EventsDummy.All, () => triggerCount++);
			eventManager.Subscribe(EventsDummy.Two, (int arg1, int arg2) => triggerCount += arg1 + arg2);
			eventManager.Trigger(EventsDummy.Two, 2, 3);
			eventManager.ResolveEvents();

			Assert.That(triggerCount == 7);
		}

		[Test]
		public void TriggerThreeArguments()
		{
			int triggerCount = 0;
			eventManager.SubscribeAll((EventsDummy id) => triggerCount++);
			eventManager.Subscribe(EventsDummy.All, () => triggerCount++);
			eventManager.Subscribe(EventsDummy.Three, (int arg1, int arg2, int arg3) => triggerCount += arg1 + arg2 + arg3);
			eventManager.Trigger(EventsDummy.Three, 2, 3, 4);
			eventManager.ResolveEvents();

			Assert.That(triggerCount == 11);
		}

		[Test]
		public void TriggerFourArguments()
		{
			int triggerCount = 0;
			eventManager.SubscribeAll((EventsDummy id) => triggerCount++);
			eventManager.Subscribe(EventsDummy.All, () => triggerCount++);
			eventManager.Subscribe(EventsDummy.Four, (int arg1, int arg2, int arg3, int arg4) => triggerCount += arg1 + arg2 + arg3 + arg4);
			eventManager.Trigger(EventsDummy.Four, 2, 3, 4, 5);
			eventManager.ResolveEvents();

			Assert.That(triggerCount == 16);
		}

		public class EventsDummy : PEnumFlag<EventsDummy>
		{
			public static readonly EventsDummy All = new EventsDummy(0, 1, 2, 3, 4);
			public static readonly EventsDummy Zero = new EventsDummy(0);
			public static readonly EventsDummy One = new EventsDummy(1);
			public static readonly EventsDummy Two = new EventsDummy(2);
			public static readonly EventsDummy Three = new EventsDummy(3);
			public static readonly EventsDummy Four = new EventsDummy(4);

			protected EventsDummy(params byte[] values) : base(values) { }

			public override bool Equals(EventsDummy other)
			{
				return HasAny(other);
			}
		}
	}
}
