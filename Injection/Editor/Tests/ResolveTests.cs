using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Injection.Tests
{
	public class ResolveTests : InjectionTestBase
	{
		[Test]
		public void ResolveAll()
		{
			Container.Binder.Bind<IDummy>().To<Dummy1>().AsSingleton();
			Container.Binder.Bind<IDummy>().To<Dummy1>().AsSingleton();
			Container.Binder.Bind<IDummy>().To<Dummy1>().AsSingleton();
			Container.Binder.Bind<IDummy>().To<Dummy2>().AsSingleton();
			Container.Binder.Bind<IDummy>().To<Dummy3>().AsSingleton();
			Container.Binder.Bind<IDummy>().To<Dummy4>().AsSingleton();
			Container.Binder.Bind<DummyField>().ToSelf().AsSingleton();
			Container.Binder.Bind<DummyProperty>().ToSelf().AsSingleton();
			Container.Binder.Bind<DummySubField>().ToSelf().AsSingleton();
			Container.Binder.Bind<DummySubProperty>().ToSelf().AsSingleton();

			var dummies1 = Container.Resolver.ResolveAll<IDummy>();
			var dummies2 = Container.Resolver.ResolveAll<IDummy>();

			Assert.IsNotNull(dummies1);
			Assert.IsNotNull(dummies2);
			Assert.That(dummies1.Count(), Is.EqualTo(6));
			Assert.That(dummies2.Count(), Is.EqualTo(6));
			Assert.That(dummies1.SequenceEqual(dummies2));
		}
	}
}
