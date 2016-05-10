using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using NUnit.Framework;

namespace Pseudo.Tests
{
	public class ByteFlagTests
	{
		[Test]
		public void AddByte()
		{
			var flagsA = new ByteFlag(1, 2, 3);
			var flagsB = flagsA + 4;

			Assert.That(flagsA, !Is.EqualTo(flagsB));
			Assert.IsFalse(flagsA[4]);
			Assert.IsTrue(flagsB[4]);
		}

		[Test]
		public void SubtractByte()
		{
			var flagsA = new ByteFlag(1, 2, 3);
			var flagsB = flagsA - 3;

			Assert.That(flagsA, !Is.EqualTo(flagsB));
			Assert.IsTrue(flagsA[3]);
			Assert.IsFalse(flagsB[3]);
		}

		[Test]
		public void AddFlags()
		{
			var flagsA = new ByteFlag(1, 2, 3);
			var flagsB = flagsA + new ByteFlag(3, 4);

			Assert.That(flagsA, !Is.EqualTo(flagsB));
			Assert.IsFalse(flagsA[4]);
			Assert.IsTrue(flagsB[4]);
		}

		[Test]
		public void SubtractFlags()
		{
			var flagsA = new ByteFlag(1, 2, 3);
			var flagsB = flagsA - new ByteFlag(4, 3);

			Assert.That(flagsA, !Is.EqualTo(flagsB));
			Assert.IsTrue(flagsA[3]);
			Assert.IsFalse(flagsB[3]);
		}

		[Test]
		public void AndFlags()
		{
			var flagsA = new ByteFlag(1, 2, 3);
			var flagsB = flagsA & new ByteFlag(3, 4);

			Assert.That(flagsA, !Is.EqualTo(flagsB));
			Assert.That(flagsB == new ByteFlag(3));
		}

		[Test]
		public void OrFlags()
		{
			var flagsA = new ByteFlag(1, 2, 3);
			var flagsB = flagsA | new ByteFlag(3, 4);

			Assert.That(flagsA, !Is.EqualTo(flagsB));
			Assert.That(flagsB == new ByteFlag(1, 2, 3, 4));
		}

		[Test]
		public void XorFlags()
		{
			var flagsA = new ByteFlag(1, 2, 3);
			var flagsB = flagsA ^ new ByteFlag(3, 4);

			Assert.That(flagsA, !Is.EqualTo(flagsB));
			Assert.That(flagsB == new ByteFlag(1, 2, 4));
		}

		[Test]
		public void HasAll()
		{
			var flags = new ByteFlag(1, 2, 3);

			Assert.IsTrue(flags.HasAll(new ByteFlag(1, 2, 3)));
			Assert.IsTrue(flags.HasAll(new ByteFlag(1)));
			Assert.IsTrue(flags.HasAll(ByteFlag.Nothing));
			Assert.IsTrue(ByteFlag.Everything.HasAll(flags));
			Assert.IsFalse(flags.HasAll(new ByteFlag(1, 2, 4)));
			Assert.IsFalse(flags.HasAll(new ByteFlag(4, 5, 6)));
			Assert.IsFalse(flags.HasAll(ByteFlag.Everything));
			Assert.IsFalse(ByteFlag.Nothing.HasAll(flags));
		}

		[Test]
		public void HasAny()
		{
			var flags = new ByteFlag(1, 2, 3);

			Assert.IsTrue(flags.HasAny(new ByteFlag(1, 2, 3)));
			Assert.IsTrue(flags.HasAny(new ByteFlag(1)));
			Assert.IsTrue(flags.HasAny(new ByteFlag(1, 2, 4)));
			Assert.IsTrue(flags.HasAny(ByteFlag.Everything));
			Assert.IsTrue(ByteFlag.Everything.HasAny(flags));
			Assert.IsFalse(flags.HasAny(new ByteFlag(4, 5, 6)));
			Assert.IsFalse(flags.HasAny(ByteFlag.Nothing));
			Assert.IsFalse(ByteFlag.Nothing.HasAny(flags));
		}

		[Test]
		public void HasNone()
		{
			var flags = new ByteFlag(1, 2, 3);

			Assert.IsTrue(flags.HasNone(new ByteFlag(4, 5, 6)));
			Assert.IsTrue(flags.HasNone(ByteFlag.Nothing));
			Assert.IsTrue(ByteFlag.Nothing.HasNone(flags));
			Assert.IsFalse(flags.HasNone(new ByteFlag(1, 2, 3)));
			Assert.IsFalse(flags.HasNone(new ByteFlag(1)));
			Assert.IsFalse(flags.HasNone(new ByteFlag(1, 2, 4)));
			Assert.IsFalse(flags.HasNone(ByteFlag.Everything));
			Assert.IsFalse(ByteFlag.Everything.HasNone(flags));
		}
	}
}
