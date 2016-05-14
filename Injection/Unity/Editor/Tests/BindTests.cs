using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Injection.Tests
{
	public class BindTests : InjectionTestsBase
	{
		[Test]
		public void BindToSingle()
		{
			Container.Binder.Bind<IDummy>().To<Dummy1>().AsSingleton();

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
			Container.Binder.Bind<IDummy>().To<Dummy1>().AsTransient();

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
			Container.Binder.Bind<IDummy>().ToMethod(c => new Dummy1()).AsSingleton();

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
			Container.Binder.Bind<IDummy>().ToMethod(c => new Dummy1()).AsTransient();

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
			Container.Binder.BindAll<Dummy1>().ToSelf().AsSingleton();

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
			Container.Binder.Bind<Dummy5, IDummy>().To<Dummy5>().AsTransient();
			Container.Binder.Bind<int>().ToSelf().AsSingleton();
			Container.Binder.Bind<long>().ToInstance(100L);
			Container.Binder.Bind<IConvertible>().To<float>().AsTransient();
			Container.Binder.Bind<IComparable>().ToMethod(c => b++).AsTransient();

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
			Assert.That(Container.Binder.HasBinding<IDummyAttribute1>());

			var dummy1 = Container.Resolver.Resolve<DummyAttribute1>();
			var dummy2 = Container.Resolver.Resolve<DummyAttribute1>();
			var dummy3 = Container.Resolver.Resolve<IDummyAttribute1>();
			var dummy4 = Container.Resolver.Resolve<IDummyAttribute1>();

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

			var dummy1 = Container.Resolver.Resolve<DummyAttribute2>("Boba");
			var dummy2 = Container.Resolver.Resolve<DummyAttribute2>("Boba");
			var dummy3 = Container.Resolver.Resolve<DummyAttribute2>("Fett");
			var dummy4 = Container.Resolver.Resolve<DummyAttribute2>("Fett");

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
		public void BindAllWithAttributeCondition()
		{
			Assert.That(!Container.Binder.HasBinding<DummyAttribute1>());
			Container.Binder.Bind(GetType().Assembly);
			Assert.That(Container.Binder.HasBinding<DummyAttribute1>());
			Assert.That(Container.Binder.HasBinding<IDummyAttribute1>());

			var dummy1 = Container.Resolver.Resolve<DummyAttribute4>("Jango");
			var dummy2 = Container.Resolver.Resolve<IDummyAttribute2>("Jango");
			var dummy3 = Container.Resolver.Resolve<DummyAttribute5>("Boba");
			var dummy4 = Container.Resolver.Resolve<IDummyAttribute2>("Boba");
			var dummy5 = Container.Resolver.Resolve<DummyAttribute5>("Fett");
			var dummies = Container.Resolver.ResolveAll<object>("Boba").ToArray();

			Assert.IsNotNull(dummy1);
			Assert.IsNotNull(dummy2);
			Assert.IsNotNull(dummy3);
			Assert.IsNotNull(dummy4);
			Assert.IsNotNull(dummy5);
			Assert.That(dummy1, Is.EqualTo(dummy2));
			Assert.That(dummy1, !Is.EqualTo(dummy3));
			Assert.That(dummy1, !Is.EqualTo(dummy4));
			Assert.That(dummy1, !Is.EqualTo(dummy5));
			Assert.That(dummy2, !Is.EqualTo(dummy3));
			Assert.That(dummy2, !Is.EqualTo(dummy4));
			Assert.That(dummy2, !Is.EqualTo(dummy5));
			Assert.That(dummy3, Is.EqualTo(dummy4));
			Assert.That(dummy3, !Is.EqualTo(dummy5));
			Assert.That(dummy4, !Is.EqualTo(dummy5));
			Assert.That(dummies.Length, Is.EqualTo(1));
			Assert.That(dummies.SequenceEqual(new[] { dummy3 }));
		}

		[Bind(typeof(DummyAttribute1), BindScope.Transient)]
		[Bind(typeof(IDummyAttribute1), BindScope.Singleton)]
		public class DummyAttribute1 : IDummyAttribute1 { }

		[Bind(typeof(DummyAttribute2), BindScope.Singleton, ConditionSource.Identifier, ConditionComparer.Equals, "Boba")]
		[Bind(typeof(DummyAttribute2), BindScope.Singleton, ConditionSource.Identifier, ConditionComparer.Equals, "Fett")]
		public class DummyAttribute2 : IDummyAttribute1 { }

		public class DummyAttribute3 : IDummyAttribute2 { }
		[BindAll(BindScope.Singleton, ConditionSource.Identifier, ConditionComparer.Equals, "Jango")]
		public class DummyAttribute4 : IDummyAttribute2 { }
		[BindAll(BindScope.Singleton, ConditionSource.Identifier, ConditionComparer.Equals, "Boba")]
		[BindAll(BindScope.Transient, ConditionSource.Identifier, ConditionComparer.Equals, "Fett")]
		public class DummyAttribute5 : IDummyAttribute2 { }

		[BindFactory(typeof(DummyAttribute3), BindScope.Transient)]
		public class DummyFactory : InjectionFactoryBase<DummyAttribute3>
		{
			public int Calls;

			public override DummyAttribute3 Create(InjectionContext argument)
			{
				Calls++;

				return new DummyAttribute3();
			}
		}

		public interface IDummyAttribute1 { }
		public interface IDummyAttribute2 { }
	}
}
