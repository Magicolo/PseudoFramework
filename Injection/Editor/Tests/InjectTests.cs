using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Injection.Tests
{
	public class InjectTests : InjectionTestsBase
	{
		[Test]
		public void InjectionField()
		{
			Container.Binder.Bind<Dummy1>().ToSelf();
			Container.Binder.Bind<DummyField>().ToSelf();
			Container.Binder.Bind<DummySubField>().ToSelf();

			var instance = Container.Resolver.Resolve<Dummy1>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Field);
			Assert.IsNotNull(instance.Field.SubField);
		}

		[Test]
		public void InjectionProperty()
		{
			Container.Binder.Bind<Dummy1>().ToSelf();
			Container.Binder.Bind<DummyProperty>().ToSelf();
			Container.Binder.Bind<DummySubProperty>().ToSelf();

			var instance = Container.Resolver.Resolve<Dummy1>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Property);
			Assert.IsNotNull(instance.Property.SubProperty);
		}

		[Test]
		public void InjectionConstructor()
		{
			Container.Binder.Bind<Dummy2>().ToSelf();
			Container.Binder.Bind<DummyField>().ToSelf();
			Container.Binder.Bind<DummySubField>().ToSelf();
			Container.Binder.Bind<DummyProperty>().ToSelf();
			Container.Binder.Bind<DummySubProperty>().ToSelf();

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
			Container.Binder.Bind<Dummy3>().ToSelf();
			Container.Binder.Bind<DummyField>().ToSelf();
			Container.Binder.Bind<DummySubField>().ToSelf();
			Container.Binder.Bind<DummyProperty>().ToSelf();
			Container.Binder.Bind<DummySubProperty>().ToSelf();

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
			Container.Binder.Bind<Dummy4>().ToSelf();
			Container.Binder.Bind<DummyField>().ToSelf();
			Container.Binder.Bind<DummySubField>().ToSelf();
			Container.Binder.Bind<DummyProperty>().ToSelf();
			Container.Binder.Bind<DummySubProperty>().ToSelf();
			Container.Binder.Bind<Dummy1>().ToSelf().AsSingleton().WhenInto(typeof(Dummy2));
			Container.Binder.Bind<IDummy>().To<Dummy1>().AsSingleton().When(c => c.Type == ContextTypes.Field);
			Container.Binder.Bind<IDummy>().To<Dummy2>().AsSingleton().WhenHas("Boba");

			var instance = Container.Resolver.Resolve<Dummy4>();

			Assert.IsNotNull(instance);
			Assert.IsNotNull(instance.Dummy1);
			Assert.IsNotNull(instance.Dummy2);
			Assert.That(instance.Dummy1, Is.TypeOf<Dummy1>());
			Assert.That(instance.Dummy2, Is.TypeOf<Dummy2>());
		}
	}
}
