using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using NUnit.Framework;

namespace Pseudo.Groupingz.Tests
{
	public class MatchTests : GroupingTestsBase
	{
		[Test]
		public void MatchAll()
		{
			var dummy1 = new Dummy();
			var dummy2 = new Dummy();
			var dummy3 = new Dummy();
			GroupManager.Register(dummy1, "Boba");
			GroupManager.Register(dummy1, "Fett");
			GroupManager.Register(dummy1, "Jango");
			GroupManager.Register(dummy2, "Fett");
			GroupManager.Register(dummy2, "Jango");
			GroupManager.Register(dummy3, "Jango");
			var group1 = GroupManager.GetGroup(MatchType.All);
			var group2 = GroupManager.GetGroup(MatchType.All, "Boba");
			var group3 = GroupManager.GetGroup(MatchType.All, "Boba", "Fett");
			var group4 = GroupManager.GetGroup(MatchType.All, "Fett", "Jango");
			var group5 = GroupManager.GetGroup(MatchType.All, "Boba", "Fett", "Jango");

			Assert.That(group1.Count, Is.EqualTo(3));
			Assert.That(group1[0], Is.SameAs(dummy1));
			Assert.That(group1[1], Is.SameAs(dummy2));
			Assert.That(group1[2], Is.SameAs(dummy3));
			Assert.That(group2.Count, Is.EqualTo(1));
			Assert.That(group2[0], Is.SameAs(dummy1));
			Assert.That(group3.Count, Is.EqualTo(1));
			Assert.That(group3[0], Is.SameAs(dummy1));
			Assert.That(group4.Count, Is.EqualTo(2));
			Assert.That(group4[0], Is.SameAs(dummy1));
			Assert.That(group4[1], Is.SameAs(dummy2));
			Assert.That(group5.Count, Is.EqualTo(1));
			Assert.That(group5[0], Is.SameAs(dummy1));
		}

		[Test]
		public void MatchAny()
		{
			var dummy1 = new Dummy();
			var dummy2 = new Dummy();
			var dummy3 = new Dummy();
			GroupManager.Register(dummy1, "Boba");
			GroupManager.Register(dummy1, "Fett");
			GroupManager.Register(dummy1, "Jango");
			GroupManager.Register(dummy2, "Fett");
			GroupManager.Register(dummy2, "Jango");
			GroupManager.Register(dummy3, "Jango");
			var group1 = GroupManager.GetGroup(MatchType.Any);
			var group2 = GroupManager.GetGroup(MatchType.Any, "Boba");
			var group3 = GroupManager.GetGroup(MatchType.Any, "Boba", "Fett");
			var group4 = GroupManager.GetGroup(MatchType.Any, "Fett", "Jango");
			var group5 = GroupManager.GetGroup(MatchType.Any, "Boba", "Fett", "Jango");

			Assert.That(group1.Count, Is.EqualTo(3));
			Assert.That(group1[0], Is.SameAs(dummy1));
			Assert.That(group1[1], Is.SameAs(dummy2));
			Assert.That(group1[2], Is.SameAs(dummy3));
			Assert.That(group2.Count, Is.EqualTo(1));
			Assert.That(group2[0], Is.SameAs(dummy1));
			Assert.That(group3.Count, Is.EqualTo(2));
			Assert.That(group3[0], Is.SameAs(dummy1));
			Assert.That(group3[1], Is.SameAs(dummy2));
			Assert.That(group4.Count, Is.EqualTo(3));
			Assert.That(group4[0], Is.SameAs(dummy1));
			Assert.That(group4[1], Is.SameAs(dummy2));
			Assert.That(group4[2], Is.SameAs(dummy3));
			Assert.That(group5.Count, Is.EqualTo(3));
			Assert.That(group5[0], Is.SameAs(dummy1));
			Assert.That(group5[1], Is.SameAs(dummy2));
			Assert.That(group5[2], Is.SameAs(dummy3));
		}

		[Test]
		public void MatchNone()
		{
			var dummy1 = new Dummy();
			var dummy2 = new Dummy();
			var dummy3 = new Dummy();
			GroupManager.Register(dummy1, "Boba");
			GroupManager.Register(dummy1, "Fett");
			GroupManager.Register(dummy1, "Jango");
			GroupManager.Register(dummy2, "Fett");
			GroupManager.Register(dummy2, "Jango");
			GroupManager.Register(dummy3, "Jango");
			var group1 = GroupManager.GetGroup(MatchType.None);
			var group2 = GroupManager.GetGroup(MatchType.None, "Boba");
			var group3 = GroupManager.GetGroup(MatchType.None, "Boba", "Fett");
			var group4 = GroupManager.GetGroup(MatchType.None, "Fett", "Jango");
			var group5 = GroupManager.GetGroup(MatchType.None, "Boba", "Fett", "Jango");

			Assert.That(group1.Count, Is.EqualTo(0));
			Assert.That(group2.Count, Is.EqualTo(2));
			Assert.That(group2[0], Is.SameAs(dummy2));
			Assert.That(group2[1], Is.SameAs(dummy3));
			Assert.That(group3.Count, Is.EqualTo(1));
			Assert.That(group3[0], Is.SameAs(dummy3));
			Assert.That(group4.Count, Is.EqualTo(0));
			Assert.That(group5.Count, Is.EqualTo(0));
		}

		[Test]
		public void MatchExact()
		{
			var dummy1 = new Dummy();
			var dummy2 = new Dummy();
			var dummy3 = new Dummy();
			GroupManager.Register(dummy1, "Boba");
			GroupManager.Register(dummy1, "Fett");
			GroupManager.Register(dummy1, "Jango");
			GroupManager.Register(dummy2, "Fett");
			GroupManager.Register(dummy2, "Jango");
			GroupManager.Register(dummy3, "Jango");
			var group1 = GroupManager.GetGroup(MatchType.Exact);
			var group2 = GroupManager.GetGroup(MatchType.Exact, "Boba");
			var group3 = GroupManager.GetGroup(MatchType.Exact, "Boba", "Fett");
			var group4 = GroupManager.GetGroup(MatchType.Exact, "Fett", "Jango");
			var group5 = GroupManager.GetGroup(MatchType.Exact, "Boba", "Fett", "Jango");

			Assert.That(group1.Count, Is.EqualTo(0));
			Assert.That(group2.Count, Is.EqualTo(0));
			Assert.That(group3.Count, Is.EqualTo(0));
			Assert.That(group4.Count, Is.EqualTo(1));
			Assert.That(group4[0], Is.SameAs(dummy2));
			Assert.That(group5.Count, Is.EqualTo(1));
			Assert.That(group5[0], Is.SameAs(dummy1));
		}
	}
}
