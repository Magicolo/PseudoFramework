using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using NUnit.Framework;

namespace Pseudo.Pooling.Tests
{
	public abstract class PoolingTestsBase
	{
		[SetUp]
		public virtual void Setup() { }

		[TearDown]
		public virtual void TearDown() { }
	}
}
