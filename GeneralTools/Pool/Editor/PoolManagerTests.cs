using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using NUnit.Framework;
using Pseudo.Internal.Pool;
using NSubstitute;

namespace Tests
{
	public class PoolManagerTests
	{
		[SetUp]
		public void Setup()
		{

		}

		[TearDown]
		public void TearDown()
		{

		}

		[Test]
		public void InitializeValue()
		{
			var reference = new DummyPoolable { Value1 = 1f, Value2 = "Boba" };
			var instance = new DummyPoolable();
			var initializer = PoolUtility.GetPoolInitializer(reference);

			initializer.InitializeFields(instance);

			Assert.That(instance.Value1, Is.EqualTo(reference.Value1));
			Assert.That(instance.Value2, Is.EqualTo(reference.Value2));
		}

		[Test]
		public void IgnoreValues()
		{
			var reference = new DummyPoolable { Value3 = true, Value4 = ScriptableObject.CreateInstance<DummyScriptable>() };
			var instance = new DummyPoolable();
			var initializer = PoolUtility.GetPoolInitializer(reference);

			initializer.InitializeFields(instance);

			Assert.That(instance.Value3, Is.False);
			Assert.That(instance.Value4, Is.Null);
		}

		[Test]
		public void InitializeContent()
		{
			var reference = new DummyPoolable { Value5 = new DummyContent { Value1 = 1f, Value2 = "Boba" } };
			var instance = new DummyPoolable();
			var initializer = PoolUtility.GetPoolInitializer(reference);

			initializer.InitializeFields(instance);

			Assert.That(instance.Value5.Equals(reference.Value5));
		}

		[Test]
		public void InitializeArrayOrList()
		{
			var reference = new DummyPoolable { Value6 = new[] { 0, 1, 2 }, Value7 = new List<char> { 'a', 'b', 'c' } };
			var instance = new DummyPoolable();
			var initializer = PoolUtility.GetPoolInitializer(reference);

			initializer.InitializeFields(instance);

			Assert.That(instance.Value6.ContentEquals(reference.Value6));
			Assert.That(instance.Value7.ContentEquals(reference.Value7));
		}

		[Test]
		public void InitializeArrayOrListContent()
		{
			var reference = new DummyPoolable
			{
				Value8 = new[] { new DummyContent { Value1 = 1f, Value2 = "Boba" } },
				Value9 = new List<DummyContent> { new DummyContent { Value1 = 2f, Value2 = "Fett" } }
			};

			var instance = new DummyPoolable();
			var initializer = PoolUtility.GetPoolInitializer(reference);

			initializer.InitializeFields(instance);

			Assert.That(instance.Value8.ContentEquals(reference.Value8));
			Assert.That(instance.Value9.ContentEquals(reference.Value9));
		}

		[Test, ExpectedException(typeof(InitializationCycleException))]
		public void InitializationCycle()
		{
			var reference = new DummyPoolable();
			reference.Value10 = reference;

			PoolUtility.GetPoolInitializer(reference);
		}

		[Test]
		public void PoolableCalls()
		{
			var reference = Substitute.For<DummyPoolable>();
			var initializer = PoolUtility.GetPoolInitializer(reference);
			initializer.InitializeFields(reference);

			Received.InOrder(() =>
			{
				reference.OnPrePoolSettersInitialize();
				reference.OnPostPoolSettersInitialize(null);
				reference.OnPrePoolInitialize();
				reference.OnPostPoolInitialize();
			});
		}

		[Serializable]
		public class DummyPoolable : IPoolSettersInitializable, IPoolInitializable
		{
			public float Value1;
			public string Value2;
			[DoNotInitialize]
			public bool Value3;
			public ScriptableObject Value4;
			[InitializeContent]
			public DummyContent Value5;
			public int[] Value6;
			public List<char> Value7;
			[InitializeContent]
			public DummyContent[] Value8;
			[InitializeContent]
			public List<DummyContent> Value9;
			[InitializeContent]
			public object Value10;

			public void OnPrePoolInitialize() { }

			public void OnPostPoolInitialize() { }

			public void OnPrePoolSettersInitialize() { }

			public void OnPostPoolSettersInitialize(List<IPoolSetter> setters) { }
		}

		public class DummyScriptable : ScriptableObject { }

		public class DummyContent : IEquatable<DummyContent>
		{
			public float Value1;
			public string Value2;

			public bool Equals(DummyContent other)
			{
				return Value1 == other.Value1 && Value2 == other.Value2;
			}
		}
	}
}
