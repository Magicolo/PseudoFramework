using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using NUnit.Framework;

namespace Pseudo.Reflection.Tests
{
	public class ConstructorWrapperTests : ReflectionTestBase
	{
		IConstructorWrapper classConstructor1Wrapper;
		IConstructorWrapper classConstructor2Wrapper;
		IConstructorWrapper classConstructor3Wrapper;
		IConstructorWrapper classConstructor4Wrapper;

		IConstructorWrapper structConstructor1Wrapper;
		IConstructorWrapper structConstructor2Wrapper;
		IConstructorWrapper structConstructor3Wrapper;
		IConstructorWrapper structConstructor4Wrapper;

		public override void Setup()
		{
			base.Setup();

			classConstructor1Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyClass));
			classConstructor2Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyClass).GetConstructor(new[] { typeof(int) }));
			classConstructor3Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyClass).GetConstructor(new[] { typeof(int), typeof(string) }));
			classConstructor4Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyClass).GetConstructor(new[] { typeof(int), typeof(string), typeof(object) }));

			structConstructor1Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyStruct));
			structConstructor2Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyStruct).GetConstructor(new[] { typeof(int) }));
			structConstructor3Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyStruct).GetConstructor(new[] { typeof(int), typeof(string) }));
			structConstructor4Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyStruct).GetConstructor(new[] { typeof(int), typeof(string), typeof(object) }));
		}

		public override void TearDown()
		{
			base.TearDown();

			classConstructor1Wrapper = null;
			classConstructor2Wrapper = null;
			classConstructor3Wrapper = null;
			classConstructor4Wrapper = null;

			structConstructor1Wrapper = null;
			structConstructor2Wrapper = null;
			structConstructor3Wrapper = null;
			structConstructor4Wrapper = null;
		}

		[Test]
		public void ConstructClass()
		{
			var instance1 = (DummyClass)classConstructor1Wrapper.Construct();
			var instance2 = (DummyClass)classConstructor2Wrapper.Construct(1);
			var instance3 = (DummyClass)classConstructor3Wrapper.Construct(2, "2");
			var instance4 = (DummyClass)classConstructor4Wrapper.Construct(3, "3", instance1);

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.IsNotNull(instance3);
			Assert.IsNotNull(instance4);

			Assert.That(instance1.Value, Is.EqualTo(0));
			Assert.That(instance1.Reference, Is.EqualTo(null));
			Assert.That(instance1.Object, Is.EqualTo(null));

			Assert.That(instance2.Value, Is.EqualTo(1));
			Assert.That(instance2.Reference, Is.EqualTo(null));
			Assert.That(instance2.Object, Is.EqualTo(null));

			Assert.That(instance3.Value, Is.EqualTo(2));
			Assert.That(instance3.Reference, Is.EqualTo("2"));
			Assert.That(instance3.Object, Is.EqualTo(null));

			Assert.That(instance4.Value, Is.EqualTo(3));
			Assert.That(instance4.Reference, Is.EqualTo("3"));
			Assert.That(instance4.Object, Is.EqualTo(instance1));
		}

		[Test]
		public void ConstructStruct()
		{
			var instance1 = (DummyStruct)structConstructor1Wrapper.Construct();
			var instance2 = (DummyStruct)structConstructor2Wrapper.Construct(1);
			var instance3 = (DummyStruct)structConstructor3Wrapper.Construct(2, "2");
			var instance4 = (DummyStruct)structConstructor4Wrapper.Construct(3, "3", instance1);

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.IsNotNull(instance3);
			Assert.IsNotNull(instance4);

			Assert.That(instance1.Value, Is.EqualTo(0));
			Assert.That(instance1.Reference, Is.EqualTo(null));
			Assert.That(instance1.Object, Is.EqualTo(null));

			Assert.That(instance2.Value, Is.EqualTo(1));
			Assert.That(instance2.Reference, Is.EqualTo(null));
			Assert.That(instance2.Object, Is.EqualTo(null));

			Assert.That(instance3.Value, Is.EqualTo(2));
			Assert.That(instance3.Reference, Is.EqualTo("2"));
			Assert.That(instance3.Object, Is.EqualTo(null));

			Assert.That(instance4.Value, Is.EqualTo(3));
			Assert.That(instance4.Reference, Is.EqualTo("3"));
			Assert.That(instance4.Object, Is.EqualTo(instance1));
		}

		public class DummyClass
		{
			public readonly int Value;
			public readonly string Reference;
			public readonly object Object;

			public DummyClass() { }

			public DummyClass(int value) : this()
			{
				Value = value;
			}

			public DummyClass(int value, string reference) : this(value)
			{
				Reference = reference;
			}

			public DummyClass(int value, string reference, object obj) : this(value, reference)
			{
				Object = obj;
			}
		}

		public struct DummyStruct
		{
			public readonly int Value;
			public readonly string Reference;
			public readonly object Object;

			public DummyStruct(int value) : this()
			{
				Value = value;
			}

			public DummyStruct(int value, string reference) : this(value)
			{
				Reference = reference;
			}

			public DummyStruct(int value, string reference, object obj) : this(value, reference)
			{
				Object = obj;
			}
		}
	}
}
