using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using NUnit.Framework;

namespace Pseudo.Reflection.Tests
{
	public class PropertyWrapperTests : ReflectionTestsBase
	{
		object dummyClass;
		object dummyStruct;
		IFieldOrPropertyWrapper classValueWrapper;
		IFieldOrPropertyWrapper classReferenceWrapper;
		IFieldOrPropertyWrapper structValueWrapper;
		IFieldOrPropertyWrapper structReferenceWrapper;

		public override void Setup()
		{
			base.Setup();

			dummyClass = new DummyClass { Value = 1, Reference = "2" };
			dummyStruct = new DummyStruct { Value = 3, Reference = "4" };
			classValueWrapper = ReflectionUtility.CreatePropertyWrapper(typeof(DummyClass).GetProperty("Value"));
			classReferenceWrapper = ReflectionUtility.CreatePropertyWrapper(typeof(DummyClass).GetProperty("Reference"));
			structValueWrapper = ReflectionUtility.CreatePropertyWrapper(typeof(DummyStruct).GetProperty("Value"));
			structReferenceWrapper = ReflectionUtility.CreatePropertyWrapper(typeof(DummyStruct).GetProperty("Reference"));
		}

		public override void TearDown()
		{
			base.TearDown();

			dummyClass = null;
			dummyStruct = null;
			classValueWrapper = null;
			classReferenceWrapper = null;
			structValueWrapper = null;
			structReferenceWrapper = null;
		}

		[Test]
		public void Get()
		{
			Assert.That(classValueWrapper.Get(ref dummyClass), Is.EqualTo(1));
			Assert.That(classReferenceWrapper.Get(ref dummyClass), Is.EqualTo("2"));
			Assert.That(structValueWrapper.Get(ref dummyStruct), Is.EqualTo(3));
			Assert.That(structReferenceWrapper.Get(ref dummyStruct), Is.EqualTo("4"));
		}

		[Test]
		public void Set()
		{
			classValueWrapper.Set(ref dummyClass, 5);
			classReferenceWrapper.Set(ref dummyClass, "6");
			structValueWrapper.Set(ref dummyStruct, 7);
			structReferenceWrapper.Set(ref dummyStruct, "8");

			Assert.That(classValueWrapper.Get(ref dummyClass), Is.EqualTo(5));
			Assert.That(classReferenceWrapper.Get(ref dummyClass), Is.EqualTo("6"));
			Assert.That(structValueWrapper.Get(ref dummyStruct), Is.EqualTo(7));
			Assert.That(structReferenceWrapper.Get(ref dummyStruct), Is.EqualTo("8"));
		}

		public class DummyClass
		{
			public int Value
			{
				get { return value; }
				set { this.value = value; }
			}

			public string Reference
			{
				get { return reference; }
				set { reference = value; }
			}

			int value;
			string reference;
		}

		public struct DummyStruct
		{
			public int Value
			{
				get { return value; }
				set { this.value = value; }
			}

			public string Reference
			{
				get { return reference; }
				set { reference = value; }
			}

			int value;
			string reference;
		}
	}
}
