using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using NUnit.Framework;

namespace Pseudo.Groupingz.Tests
{
	public class GroupTests : GroupingTestsBase
	{
		[Test]
		public void RegisterUnregisterSingle()
		{
			var dummy = new Dummy();
			var group1 = GroupManager.GetGroup(MatchType.All, "Boba");
			var group2 = GroupManager.GetGroup(MatchType.All, "Fett");

			Assert.That(group1, !Is.SameAs(group2));
			Assert.That(group1.Count, Is.EqualTo(0));
			Assert.That(group2.Count, Is.EqualTo(0));
			GroupManager.Register(dummy, "Boba");
			Assert.That(group1.Count, Is.EqualTo(1));
			Assert.That(group2.Count, Is.EqualTo(0));
			Assert.That(group1[0], Is.SameAs(dummy));
			GroupManager.Register(dummy, "Fett");
			Assert.That(group1.Count, Is.EqualTo(1));
			Assert.That(group2.Count, Is.EqualTo(1));
			Assert.That(group1[0], Is.SameAs(dummy));
			Assert.That(group2[0], Is.SameAs(dummy));
			GroupManager.Unregister(dummy, "Fett");
			Assert.That(group1.Count, Is.EqualTo(1));
			Assert.That(group2.Count, Is.EqualTo(0));
			Assert.That(group1[0], Is.EqualTo(dummy));
			GroupManager.Unregister(dummy, "Boba");
			Assert.That(group1.Count, Is.EqualTo(0));
			Assert.That(group2.Count, Is.EqualTo(0));
		}

		[Test]
		public void RegisterUnregisterMultiple()
		{
			var dummy = new Dummy();
			var group = GroupManager.GetGroup(MatchType.All, "Boba", "Fett");

			Assert.That(group.Count, Is.EqualTo(0));
			GroupManager.Register(dummy, "Boba");
			Assert.That(group.Count, Is.EqualTo(0));
			GroupManager.Register(dummy, "Fett");
			Assert.That(group.Count, Is.EqualTo(1));
			Assert.That(group[0], Is.SameAs(dummy));
			GroupManager.Unregister(dummy, "Fett");
			Assert.That(group.Count, Is.EqualTo(0));
			GroupManager.Unregister(dummy, "Boba");
			Assert.That(group.Count, Is.EqualTo(0));
		}

		[Test]
		public void RegisterUnregisterDuplicates()
		{
			var dummy = new Dummy();
			var group1 = GroupManager.GetGroup(MatchType.All, "Boba");
			var group2 = GroupManager.GetGroup(MatchType.All, "Boba", "Boba");

			Assert.That(group1.Count, Is.EqualTo(0));
			Assert.That(group2.Count, Is.EqualTo(0));
			GroupManager.Register(dummy, "Boba");
			Assert.That(group1.Count, Is.EqualTo(1));
			Assert.That(group2.Count, Is.EqualTo(0));
			GroupManager.Register(dummy, "Boba");
			Assert.That(group1.Count, Is.EqualTo(1));
			Assert.That(group2.Count, Is.EqualTo(1));
			GroupManager.Unregister(dummy, "Boba");
			Assert.That(group1.Count, Is.EqualTo(1));
			Assert.That(group2.Count, Is.EqualTo(0));
			GroupManager.Unregister(dummy, "Boba");
			Assert.That(group1.Count, Is.EqualTo(0));
			Assert.That(group2.Count, Is.EqualTo(0));
		}

		[Test]
		public void GetCachedGroup()
		{
			var group1 = GroupManager.GetGroup(MatchType.All, "Boba");
			var group2 = GroupManager.GetGroup(MatchType.All, "Boba");
			var group3 = GroupManager.GetGroup(MatchType.All, "Fett");

			Assert.IsNotNull(group1);
			Assert.IsNotNull(group2);
			Assert.IsNotNull(group3);
			Assert.That(group1, Is.SameAs(group2));
			Assert.That(group1, !Is.SameAs(group3));
			Assert.That(group2, !Is.SameAs(group3));
		}

		[Test]
		public void GroupEvents()
		{
			var dummy = new Dummy();
			var group = GroupManager.GetGroup(MatchType.All, "Boba");
			int addedCounter = 0;
			int removedCounter = 0;
			group.OnAdded += d => { addedCounter++; Assert.That(dummy, Is.SameAs(d)); };
			group.OnRemoved += d => { removedCounter++; Assert.That(dummy, Is.SameAs(d)); };

			Assert.That(addedCounter, Is.EqualTo(0));
			Assert.That(removedCounter, Is.EqualTo(0));
			GroupManager.Register(dummy, "Boba");
			Assert.That(addedCounter, Is.EqualTo(1));
			Assert.That(removedCounter, Is.EqualTo(0));
			GroupManager.Unregister(dummy, "Boba");
			Assert.That(addedCounter, Is.EqualTo(1));
			Assert.That(removedCounter, Is.EqualTo(1));
		}
	}
}
