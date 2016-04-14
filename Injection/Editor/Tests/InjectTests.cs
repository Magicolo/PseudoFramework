using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Injection.Tests
{
	public class InjectTests : InjectionTestBase
	{
		[Test]
		public void InjectionField()
		{
			Container.Binder.Bind<Dummy1>().ToTransient();
			Container.Binder.Bind<DummyField>().ToTransient();
			Container.Binder.Bind<DummySubField>().ToTransient();

			var instance = Container.Resolver.Resolve<Dummy1>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Field);
			Assert.IsNotNull(instance.Field.SubField);
		}

		[Test]
		public void InjectionProperty()
		{
			Container.Binder.Bind<Dummy1>().ToTransient();
			Container.Binder.Bind<DummyProperty>().ToTransient();
			Container.Binder.Bind<DummySubProperty>().ToTransient();

			var instance = Container.Resolver.Resolve<Dummy1>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Property);
			Assert.IsNotNull(instance.Property.SubProperty);
		}

		[Test]
		public void InjectionConstructor()
		{
			Container.Binder.Bind<Dummy2>().ToTransient();
			Container.Binder.Bind<DummyField>().ToTransient();
			Container.Binder.Bind<DummySubField>().ToTransient();
			Container.Binder.Bind<DummyProperty>().ToTransient();
			Container.Binder.Bind<DummySubProperty>().ToTransient();

			var instance = Container.Resolver.Resolve<Dummy2>();

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
			Container.Binder.Bind<Dummy3>().ToTransient();
			Container.Binder.Bind<DummyField>().ToTransient();
			Container.Binder.Bind<DummySubField>().ToTransient();
			Container.Binder.Bind<DummyProperty>().ToTransient();
			Container.Binder.Bind<DummySubProperty>().ToTransient();

			var instance = Container.Resolver.Resolve<Dummy3>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Field);
			Assert.IsNotNull(instance.Field.SubField);
			Assert.IsNotNull(instance.Property);
			Assert.IsNotNull(instance.Property.SubProperty);
		}

		[Test]
		public void InjectionConditional()
		{
			Container.Binder.Bind<Dummy4>().ToTransient();
			Container.Binder.Bind<DummyField>().ToTransient();
			Container.Binder.Bind<DummySubField>().ToTransient();
			Container.Binder.Bind<DummyProperty>().ToTransient();
			Container.Binder.Bind<DummySubProperty>().ToTransient();
			Container.Binder.Bind<Dummy1>().ToSingleton().WhenInjectedInto(typeof(Dummy2));
			Container.Binder.Bind<IDummy>().ToSingleton<Dummy1>().When(c => c.ContextType == InjectionContext.ContextTypes.Field);
			Container.Binder.Bind<IDummy>().ToSingleton<Dummy2>().When("Boba");

			var instance = Container.Resolver.Resolve<Dummy4>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Dummy1);
			Assert.IsNotNull(instance.Dummy2);
			Assert.That(instance.Dummy1, Is.TypeOf<Dummy1>());
			Assert.That(instance.Dummy2, Is.TypeOf<Dummy2>());
		}
	}
}
