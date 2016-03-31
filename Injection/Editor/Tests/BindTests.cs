using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Injection.Tests
{
	public class BindTests : InjectionTestBase
	{
		[Test]
		public void BindToSingle()
		{
			Binder.Bind<IDummy>().ToSingleton<Dummy1>();

			var instance1 = Binder.Resolver.Resolve<IDummy>();
			var instance2 = Binder.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindToTransient()
		{
			Binder.Bind<IDummy>().ToTransient<Dummy1>();

			var instance1 = Binder.Resolver.Resolve<IDummy>();
			var instance2 = Binder.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, !Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindToInstance()
		{
			Binder.Bind<IDummy>().ToInstance(new Dummy1());

			var instance1 = Binder.Resolver.Resolve<IDummy>();
			var instance2 = Binder.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindToSingleMethod()
		{
			Binder.Bind<IDummy>().ToSingletonMethod(c => new Dummy1());

			var instance1 = Binder.Resolver.Resolve<IDummy>();
			var instance2 = Binder.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindToTransientMethod()
		{
			Binder.Bind<IDummy>().ToTransientMethod(c => new Dummy1());

			var instance1 = Binder.Resolver.Resolve<IDummy>();
			var instance2 = Binder.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, !Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindToFactory()
		{
			var factory = Substitute.For<IFactory>();
			factory.Create().Returns(new Dummy1());

			Binder.Bind<IDummy>().ToFactory(factory);

			var instance1 = Binder.Resolver.Resolve<IDummy>();
			var instance2 = Binder.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
			factory.Received(2).Create();
		}

		[Test]
		public void BindAllToSingle()
		{
			Binder.BindAll<Dummy1>().ToSingleton();

			var instance1 = Binder.Resolver.Resolve<IDummy>();
			var instance2 = Binder.Resolver.Resolve<Dummy1>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindToStruct()
		{
			byte b = 1;
			Binder.Bind<Dummy5, IDummy>().ToTransient<Dummy5>();
			Binder.Bind<int>().ToSingleton();
			Binder.Bind<long>().ToInstance(100L);
			Binder.Bind<IConvertible>().ToTransient<float>();
			Binder.Bind<IComparable>().ToTransientMethod(c => b++);

			var instance = Binder.Resolver.Resolve<Dummy5>();

			Assert.That(instance.Int, Is.EqualTo(0));
			Assert.That(instance.Long, Is.EqualTo(100L));
			Assert.That(instance.Float, Is.EqualTo(0f));
			Assert.That(instance.Byte1, Is.EqualTo(1));
			Assert.That(instance.Byte2, Is.EqualTo(2));
			Assert.That(instance.Byte3, Is.EqualTo(3));
		}
	}
}
