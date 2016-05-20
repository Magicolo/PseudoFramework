using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Groupingz.Internal;
using NUnit.Framework;

namespace Pseudo.Groupingz.Tests
{
	public abstract class GroupingTestsBase
	{
		public IGroupManager<Dummy, string> GroupManager;

		[SetUp]
		public virtual void Setup()
		{
			GroupManager = new GroupManager<Dummy, string>();
		}

		[TearDown]
		public virtual void TearDown()
		{
			GroupManager = null;
		}

		public class Dummy { }
	}
}
