using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using NUnit.Framework;
using Pseudo.Internal;

namespace Pseudo.Pooling.Tests
{
	public class ClonePoolTests : PoolingTestsBase
	{
		[Test]
		public void Create()
		{
			var reference = new Dummy1 { Value = 1, Reference = "Boba" };
			var pool = new ClonePool<Dummy1>(reference);
			var instance1 = pool.Create();

			Assert.IsNotNull(instance1);
			Assert.That(instance1, !Is.EqualTo(reference));
			Assert.That(instance1.Value, Is.EqualTo(reference.Value));
			Assert.That(instance1.Reference, Is.EqualTo(reference.Reference));

			instance1.Value = 2;
			instance1.Reference = "Fett";
			pool.Recycle(instance1);
			var instance2 = pool.Create();

			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance2, !Is.EqualTo(reference));
			Assert.That(instance2.Value, !Is.EqualTo(reference.Value));
			Assert.That(instance2.Reference, !Is.EqualTo(reference.Reference));
		}

		[Test]
		public void CreateWithCloner()
		{
			var reference = new Dummy2 { Value = 1, Reference = "Boba" };
			var pool = new ClonePool<Dummy2>(reference, new Dummy2Cloner());
			var instance1 = pool.Create();

			Assert.IsNotNull(instance1);
			Assert.That(instance1, Is.EqualTo(reference));
		}

		[Test]
		public void CreateWithClonerFunc()
		{
			var reference = new Dummy2 { Value = 1, Reference = "Boba" };
			var pool = new ClonePool<Dummy2>(reference, r => r, (s, t) => { });
			var instance1 = pool.Create();

			Assert.IsNotNull(instance1);
			Assert.That(instance1, Is.EqualTo(reference));
		}

		[Test]
		public void CreateWithCopier()
		{
			var reference = new Dummy2 { Value = 1, Reference = "Boba" };
			var pool = new ClonePool<Dummy2>(reference, r => new Dummy2(), new Dummy2Copier());
			var instance1 = pool.Create();

			Assert.IsNotNull(instance1);
			Assert.That(instance1, !Is.EqualTo(reference));
			Assert.That(instance1.Value, Is.EqualTo(reference.Value + 1));
			Assert.That(instance1.Reference, Is.EqualTo(reference.Reference + "Fett"));
		}

		[Test]
		public void CreateWithCopierAction()
		{
			var reference = new Dummy2 { Value = 1, Reference = "Boba" };
			var pool = new ClonePool<Dummy2>(reference, r => new Dummy2(), (s, t) => t.Value = s.Value + 2);
			var instance1 = pool.Create();

			Assert.IsNotNull(instance1);
			Assert.That(instance1, !Is.EqualTo(reference));
			Assert.That(instance1.Value, Is.EqualTo(reference.Value + 2));
			Assert.That(instance1.Reference, !Is.EqualTo(reference.Reference));
		}

		[Test]
		public void PoolDefaultModules()
		{
			var pool1 = new ClonePool<Dummy2>(new Dummy2());
			var pool2 = new ClonePool<Dummy3>(new Dummy3());

			PDebug.Log(Cloner<Dummy2>.Default);
			PDebug.Log(Copier<Dummy2>.Default);
			Assert.That(pool1.Cloner, Is.AssignableFrom<Dummy2Cloner>());
			Assert.That(pool1.Copier, Is.AssignableFrom<Dummy2Copier>());
			Assert.That(pool2.Cloner, Is.AssignableFrom<DefaultCloner<Dummy3>>());
			Assert.That(pool2.Copier, Is.AssignableFrom<DefaultCopier<Dummy3>>());
		}

		public class Dummy1
		{
			public int Value;
			public string Reference;
		}

		public class Dummy2
		{
			public int Value;
			public string Reference;
		}

		public class Dummy3 : ICloneable<Dummy3>, ICopyable<Dummy3>
		{
			public int Value;
			public string Reference;

			public Dummy3 Clone()
			{
				var instance = new Dummy3();
				CopyTo(instance);

				return instance;
			}

			public void Copy(Dummy3 source)
			{
				source.CopyTo(this);
			}

			public void CopyTo(Dummy3 target)
			{
				target.Value = Value;
				target.Reference = Reference;
			}
		}

		public class Dummy2Cloner : Cloner<Dummy2>
		{
			public override Dummy2 Clone(Dummy2 source)
			{
				return source;
			}
		}

		public class Dummy2Copier : Copier<Dummy2>
		{
			public override void CopyTo(Dummy2 source, Dummy2 target)
			{
				target.Value = source.Value + 1;
				target.Reference = source.Reference + "Fett";
			}
		}
	}
}
