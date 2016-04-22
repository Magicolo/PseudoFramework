using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using NUnit.Framework;

namespace Pseudo.Injection.Tests
{
	public class InstantiateTests : InjectionTestsBase
	{
		[Test]
		public void Instantiate()
		{
			var dummy1 = Container.Instantiator.Instantiate<DummySubField>();
			var dummy2 = Container.Instantiator.Instantiate<DummySubProperty>();

			Assert.IsNotNull(dummy1);
			Assert.IsNotNull(dummy2);
		}

		[Test]
		public void InstantiateWithDefaultValues()
		{
			var dummy1 = Container.Get<InstantiateDummyWithDefaultValues>();

			Assert.IsNotNull(dummy1);
			Assert.That(dummy1.Integer, Is.EqualTo(1));
			Assert.That(dummy1.Single, Is.EqualTo(2f));
			Assert.That(dummy1.Reference, Is.EqualTo(null));
		}

		//[Test]
		//public void InstantiateWithArguments()
		//{
		//	var dummy1 = Container.Instantiator.Instantiate<InstantiateDummy>((short)1, 2, true);
		//	var dummy2 = Container.Instantiator.Instantiate<InstantiateDummy>((byte)3, null, null);

		//	Assert.IsNotNull(dummy1);
		//	Assert.IsNotNull(dummy2);
		//	Assert.That(dummy1, !Is.EqualTo(dummy2));
		//	Assert.That(dummy1.Integer, Is.EqualTo(1));
		//	Assert.That(dummy1.Single, Is.EqualTo(2));
		//	Assert.That(dummy1.Boolean, Is.EqualTo(true));
		//	Assert.That(dummy2.Integer, Is.EqualTo(3));
		//	Assert.That(dummy2.Single, Is.EqualTo(0f));
		//	Assert.That(dummy2.Boolean, Is.EqualTo(false));
		//}

		public class InstantiateDummy
		{
			public readonly int Integer;
			public readonly float Single;
			public readonly bool Boolean;

			public InstantiateDummy() { }

			public InstantiateDummy(int integer, float single, bool boolean)
			{
				Integer = integer;
				Single = single;
				Boolean = boolean;
			}
		}

		public class InstantiateDummyWithDefaultValues
		{
			public readonly int Integer;
			public readonly float Single;
			public readonly object Reference;

			public InstantiateDummyWithDefaultValues(int integer = 1, float single = 2f, object reference = null)
			{
				Integer = integer;
				Single = single;
				Reference = reference;
			}
		}
	}
}
