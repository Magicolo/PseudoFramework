using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;
using System;
using Zenject;
using Pseudo;

namespace Tests
{
	public class SystemManagerTests
	{
		SystemManager systemManager;

		[SetUp]
		public void Setup()
		{
			systemManager = new SystemManager();
		}

		[TearDown]
		public void TearDown()
		{
			systemManager = null;
		}

		[Test]
		public void AddSystem()
		{
			var system = Substitute.For<ISystem>();

			systemManager.AddSystem(system);
			Assert.That(systemManager.Systems.Count == 1);
		}

		[Test]
		public void RemoveSystem()
		{
			var system = Substitute.For<ISystem>();

			systemManager.AddSystem(system);
			Assert.That(systemManager.Systems.Count == 1);
			systemManager.RemoveSystem(system);
			Assert.That(systemManager.Systems.Count == 0);
		}

		[Test]
		public void RemoveAllSystems()
		{
			var system = Substitute.For<ISystem>();

			systemManager.AddSystem(system);
			Assert.That(systemManager.Systems.Count == 1);
			systemManager.RemoveAllSystems();
			Assert.That(systemManager.Systems.Count == 0);
		}

		[Test]
		public void UpdateSystems()
		{
			var system = Substitute.For<ISystem, IUpdateable>();
			var updateable = (IUpdateable)system;
			updateable.Active = true;

			systemManager.AddSystem(system);
			((ITickable)systemManager).Tick();
			((ILateTickable)systemManager).LateTick();
			((IFixedTickable)systemManager).FixedTick();

			updateable.Received(1).Update();
		}

		[Test]
		public void FixedUpdateSystems()
		{
			var system = Substitute.For<ISystem, IFixedUpdateable>();
			var fixedUpdateable = (IFixedUpdateable)system;
			fixedUpdateable.Active = true;

			systemManager.AddSystem(system);
			((ITickable)systemManager).Tick();
			((ILateTickable)systemManager).LateTick();
			((IFixedTickable)systemManager).FixedTick();

			fixedUpdateable.Received(1).FixedUpdate();
		}

		[Test]
		public void LateUpdateSystems()
		{
			var system = Substitute.For<ISystem, ILateUpdateable>();
			var lateUpdateable = (ILateUpdateable)system;
			lateUpdateable.Active = true;

			systemManager.AddSystem(system);
			((ITickable)systemManager).Tick();
			((ILateTickable)systemManager).LateTick();
			((IFixedTickable)systemManager).FixedTick();

			lateUpdateable.Received(1).LateUpdate();
		}
	}
}