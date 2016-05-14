using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Pooling.Tests;
using NUnit.Framework;
using NSubstitute;
using Pseudo.Pooling.Internal;

namespace Pseudo.Pooling.Tests
{
	public class PoolTests : PoolingTestsBase
	{
		[Test]
		public void CreateSingle()
		{
			var pool = new Pool<Dummy1>();
			var instance = pool.Create();

			Assert.IsNotNull(instance);
			Assert.That(instance, Is.AssignableFrom<Dummy1>());
		}

		[Test]
		public void CreateMultiple()
		{
			var pool = new Pool<Dummy1>();
			var instance1 = pool.Create();
			var instance2 = pool.Create();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.AssignableFrom<Dummy1>());
			Assert.That(instance2, Is.AssignableFrom<Dummy1>());
			Assert.That(instance1, !Is.EqualTo(instance2));
		}

		[Test]
		public void CreateWithFactory()
		{
			var factory = Substitute.For<IFactory<Dummy1>>();
			factory.Create().Returns(info => new Dummy1 { Value = 1 });
			var pool = new Pool<Dummy1>(factory);
			var instance = pool.Create();

			Assert.IsNotNull(instance);
			Assert.That(instance, Is.AssignableFrom<Dummy1>());
			Assert.That(instance.Value, Is.EqualTo(1));
			Received.InOrder(() => factory.Create());
		}

		[Test]
		public void CreateWithFactoryFunc()
		{
			var pool = new Pool<Dummy1>(() => new Dummy1 { Value = 1 });
			var instance = pool.Create();

			Assert.IsNotNull(instance);
			Assert.That(instance, Is.AssignableFrom<Dummy1>());
			Assert.That(instance.Value, Is.EqualTo(1));
		}

		[Test]
		public void CreateWithInitializer()
		{
			var initializer = new Dummy2Initializer();
			var pool = new Pool<Dummy2>(() => new Dummy2 { Value = 1 }, initializer);
			var instance = pool.Create();

			Assert.That(pool.Initializer, Is.EqualTo(initializer));
			Assert.IsNotNull(instance);
			Assert.That(instance, Is.AssignableFrom<Dummy2>());
			Assert.That(instance.Value, Is.EqualTo(2));

			pool.Recycle(instance);

			Assert.That(instance.Value, Is.EqualTo(3));
		}

		[Test]
		public void CreateWithInitializerAction()
		{
			var pool = new Pool<Dummy1>(() => new Dummy1 { Value = 1 }, d => d.Value = 2);
			var instance = pool.Create();

			Assert.IsNotNull(instance);
			Assert.That(instance, Is.AssignableFrom<Dummy1>());
			Assert.That(instance.Value, Is.EqualTo(2));
		}

