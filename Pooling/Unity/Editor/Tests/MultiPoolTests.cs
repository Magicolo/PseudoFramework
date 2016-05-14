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
	public class MultiPoolTests : PoolingTestsBase
	{
		[Test]
		public void CreateWithInheritance()
		{
			var multiPool = new MultiPool<IDummy>();
			var instance1 = multiPool.Create<Dummy1>();
			var instance2 = multiPool.Create(typeof(Dummy2));

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.AssignableFrom<Dummy1>());
			Assert.That(instance2, Is.AssignableFrom<Dummy2>());
		}

		[Test]
		public void RecycleWithInheritance()
		{
			var multiPool = new MultiPool<IDummy>();
			var instance1 = multiPool.Create<Dummy1>();
			var instance2 = multiPool.Create(typeof(Dummy2));
			multiPool.Recycle(instance1);
			multiPool.Recycle(instance2);
			var instance3 = multiPool.Create(typeof(Dummy1));
			var instance4 = multiPool.Create<Dummy2>();
			multiPool.Recycle(instance3);
			multiPool.Recycle(instance4);

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.IsNotNull(instance3);
			Assert.IsNotNull(instance4);
			Assert.That(instance1, !Is.EqualTo(instance2));
			Assert.That(instance1, Is.EqualTo(instance3));
			Assert.That(instance1, !Is.EqualTo(instance4));
			Assert.That(instance2, !Is.EqualTo(instance3));
			Assert.That(instance2, Is.EqualTo(instance4));
			Assert.That(instance3, !Is.EqualTo(instance4));
			Assert.That(multiPool.GetPool<Dummy1>().Storage.Count, Is.EqualTo(1));
			Assert.That(multiPool.GetPool(typeof(Dummy2)).Storage.Count, Is.EqualTo(1));
		}

		[Test]
		public void CreateWithRegisteredPools()
		{
			var pool1 = new Pool<Dummy1>();
			var pool2 = new Pool<Dummy2>();
			var multiPool = new MultiPool<IDummy>(pool1);
			multiPool.AddPool(pool2);

			Assert.IsTrue(multiPool.ContainsPool(typeof(Dummy1)));
			Assert.IsTrue(multiPool.ContainsPool<Dummy2>());
			Assert.IsFalse(multiPool.ContainsPool<IDummy>());
		}

		[Test]
		public void CreateWithPoolFactory()
		{
			var factory = new DummyPoolFactory();
			var multiPool = new MultiPool<IDummy>(factory);
			var instance1 = multiPool.Create<Dummy1>();
			var instance2 = multiPool.Create<Dummy2>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1.Value, Is.EqualTo(1));
			Assert.That(instance2.Value, Is.EqualTo(2));
		}

		[Test]
		public void CreateWithPoolFactoryFunc()
		{
			var multiPool = new MultiPool<IDummy>(t =>
			{
				if (t.Is<Dummy1>())
					return new Pool<Dummy1>(() => new Dummy1 { Value = 1 });
				else if (t.Is<Dummy2>())
					return new Pool<Dummy2>(() => new Dummy2 { Value = 2 });
				else
					return new Pool<IDummy>(() => null);
			});
			var instance1 = multiPool.Create<Dummy1>();
			var instance2 = multiPool.Create<Dummy2>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1.Value, Is.EqualTo(1));
			Assert.That(instance2.Value, Is.EqualTo(2));
		}

		[Test]
		public void AddContainRemoveClearPool()
		{
			var pool1 = new Pool<Dummy1>();
			IPool pool2 = new Pool<Dummy2>();
			var multiPool = new MultiPool<Dummy1>(pool1);

			Assert.That(multiPool.PoolCount, Is.EqualTo(1));
			Assert.IsFalse(multiPool.AddPool(pool2));
			Assert.IsTrue(multiPool.ContainsPool<Dummy1>());
			Assert.IsFalse(multiPool.ContainsPool(typeof(Dummy2)));
			Assert.IsFalse(multiPool.ContainsPool(typeof(IDummy)));

			multiPool.RemovePool<Dummy1>();

			Assert.That(multiPool.PoolCount, Is.EqualTo(0));
			Assert.IsFalse(multiPool.ContainsPool(typeof(Dummy1)));
			Assert.IsTrue(multiPool.AddPool(pool1));
			Assert.That(multiPool.PoolCount, Is.EqualTo(1));
			Assert.IsTrue(multiPool.ContainsPool<Dummy1>());

			multiPool.ClearPools();

			Assert.That(multiPool.PoolCount, Is.EqualTo(0));
			Assert.IsFalse(multiPool.ContainsPool<Dummy1>());
		}

		public interface IDummy { }

		public class Dummy1 : IDummy
		{
			public int Value;
		}

		public class Dummy2 : IDummy
		{
			public int Value;
		}

		public class DummyPoolFactory : PoolFactoryBase
		{
			public override IPool Create(Type argument)
			{
				if (argument.Is<Dummy1>())
					return new Pool<Dummy1>(() => new Dummy1 { Value = 1 });
				else if (argument.Is<Dummy2>())
					return new Pool<Dummy2>(() => new Dummy2 { Value = 2 });
				else
					return new Pool<IDummy>(() => null);
			}
		}
	}
}
