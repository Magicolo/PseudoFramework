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
	public class FieldWrapperTests : ReflectionTestBase
	{
		object dummyClass;
		object dummyStruct;
		IMemberWrapper classValueWrapper;
		IMemberWrapper classReferenceWrapper;
		IMemberWrapper structValueWrapper;
		IMemberWrapper structReferenceWrapper;

		public override void Setup()
		{
			base.Setup();

			dummyClass = new DummyClass { Value = 1, Reference = "2" };
			dummyStruct = new DummyStruct { Value = 3, Reference = "4" };
			classValueWrapper = ReflectionUtility.CreateWrapper(typeof(DummyClass).GetField("Value"));
			classReferenceWrapper = ReflectionUtility.CreateWrapper(typeof(DummyClass).GetField("Reference"));
			structValueWrapper = ReflectionUtility.CreateWrapper(typeof(DummyStruct).GetField("Value"));
			structReferenceWrapper = ReflectionUtility.CreateWrapper(typeof(DummyStruct).GetField("Reference"));
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
			public int Value;
			public string Reference;
		}

		public struct DummyStruct
		{
			public int Value;
			public string Reference;
		}
	}
}
