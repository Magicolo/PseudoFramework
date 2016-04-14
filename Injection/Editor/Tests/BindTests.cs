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
			Container.Binder.Bind<IDummy>().ToSingleton<Dummy1>();

			var instance1 = Container.Resolver.Resolve<IDummy>();
			var instance2 = Container.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindToTransient()
		{
			Container.Binder.Bind<IDummy>().ToTransient<Dummy1>();

			var instance1 = Container.Resolver.Resolve<IDummy>();
			var instance2 = Container.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, !Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindToInstance()
		{
			Container.Binder.Bind<IDummy>().ToInstance(new Dummy1());

			var instance1 = Container.Resolver.Resolve<IDummy>();
			var instance2 = Container.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindToSingleMethod()
		{
			Container.Binder.Bind<IDummy>().ToSingletonMethod(c => new Dummy1());

			var instance1 = Container.Resolver.Resolve<IDummy>();
			var instance2 = Container.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindToTransientMethod()
		{
			Container.Binder.Bind<IDummy>().ToTransientMethod(c => new Dummy1());

			var instance1 = Container.Resolver.Resolve<IDummy>();
			var instance2 = Container.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, !Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindToFactory()
		{
			var factory = Substitute.For<IInjectionFactory>();
			factory.Create(default(InjectionContext)).ReturnsForAnyArgs(new Dummy1());

			Container.Binder.Bind<IDummy>().ToFactory(factory);

			var instance1 = Container.Resolver.Resolve<IDummy>();
			var instance2 = Container.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
			factory.ReceivedWithAnyArgs(2).Create(default(InjectionContext));
		}

		[Test]
		public void BindAllToSingle()
		{
			Container.Binder.BindAll<Dummy1>().ToSingleton();

			var instance1 = Container.Resolver.Resolve<IDummy>();
			var instance2 = Container.Resolver.Resolve<Dummy1>();

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
			Container.Binder.Bind<Dummy5, IDummy>().ToTransient<Dummy5>();
			Container.Binder.Bind<int>().ToSingleton();
			Container.Binder.Bind<long>().ToInstance(100L);
			Container.Binder.Bind<IConvertible>().ToTransient<float>();
			Container.Binder.Bind<IComparable>().ToTransientMethod(c => b++);

			var instance = Container.Resolver.Resolve<Dummy5>();

			Assert.That(instance.Int, Is.EqualTo(0));
			Assert.That(instance.Long, Is.EqualTo(100L));
			Assert.That(instance.Float, Is.EqualTo(0f));
			Assert.That(instance.Byte1, Is.EqualTo(1));
			Assert.That(instance.Byte2, Is.EqualTo(2));
			Assert.That(instance.Byte3, Is.EqualTo(3));
		}

		[Test]
		public void BindWithAttribute()
		{
			Assert.That(!Container.Binder.HasBinding<DummyAttribute1>());
			Container.Binder.Bind(GetType().Assembly);
			Assert.That(Container.Binder.HasBinding<DummyAttribute1>());
			Assert.That(Container.Binder.HasBinding<IDummyAttribute>());

			var dummy1 = Container.Resolver.Resolve<DummyAttribute1>();
			var dummy2 = Container.Resolver.Resolve<DummyAttribute1>();
			var dummy3 = Container.Resolver.Resolve<IDummyAttribute>();
			var dummy4 = Container.Resolver.Resolve<IDummyAttribute>();

			Assert.IsNotNull(dummy1);
			Assert.IsNotNull(dummy2);
			Assert.IsNotNull(dummy3);
			Assert.IsNotNull(dummy4);
			Assert.That(dummy1, !Is.EqualTo(dummy2));
			Assert.That(dummy1, !Is.EqualTo(dummy3));
			Assert.That(dummy1, !Is.EqualTo(dummy4));
			Assert.That(dummy2, !Is.EqualTo(dummy3));
			Assert.That(dummy2, !Is.EqualTo(dummy4));
			Assert.That(dummy3, Is.EqualTo(dummy4));
		}

		[Test]
		public void BindWithAttributeConditions()
		{
			Assert.That(!Container.Binder.HasBinding<DummyAttribute2>());
			Container.Binder.Bind(GetType().Assembly);
			Assert.That(Container.Binder.HasBinding<DummyAttribute2>());

			var context1 = new InjectionContext
			{
				Container = Container,
				ContractType = typeof(DummyAttribute2),
				Identifier = "Boba"
			};
			var context2 = new InjectionContext
			{
				Container = Container,
				ContractType = typeof(DummyAttribute2),
				Identifier = "Fett"
			};

			var dummy1 = Container.Resolver.Resolve(context1);
			var dummy2 = Container.Resolver.Resolve(context1);
			var dummy3 = Container.Resolver.Resolve(context2);
			var dummy4 = Container.Resolver.Resolve(context2);

			Assert.IsNotNull(dummy1);
			Assert.IsNotNull(dummy2);
			Assert.IsNotNull(dummy3);
			Assert.IsNotNull(dummy4);
			Assert.That(dummy1, Is.EqualTo(dummy2));
			Assert.That(dummy1, !Is.EqualTo(dummy3));
			Assert.That(dummy1, !Is.EqualTo(dummy4));
			Assert.That(dummy2, !Is.EqualTo(dummy3));
			Assert.That(dummy2, !Is.EqualTo(dummy4));
			Assert.That(dummy3, Is.EqualTo(dummy4));
		}

		[Test]
		public void BindFactoryWithAttribute()
		{
			Assert.That(!Container.Binder.HasBinding<DummyAttribute3>());
			Container.Binder.Bind(GetType().Assembly);
			Assert.That(Container.Binder.HasBinding<DummyAttribute3>());

			var factory = Container.Resolver.Resolve<DummyFactory>();
			var dummy1 = Container.Resolver.Resolve<DummyAttribute3>();
			var dummy2 = Container.Resolver.Resolve<DummyAttribute3>();
			var dummy3 = Container.Resolver.Resolve<DummyAttribute3>();

			Assert.IsNotNull(factory);
			Assert.IsNotNull(dummy1);
			Assert.IsNotNull(dummy2);
			Assert.IsNotNull(dummy3);
			Assert.That(dummy1, !Is.EqualTo(dummy2));
			Assert.That(dummy1, !Is.EqualTo(dummy3));
			Assert.That(dummy2, !Is.EqualTo(dummy3));
			Assert.That(factory.Calls, Is.EqualTo(3));
		}
	}
}
