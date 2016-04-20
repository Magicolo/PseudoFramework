using UnityEngine;
using System.Collections;
using NUnit.Framework;

namespace Pseudo.Reflection.Tests
{
	public class ReflectionTestBase
	{
		[SetUp]
		public virtual void Setup() { }

		[TearDown]
		public virtual void TearDown() { }
	}
}