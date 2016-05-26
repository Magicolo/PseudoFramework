using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using NUnit.Framework;

namespace Pseudo.Initialization.Tests
{
	public class InitializeTests : InitializationTestsBase
	{
		[Test]
		public void InitializeInt()
		{
			int instance = 1;
			int reference = 2;

			Initializer<int>.Default.Initialize(ref instance, reference);

			Assert.That(instance, Is.EqualTo(reference));
		}

		[Test]
		public void InitializeString()
		{
			var instance = "Boba";
			var reference = "Fett";

			Initializer<string>.Default.Initialize(ref instance, reference);

			Assert.That(instance, Is.EqualTo(reference));
		}

		[Test]
		public void InitializeValue()
		{
			var instance1 = new DummyValue();
			var instance2 = default(DummyValue);
			var instance3 = new DummyValue { Value = 2, SubValue = new SubDummyValue { Value = "Boba" } };
			var reference = new DummyValue { Value = 3, SubValue = new SubDummyValue { Value = "Fett" } };

			Initializer<DummyValue>.Default.Initialize(ref instance1, reference);
			Initializer<DummyValue>.Default.Initialize(ref instance2, reference);
			Initializer<DummyValue>.Default.Initialize(ref instance3, reference);

			Assert.That(instance1, Is.EqualTo(reference));
			Assert.That(instance1.Value, Is.EqualTo(reference.Value));
			Assert.That(instance1.SubValue, Is.EqualTo(reference.SubValue));
			Assert.That(instance2, Is.EqualTo(reference));
			Assert.That(instance3, Is.EqualTo(reference));
			Assert.That(instance3.Value, Is.EqualTo(reference.Value));
			Assert.That(instance3.SubValue.Value, Is.EqualTo(reference.SubValue.Value));
		}

		[Test]
		public void InitializeReference()
		{
			var instance1 = new DummyReference();
			var instance2 = default(DummyReference);
			var instance3 = new DummyReference { Value = 2, SubReference = new SubDummyReference { Value = "Boba" } };
			var reference = new DummyReference { Value = 3, SubReference = new SubDummyReference { Value = "Fett" } };

			Initializer<DummyReference>.Default.Initialize(ref instance1, reference);
			Initializer<DummyReference>.Default.Initialize(ref instance2, reference);
			Initializer<DummyReference>.Default.Initialize(ref instance3, reference);

			Assert.That(instance1, !Is.EqualTo(reference));
			Assert.That(instance1.Value, Is.EqualTo(reference.Value));
			Assert.That(instance1.SubReference, Is.EqualTo(reference.SubReference));
			Assert.That(instance2, Is.EqualTo(reference));
			Assert.That(instance3, !Is.EqualTo(reference));
			Assert.That(instance3.Value, Is.EqualTo(reference.Value));
			Assert.That(instance3.SubReference, !Is.EqualTo(reference.SubReference));
			Assert.That(instance3.SubReference.Value, Is.EqualTo(reference.SubReference.Value));
		}

		[Test]
		public void InitializeIntArray()
		{
			var instance = new[] { 0, 1, 2 };
			var reference = new[] { 3, 4, 5, 6, 7, 8, 9 };

			Initializer<int[]>.Default.Initialize(ref instance, reference);

			Assert.That(instance, !Is.SameAs(reference));
			Assert.That(instance.Length, Is.EqualTo(reference.Length));
			Assert.That(instance.ContentEquals(reference));
		}

		[Test]
		public void InitializeStringArray()
		{
			var instance = new[] { "A", "B", "C" };
			var reference = new[] { "D", "E" };

			Initializer<string[]>.Default.Initialize(ref instance, reference);

			Assert.That(instance, !Is.SameAs(reference));
			Assert.That(instance.Length, Is.EqualTo(reference.Length));
			Assert.That(instance.ContentEquals(reference));
		}

		[Test]
		public void InitializeValueArray()
		{
			var instance = new[]
			{
				default(DummyValue),
				default(DummyValue),
				default(DummyValue),
				default(DummyValue),
				default(DummyValue),
			};
			var reference = new[]
			{
				new DummyValue { Value = 1 },
				new DummyValue { Value = 2, SubValue = new SubDummyValue() },
				new DummyValue { Value = 3, SubValue = new SubDummyValue { Value = "A" } }
			};

			Initializer<DummyValue[]>.Default.Initialize(ref instance, reference);

			Assert.That(instance, !Is.SameAs(reference));
			Assert.That(instance.Length, Is.EqualTo(reference.Length));
			Assert.That(instance.ContentEquals(reference));
		}

		[Test]
		public void InitializeReferenceArray()
		{
			var instance = new[]
			{
				default(DummyReference),
				default(DummyReference),
				default(DummyReference),
				default(DummyReference),
				default(DummyReference),
			};
			var reference = new[]
			{
				new DummyReference{ Value = 1 },
				new DummyReference{ Value = 2, SubReference = new SubDummyReference() },
				new DummyReference{ Value = 3, SubReference = new SubDummyReference { Value = "A" } }
			};

			Initializer<DummyReference[]>.Default.Initialize(ref instance, reference);

			Assert.That(instance, !Is.SameAs(reference));
			Assert.That(instance.Length, Is.EqualTo(reference.Length));
			Assert.That(!instance.Contains(null));
			Assert.That(instance.ContentEquals(reference, (a, b) =>
				a.Value == b.Value &&
				(a.SubReference == b.SubReference || a.SubReference.Value == b.SubReference.Value)));
		}

		[Test]
		public void InitializationCycle()
		{
			var instance = new DummyCycle();
			instance.Dummy = instance;
			var reference = new DummyCycle();
			reference.Dummy = reference;

			Initializer<DummyCycle>.Default.Initialize(ref instance, reference);
			Assert.That(instance, !Is.SameAs(reference));
			Assert.That(instance.Dummy, !Is.SameAs(reference.Dummy));
			Assert.That(instance.Dummy, Is.SameAs(instance.Dummy));
			Assert.That(reference.Dummy, Is.SameAs(reference.Dummy));
		}

		public struct DummyValue
		{
			public int Value;
			public SubDummyValue SubValue;
		}

		public struct SubDummyValue
		{
			public string Value { get; set; }
		}

		public class DummyReference
		{
			public int Value { get; set; }
			public SubDummyReference SubReference;
		}

		public class SubDummyReference
		{
			public string Value;
		}

		public class DummyCycle
		{
			public DummyCycle Dummy;
		}
	}
}
