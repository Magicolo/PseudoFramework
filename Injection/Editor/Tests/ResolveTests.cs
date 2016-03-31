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
			Binder.Bind<IDummy>().ToSingleton<Dummy1>();
			Binder.Bind<IDummy>().ToSingleton<Dummy1>();
			Binder.Bind<IDummy>().ToSingleton<Dummy1>();
			Binder.Bind<IDummy>().ToSingleton<Dummy2>();
			Binder.Bind<IDummy>().ToSingleton<Dummy3>();
			Binder.Bind<IDummy>().ToSingleton<Dummy4>();
			Binder.Bind<DummyField>().ToSingleton();
			Binder.Bind<DummyProperty>().ToSingleton();
			Binder.Bind<DummySubField>().ToSingleton();
			Binder.Bind<DummySubProperty>().ToSingleton();

			var dummies1 = Binder.Resolver.ResolveAll<IDummy>();
			var dummies2 = Binder.Resolver.ResolveAll<IDummy>();

			Assert.IsNotNull(dummies1);
			Assert.IsNotNull(dummies2);
			Assert.That(dummies1.Count(), Is.EqualTo(6));
			Assert.That(dummies2.Count(), Is.EqualTo(6));
			Assert.That(dummies1.SequenceEqual(dummies2));
		}
	}
}
