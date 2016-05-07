using UnityEngine;
using System.Collections;
using NUnit.Framework;

namespace Pseudo.Initialization.Tests
{
	public class InitializationTestsBase
	{
		[SetUp]
		public virtual void Setup() { }

		[TearDown]
		public virtual void TearDown() { }
	}
}