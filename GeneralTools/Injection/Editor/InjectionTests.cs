using UnityEngine;
using System;
using NUnit.Framework;
using NSubstitute;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Injection;

namespace Pseudo.Tests
{
	public class InjectionTests
	{
		IBinder binder;

		[SetUp]
		public void Setup()
		{
			binder = new Binder();
		}

		[TearDown]
		public void TearDown()
		{
			binder = null;
		}

		[Test]
		public void BindingToSingle()
		{
			binder.Bind<IDummy>().ToSingleton<Dummy1>();

			var instance1 = binder.Resolver.Resolve<IDummy>();
			var instance2 = binder.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindingToTransient()
		{
			binder.Bind<IDummy>().ToTransient<Dummy1>();

			var instance1 = binder.Resolver.Resolve<IDummy>();
			var instance2 = binder.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, !Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindingToInstance()
		{
			binder.Bind<IDummy>().ToInstance(new Dummy1());

			var instance1 = binder.Resolver.Resolve<IDummy>();
			var instance2 = binder.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindingToSingleMethod()
		{
			binder.Bind<IDummy>().ToSingletonMethod(c => new Dummy1());

			var instance1 = binder.Resolver.Resolve<IDummy>();
			var instance2 = binder.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindingToTransientMethod()
		{
			binder.Bind<IDummy>().ToTransientMethod(c => new Dummy1());

			var instance1 = binder.Resolver.Resolve<IDummy>();
			var instance2 = binder.Resolver.Resolve<IDummy>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, !Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void BindingToFactory()
		{
			var factory = Substitute.For<IFactory>();
			factory.Create().Returns(new Dummy1());

			binder.Bind<IDummy>().ToFactory(factory);

			var instance1 = binder.Resolver.Resolve<IDummy>();
			var instance2 = binder.Resolver.Resolve<IDummy>();

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
			binder.BindAll<Dummy1>().ToSingleton();

			var instance1 = binder.Resolver.Resolve<IDummy>();
			var instance2 = binder.Resolver.Resolve<Dummy1>();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.That(instance1, Is.EqualTo(instance2));
			Assert.That(instance1, Is.TypeOf<Dummy1>());
			Assert.That(instance2, Is.TypeOf<Dummy1>());
		}

		[Test]
		public void InjectionField()
		{
			binder.Bind<Dummy1>().ToTransient();
			binder.Bind<DummyField>().ToTransient();
			binder.Bind<DummySubField>().ToTransient();

			var instance = binder.Resolver.Resolve<Dummy1>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Field);
			Assert.IsNotNull(instance.Field.SubField);
		}

		[Test]
		public void InjectionProperty()
		{
			binder.Bind<Dummy1>().ToTransient();
			binder.Bind<DummyProperty>().ToTransient();
			binder.Bind<DummySubProperty>().ToTransient();

			var instance = binder.Resolver.Resolve<Dummy1>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Property);
			Assert.IsNotNull(instance.Property.SubProperty);
		}

		[Test]
		public void InjectionConstructor()
		{
			binder.Bind<Dummy2>().ToTransient();
			binder.Bind<DummyField>().ToTransient();
			binder.Bind<DummySubField>().ToTransient();
			binder.Bind<DummyProperty>().ToTransient();
			binder.Bind<DummySubProperty>().ToTransient();

			var instance = binder.Resolver.Resolve<Dummy2>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Field);
			Assert.IsNotNull(instance.Field.SubField);
			Assert.IsNotNull(instance.Property);
			Assert.IsNotNull(instance.Property.SubProperty);
			Assert.IsNull(instance.Dummy);
		}

		[Test]
		public void InjectionMethod()
		{
			binder.Bind<Dummy3>().ToTransient();
			binder.Bind<DummyField>().ToTransient();
			binder.Bind<DummySubField>().ToTransient();
			binder.Bind<DummyProperty>().ToTransient();
			binder.Bind<DummySubProperty>().ToTransient();

			var instance = binder.Resolver.Resolve<Dummy3>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Field);
			Assert.IsNotNull(instance.Field.SubField);
			Assert.IsNotNull(instance.Property);
			Assert.IsNotNull(instance.Property.SubProperty);
		}

		[Test]
		public void InjectionConditional()
		{
			binder.Bind<Dummy4>().ToTransient();
			binder.Bind<DummyField>().ToTransient();
			binder.Bind<DummySubField>().ToTransient();
			binder.Bind<DummyProperty>().ToTransient();
			binder.Bind<DummySubProperty>().ToTransient();
			binder.Bind<Dummy1>().ToSingleton().WhenInjectedInto(typeof(Dummy2));
			binder.Bind<IDummy>().ToSingleton<Dummy1>().When(c => c.ContextType == InjectionContext.ContextTypes.Field);
			binder.Bind<IDummy>().ToSingleton<Dummy2>().When("Boba");

			var instance = binder.Resolver.Resolve<Dummy4>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Dummy1);
			Assert.IsNotNull(instance.Dummy2);
			Assert.That(instance.Dummy1, Is.TypeOf<Dummy1>());
			Assert.That(instance.Dummy2, Is.TypeOf<Dummy2>());
		}

		[Test]
		public void ResolveAll()
		{
			binder.Bind<IDummy>().ToSingleton<Dummy1>();
			binder.Bind<IDummy>().ToSingleton<Dummy1>();
			binder.Bind<IDummy>().ToSingleton<Dummy1>();
			binder.Bind<IDummy>().ToSingleton<Dummy2>();
			binder.Bind<IDummy>().ToSingleton<Dummy3>();
			binder.Bind<IDummy>().ToSingleton<Dummy4>();
			binder.Bind<DummyField>().ToSingleton();
			binder.Bind<DummyProperty>().ToSingleton();
			binder.Bind<DummySubField>().ToSingleton();
			binder.Bind<DummySubProperty>().ToSingleton();

			var dummies1 = binder.Resolver.ResolveAll<IDummy>();
			var dummies2 = binder.Resolver.ResolveAll<IDummy>();

			Assert.IsNotNull(dummies1);
			Assert.IsNotNull(dummies2);
			Assert.That(dummies1.Count(), Is.EqualTo(6));
			Assert.That(dummies2.Count(), Is.EqualTo(6));
			Assert.That(dummies1.SequenceEqual(dummies2));
		}

		public class Dummy1 : IDummy
		{
			[Inject(optional: true)]
			public DummyField Field;
			[Inject(optional: true)]
			public DummyProperty Property { get; set; }
		}
		public class Dummy2 : IDummy
		{
			public DummyField Field;
			public DummyProperty Property { get; set; }
			public Dummy1 Dummy;

			public Dummy2(DummyField field, DummyProperty property, [Inject(optional: true)] Dummy1 dummy)
			{
				Field = field;
				Property = property;
				Dummy = dummy;
			}
		}
		public class Dummy3 : IDummy
		{
			public DummyField Field;
			public DummyProperty Property { get; set; }

			[Inject]
			void InitializeField(DummyField field)
			{
				Field = field;
			}

			[Inject]
			void InitializeProperty(DummyProperty property)
			{
				Property = property;
			}
		}
		public class Dummy4 : IDummy
		{
			[Inject]
			public IDummy Dummy1;
			[Inject(optional: true, identifier: "Boba")]
			public IDummy Dummy2 { get; set; }
		}
		public interface IDummy { }
		public class DummyField
		{
			[Inject(optional: true)]
			public DummySubField SubField;
		}
		public class DummySubField { }
		public class DummyProperty
		{
			[Inject(optional: true)]
			public DummySubProperty SubProperty { get; set; }
		}
		public class DummySubProperty { }
	}
}
