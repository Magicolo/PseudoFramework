using UnityEngine;
using System.Collections;
using NUnit.Framework;

namespace Pseudo.Reflection.Tests
{
	public abstract class ReflectionTestsBase
	{
		[SetUp]
		public virtual void Setup() { }

		[TearDown]
		public virtual void TearDown() { }
	}
}