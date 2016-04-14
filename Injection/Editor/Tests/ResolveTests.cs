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
			Container.Binder.Bind<IDummy>().ToSingleton<Dummy1>();
			Container.Binder.Bind<IDummy>().ToSingleton<Dummy1>();
			Container.Binder.Bind<IDummy>().ToSingleton<Dummy1>();
			Container.Binder.Bind<IDummy>().ToSingleton<Dummy2>();
			Container.Binder.Bind<IDummy>().ToSingleton<Dummy3>();
			Container.Binder.Bind<IDummy>().ToSingleton<Dummy4>();
			Container.Binder.Bind<DummyField>().ToSingleton();
			Container.Binder.Bind<DummyProperty>().ToSingleton();
			Container.Binder.Bind<DummySubField>().ToSingleton();
			Container.Binder.Bind<DummySubProperty>().ToSingleton();

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
