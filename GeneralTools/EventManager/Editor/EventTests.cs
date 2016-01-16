using NUnit.Framework;
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
			eventManager.Subscribe(allReceiver);
			eventManager.Unsubscribe(EventsDummy.Zero, receiver);
			eventManager.Unsubscribe(allReceiver);
			eventManager.Trigger(EventsDummy.Zero);

			Assert.That(triggerCount == 0);
		}

		[Test]
		public void TriggerNoArgument()
		{
			int triggerCount = 0;
			eventManager.Subscribe(EventsDummy.Zero, () => triggerCount++);
			eventManager.Subscribe((EventsDummy id) => triggerCount++);
			eventManager.Trigger(EventsDummy.Zero);

			Assert.That(triggerCount == 2);
		}

		[Test]
		public void TriggerOneArgument()
		{
			int triggerCount = 0;
			eventManager.Subscribe(EventsDummy.One, (int arg) => triggerCount += arg);
			eventManager.Subscribe((EventsDummy id) => triggerCount++);
			eventManager.Trigger(EventsDummy.One, 2);

			Assert.That(triggerCount == 3);
		}

		[Test]
		public void TriggerTwoArguments()
		{
			int triggerCount = 0;
			eventManager.Subscribe(EventsDummy.Two, (int arg1, int arg2) => triggerCount += arg1 + arg2);
			eventManager.Subscribe((EventsDummy id) => triggerCount++);
			eventManager.Trigger(EventsDummy.Two, 2, 3);

			Assert.That(triggerCount == 6);
		}

		[Test]
		public void TriggerThreeArguments()
		{
			int triggerCount = 0;
			eventManager.Subscribe(EventsDummy.Three, (int arg1, int arg2, int arg3) => triggerCount += arg1 + arg2 + arg3);
			eventManager.Subscribe((EventsDummy id) => triggerCount++);
			eventManager.Trigger(EventsDummy.Three, 2, 3, 4);

			Assert.That(triggerCount == 10);
		}

		[Test]
		public void TriggerFourArguments()
		{
			int triggerCount = 0;
			eventManager.Subscribe(EventsDummy.Four, (int arg1, int arg2, int arg3, int arg4) => triggerCount += arg1 + arg2 + arg3 + arg4);
			eventManager.Subscribe((EventsDummy id) => triggerCount++);
			eventManager.Trigger(EventsDummy.Four, 2, 3, 4, 5);

			Assert.That(triggerCount == 15);
		}

		public class EventsDummy : PEnum<EventsDummy, int>
		{
			public static readonly EventsDummy Zero = new EventsDummy(0);
			public static readonly EventsDummy One = new EventsDummy(1);
			public static readonly EventsDummy Two = new EventsDummy(2);
			public static readonly EventsDummy Three = new EventsDummy(3);
			public static readonly EventsDummy Four = new EventsDummy(4);

			protected EventsDummy(int value) : base(value) { }
		}
	}
}
