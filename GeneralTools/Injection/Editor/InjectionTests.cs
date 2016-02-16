using UnityEngine;
using System;
using NUnit.Framework;
using NSubstitute;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Injection;

namespace Tests
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
			binder.Bind<IDummy>().ToSingle<Dummy1>();

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
		public void BindingToMethod()
		{
			binder.Bind<IDummy>().ToMethod((b, a) => new Dummy1());

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
			binder.BindAll<Dummy1>().ToSingle();

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

			var instance = binder.Resolver.Resolve<Dummy2>(new Dummy1());

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Field);
			Assert.IsNotNull(instance.Field.SubField);
			Assert.IsNotNull(instance.Property);
			Assert.IsNotNull(instance.Property.SubProperty);
			Assert.IsNotNull(instance.Dummy);
			Assert.IsNull(instance.Dummy.Field);
			Assert.IsNull(instance.Dummy.Property);
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
			binder.Bind<Dummy1>().ToSingle().When(c => c.DeclaringType == typeof(Dummy2));
			binder.Bind<IDummy>().ToSingle<Dummy1>().When(c => c.Type == InjectionContext.Types.Field);
			binder.Bind<IDummy>().ToSingle<Dummy2>().When(c => c.Identifier == "Boba");

			var instance = binder.Resolver.Resolve<Dummy4>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Dummy1);
			Assert.IsNotNull(instance.Dummy2);
			Assert.That(instance.Dummy1, Is.TypeOf<Dummy1>());
			Assert.That(instance.Dummy2, Is.TypeOf<Dummy2>());
		}

		public class Dummy1 : IDummy
		{
			[Inject(Optional = true)]
			public DummyField Field;
			[Inject(Optional = true)]
			public DummyProperty Property { get; set; }

		}
		public class Dummy2 : IDummy
		{
			public DummyField Field;
			public DummyProperty Property { get; set; }
			public Dummy1 Dummy;

			public Dummy2(DummyField field, DummyProperty property, Dummy1 dummy)
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
			[Inject(Identifier = "Boba")]
			public IDummy Dummy2 { get; set; }
		}
		public interface IDummy { }
		public class DummyField
		{
			[Inject(Optional = true)]
			public DummySubField SubField;
		}
		public class DummySubField { }
		public class DummyProperty
		{
			[Inject(Optional = true)]
			public DummySubProperty SubProperty { get; set; }
		}
		public class DummySubProperty { }
	}
}
