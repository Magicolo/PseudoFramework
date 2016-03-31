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
			Binder.Bind<Dummy1>().ToTransient();
			Binder.Bind<DummyField>().ToTransient();
			Binder.Bind<DummySubField>().ToTransient();

			var instance = Binder.Resolver.Resolve<Dummy1>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Field);
			Assert.IsNotNull(instance.Field.SubField);
		}

		[Test]
		public void InjectionProperty()
		{
			Binder.Bind<Dummy1>().ToTransient();
			Binder.Bind<DummyProperty>().ToTransient();
			Binder.Bind<DummySubProperty>().ToTransient();

			var instance = Binder.Resolver.Resolve<Dummy1>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Property);
			Assert.IsNotNull(instance.Property.SubProperty);
		}

		[Test]
		public void InjectionConstructor()
		{
			Binder.Bind<Dummy2>().ToTransient();
			Binder.Bind<DummyField>().ToTransient();
			Binder.Bind<DummySubField>().ToTransient();
			Binder.Bind<DummyProperty>().ToTransient();
			Binder.Bind<DummySubProperty>().ToTransient();

			var instance = Binder.Resolver.Resolve<Dummy2>();

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
			Binder.Bind<Dummy3>().ToTransient();
			Binder.Bind<DummyField>().ToTransient();
			Binder.Bind<DummySubField>().ToTransient();
			Binder.Bind<DummyProperty>().ToTransient();
			Binder.Bind<DummySubProperty>().ToTransient();

			var instance = Binder.Resolver.Resolve<Dummy3>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Field);
			Assert.IsNotNull(instance.Field.SubField);
			Assert.IsNotNull(instance.Property);
			Assert.IsNotNull(instance.Property.SubProperty);
		}

		[Test]
		public void InjectionConditional()
		{
			Binder.Bind<Dummy4>().ToTransient();
			Binder.Bind<DummyField>().ToTransient();
			Binder.Bind<DummySubField>().ToTransient();
			Binder.Bind<DummyProperty>().ToTransient();
			Binder.Bind<DummySubProperty>().ToTransient();
			Binder.Bind<Dummy1>().ToSingleton().WhenInjectedInto(typeof(Dummy2));
			Binder.Bind<IDummy>().ToSingleton<Dummy1>().When(c => c.ContextType == InjectionContext.ContextTypes.Field);
			Binder.Bind<IDummy>().ToSingleton<Dummy2>().When("Boba");

			var instance = Binder.Resolver.Resolve<Dummy4>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Dummy1);
			Assert.IsNotNull(instance.Dummy2);
			Assert.That(instance.Dummy1, Is.TypeOf<Dummy1>());
			Assert.That(instance.Dummy2, Is.TypeOf<Dummy2>());
		}
	}
}
