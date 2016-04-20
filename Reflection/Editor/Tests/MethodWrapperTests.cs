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
	public class MethodWrapperTests : ReflectionTestBase
	{
		object dummyClass;
		object dummyStruct;

		IMethodWrapper classMethod1Wrapper;
		IMethodWrapper classMethod2Wrapper;
		IMethodWrapper classMethod3Wrapper;
		IMethodWrapper classMethod4Wrapper;
		IMethodWrapper classMethod5Wrapper;
		IMethodWrapper classMethod6Wrapper;
		IMethodWrapper classMethod7Wrapper;
		IMethodWrapper classMethod8Wrapper;

		IMethodWrapper structMethod1Wrapper;
		IMethodWrapper structMethod2Wrapper;
		IMethodWrapper structMethod3Wrapper;
		IMethodWrapper structMethod4Wrapper;
		IMethodWrapper structMethod5Wrapper;
		IMethodWrapper structMethod6Wrapper;
		IMethodWrapper structMethod7Wrapper;
		IMethodWrapper structMethod8Wrapper;

		public override void Setup()
		{
			base.Setup();

			dummyClass = new DummyClass();
			dummyStruct = new DummyStruct();

			classMethod1Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyClass).GetMethod("Method1"));
			classMethod2Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyClass).GetMethod("Method2"));
			classMethod3Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyClass).GetMethod("Method3"));
			classMethod4Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyClass).GetMethod("Method4"));
			classMethod5Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyClass).GetMethod("Method5"));
			classMethod6Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyClass).GetMethod("Method6"));
			classMethod7Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyClass).GetMethod("Method7"));
			classMethod8Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyClass).GetMethod("Method8"));

			structMethod1Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyStruct).GetMethod("Method1"));
			structMethod2Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyStruct).GetMethod("Method2"));
			structMethod3Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyStruct).GetMethod("Method3"));
			structMethod4Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyStruct).GetMethod("Method4"));
			structMethod5Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyStruct).GetMethod("Method5"));
			structMethod6Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyStruct).GetMethod("Method6"));
			structMethod7Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyStruct).GetMethod("Method7"));
			structMethod8Wrapper = ReflectionUtility.CreateWrapper(typeof(DummyStruct).GetMethod("Method8"));
		}

		public override void TearDown()
		{
			base.TearDown();

			dummyClass = null;
			dummyStruct = null;

			classMethod1Wrapper = null;
			classMethod2Wrapper = null;
			classMethod3Wrapper = null;
			classMethod4Wrapper = null;
			classMethod5Wrapper = null;
			classMethod6Wrapper = null;
			classMethod7Wrapper = null;
			classMethod8Wrapper = null;

			structMethod1Wrapper = null;
			structMethod2Wrapper = null;
			structMethod3Wrapper = null;
			structMethod4Wrapper = null;
			structMethod5Wrapper = null;
			structMethod6Wrapper = null;
			structMethod7Wrapper = null;
			structMethod8Wrapper = null;
		}

		[Test]
		public void InvokeClass()
		{
			Assert.That(((DummyClass)dummyClass).Calls, Is.EqualTo(0));

			Assert.IsNull(classMethod1Wrapper.Invoke(ref dummyClass));
			Assert.That(((DummyClass)dummyClass).Calls, Is.EqualTo(1));

			Assert.IsNull(classMethod2Wrapper.Invoke(ref dummyClass, 2));
			Assert.That(((DummyClass)dummyClass).Calls, Is.EqualTo(3));

			Assert.IsNull(classMethod3Wrapper.Invoke(ref dummyClass, 3, "3"));
			Assert.That(((DummyClass)dummyClass).Calls, Is.EqualTo(6));

			Assert.IsNull(classMethod4Wrapper.Invoke(ref dummyClass, 4, "4", new object()));
			Assert.That(((DummyClass)dummyClass).Calls, Is.EqualTo(10));

			Assert.That(classMethod5Wrapper.Invoke(ref dummyClass), Is.EqualTo(dummyClass));
			Assert.That(((DummyClass)dummyClass).Calls, Is.EqualTo(11));

			Assert.That(classMethod6Wrapper.Invoke(ref dummyClass, 5), Is.EqualTo(dummyClass));
			Assert.That(((DummyClass)dummyClass).Calls, Is.EqualTo(16));

			Assert.That(classMethod7Wrapper.Invoke(ref dummyClass, 6, "6"), Is.EqualTo(dummyClass));
			Assert.That(((DummyClass)dummyClass).Calls, Is.EqualTo(22));

			Assert.That(classMethod8Wrapper.Invoke(ref dummyClass, 7, "7", new object()), Is.EqualTo(dummyClass));
			Assert.That(((DummyClass)dummyClass).Calls, Is.EqualTo(29));
		}

		[Test]
		public void InvokeStruct()
		{
			Assert.That(((DummyStruct)dummyStruct).Calls, Is.EqualTo(0));

			Assert.IsNull(structMethod1Wrapper.Invoke(ref dummyStruct));
			Assert.That(((DummyStruct)dummyStruct).Calls, Is.EqualTo(1));

			Assert.IsNull(structMethod2Wrapper.Invoke(ref dummyStruct, 2));
			Assert.That(((DummyStruct)dummyStruct).Calls, Is.EqualTo(3));

			Assert.IsNull(structMethod3Wrapper.Invoke(ref dummyStruct, 3, "3"));
			Assert.That(((DummyStruct)dummyStruct).Calls, Is.EqualTo(6));

			Assert.IsNull(structMethod4Wrapper.Invoke(ref dummyStruct, 4, "4", new object()));
			Assert.That(((DummyStruct)dummyStruct).Calls, Is.EqualTo(10));

			Assert.That(structMethod5Wrapper.Invoke(ref dummyStruct), Is.EqualTo(dummyStruct));
			Assert.That(((DummyStruct)dummyStruct).Calls, Is.EqualTo(11));

			Assert.That(structMethod6Wrapper.Invoke(ref dummyStruct, 5), Is.EqualTo(dummyStruct));
			Assert.That(((DummyStruct)dummyStruct).Calls, Is.EqualTo(16));

			Assert.That(structMethod7Wrapper.Invoke(ref dummyStruct, 6, "6"), Is.EqualTo(dummyStruct));
			Assert.That(((DummyStruct)dummyStruct).Calls, Is.EqualTo(22));

			Assert.That(structMethod8Wrapper.Invoke(ref dummyStruct, 7, "7", new object()), Is.EqualTo(dummyStruct));
			Assert.That(((DummyStruct)dummyStruct).Calls, Is.EqualTo(29));
		}

		public class DummyClass
		{
			public int Calls;

			public void Method1() { Calls++; }
			public void Method2(int argument1) { Calls += argument1; }
			public void Method3(int argument1, string argument2) { Calls += argument1; }
			public void Method4(int argument1, string argument2, object argument3) { Calls += argument1; }
			public DummyClass Method5() { Calls++; return this; }
			public DummyClass Method6(int argument1) { Calls += argument1; return this; }
			public DummyClass Method7(int argument1, string argument2) { Calls += argument1; return this; }
			public DummyClass Method8(int argument1, string argument2, object argument3) { Calls += argument1; return this; }
		}

		public struct DummyStruct
		{
			public int Calls;

			public void Method1() { Calls++; }
			public void Method2(int argument1) { Calls += argument1; }
			public void Method3(int argument1, string argument2) { Calls += argument1; }
			public void Method4(int argument1, string argument2, object argument3) { Calls += argument1; }
			public DummyStruct Method5() { Calls++; return this; }
			public DummyStruct Method6(int argument1) { Calls += argument1; return this; }
			public DummyStruct Method7(int argument1, string argument2) { Calls += argument1; return this; }
			public DummyStruct Method8(int argument1, string argument2, object argument3) { Calls += argument1; return this; }
		}
	}
}