		[Test]
		public void CreatePoolable()
		{
			var pool = new Pool<Dummy3>(() => new Dummy3 { Value = 1 });

			var instance1 = pool.Create();
			Assert.IsNotNull(instance1);
			Assert.That(instance1.Value, Is.EqualTo(2));
			pool.Recycle(instance1);
			Assert.That(instance1.Value, Is.EqualTo(3));

			var instance2 = pool.Create();
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance2.Value, Is.EqualTo(2));
			pool.Recycle(instance2);
			Assert.That(instance2.Value, Is.EqualTo(3));
		}

		[Test]
		public void CreateNull()
		{
			var pool = new Pool<Dummy1>(() => null);

			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			var instance1 = pool.Create();
			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			Assert.IsFalse(pool.Recycle(instance1));
			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			var instance2 = pool.Create();
			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			Assert.IsFalse(pool.Recycle(instance2));
			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			Assert.IsNull(instance1);
			Assert.IsNull(instance2);
		}

		[Test]
		public void RecycleSimple()
		{
			var pool = new Pool<Dummy1>();

			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			var instance1 = pool.Create();
			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			pool.Recycle(instance1);
			Assert.That(pool.Storage.Count, Is.EqualTo(1));
			var instance2 = pool.Create();
			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			pool.Recycle(instance2);
			Assert.That(pool.Storage.Count, Is.EqualTo(1));

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.AssignableFrom<Dummy1>());
			Assert.That(instance2, Is.AssignableFrom<Dummy1>());
			Assert.That(instance1, Is.EqualTo(instance2));
		}

		[Test]
		public void RecycleDuplicate()
		{
			var pool = new Pool<Dummy1>();
			var instance = pool.Create();

			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			Assert.IsTrue(pool.Recycle(instance));
			Assert.That(pool.Storage.Count, Is.EqualTo(1));
			Assert.IsFalse(pool.Recycle(instance));
			Assert.That(pool.Storage.Count, Is.EqualTo(1));
		}

		[Test]
		public void RecycleNull()
		{
			var pool = new Pool<Dummy1>();

			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			Assert.IsFalse(pool.Recycle(null));
			Assert.That(pool.Storage.Count, Is.EqualTo(0));
		}

		[Test]
		public void StorageFill()
		{
			var pool = new Pool<Dummy1>();

			pool.Storage.Fill(3);
			Assert.That(pool.Storage.Count, Is.EqualTo(3));
			pool.Create();
			Assert.That(pool.Storage.Count, Is.EqualTo(2));
			pool.Create();
			Assert.That(pool.Storage.Count, Is.EqualTo(1));
			pool.Create();
			Assert.That(pool.Storage.Count, Is.EqualTo(0));
		}

		[Test]
		public void StorageTrim()
		{
			var pool = new Pool<Dummy1>();

			pool.Storage.Fill(3);
			Assert.That(pool.Storage.Count, Is.EqualTo(3));
			pool.Storage.Trim(2);
			Assert.That(pool.Storage.Count, Is.EqualTo(2));
			pool.Storage.Capacity = 1;
			Assert.That(pool.Storage.Count, Is.EqualTo(1));
			pool.Create();
			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			pool.Create();
			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			pool.Create();
			Assert.That(pool.Storage.Count, Is.EqualTo(0));
		}

		[Test]
		public void StorageOverflow()
		{
			var pool = new Pool<Dummy1>();
			pool.Storage.Capacity = 1;

			var instance1 = pool.Create();
			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			var instance2 = pool.Create();
			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			pool.Recycle(instance1);
			Assert.That(pool.Storage.Count, Is.EqualTo(1));
			pool.Recycle(instance2);
			Assert.That(pool.Storage.Count, Is.EqualTo(1));
			var instance3 = pool.Create();
			Assert.That(pool.Storage.Count, Is.EqualTo(0));
			var instance4 = pool.Create();
			Assert.That(pool.Storage.Count, Is.EqualTo(0));

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.IsNotNull(instance3);
			Assert.IsNotNull(instance4);
			Assert.That(instance1, Is.AssignableFrom<Dummy1>());
			Assert.That(instance2, Is.AssignableFrom<Dummy1>());
			Assert.That(instance3, Is.AssignableFrom<Dummy1>());
			Assert.That(instance4, Is.AssignableFrom<Dummy1>());
			Assert.That(instance1, !Is.EqualTo(instance2));
			Assert.That(instance1, Is.EqualTo(instance3));
			Assert.That(instance1, !Is.EqualTo(instance4));
			Assert.That(instance2, !Is.EqualTo(instance3));
			Assert.That(instance2, !Is.EqualTo(instance4));
			Assert.That(instance3, !Is.EqualTo(instance4));
		}

		[Test]
		public void PoolDefaultModules()
		{
			var pool = new Pool<Dummy2>();

			Assert.That(pool.Factory, Is.AssignableFrom<DefaultFactory<Dummy2>>());
			Assert.That(pool.Initializer, Is.AssignableFrom<Dummy2Initializer>());
			Assert.That(pool.Storage, Is.AssignableFrom<Storage<Dummy2>>());
		}

		public class Dummy1
		{
			public int Value;
		}

		public class Dummy2
		{
			public int Value;
		}

		public class Dummy3 : IPoolable
		{
			public int Value;

			public void OnCreate()
			{
				Value = 2;
			}

			public void OnRecycle()
			{
				Value = 3;
			}
		}

		public class Dummy2Initializer : Initializer<Dummy2>
		{
			public override void OnCreate(Dummy2 instance)
			{
				instance.Value = 2;
			}

			public override void OnRecycle(Dummy2 instance)
			{
				instance.Value = 3;
			}
		}
	}
}
