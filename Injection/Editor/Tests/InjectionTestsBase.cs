using UnityEngine;
using System;
using NUnit.Framework;
using NSubstitute;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection.Internal;

namespace Pseudo.Injection.Tests
{
	public abstract class InjectionTestsBase
	{
		public IContainer Container;

		[SetUp]
		public virtual void Setup()
		{
			Container = new Container();
		}

		[TearDown]
		public virtual void TearDown()
		{
			Container = null;
		}
	}

	public class Dummy1 : IDummy
	{
		[Inject(optional: true)]
		public DummyField Field;
		[Inject(optional: true)]
		public DummyProperty Property { get; set; }
	}

	public class Dummy2 : IDummy
	{
		public DummyField Field;
		public DummyProperty Property { get; set; }
		public Dummy1 Dummy;

		public Dummy2(DummyField field, DummyProperty property, [Inject(optional: true)] Dummy1 dummy)
		{
			Field = field;
			Property = property;
			Dummy = dummy;
		}
	}

	public class Dummy3 : IDummy
	{
		public DummyField Field;
		public DummyProperty Property { get; set; }

		[Inject]
		void InitializeField(DummyField field)
		{
			Field = field;
		}

		[Inject]
		void InitializeProperty(DummyProperty property)
		{
			Property = property;
		}
	}

	public class Dummy4 : IDummy
	{
		[Inject]
		public IDummy Dummy1;
		[Inject(optional: true, identifier: "Boba")]
		public IDummy Dummy2 { get; set; }
	}

	public class Dummy5 : IDummy
	{
		[Inject]
		public int Int;
		[Inject]
		public long Long;
		[Inject]
		public IConvertible Float { get; set; }

		public readonly IComparable Byte1;
		public readonly IComparable Byte2;
		public readonly IComparable Byte3;

		public Dummy5(IComparable byte1, IComparable byte2, IComparable byte3)
		{
			Byte1 = byte1;
			Byte2 = byte2;
			Byte3 = byte3;
		}
	}

	public interface IDummy { }

	public class DummyField
	{
		[Inject(optional: true)]
		public DummySubField SubField;
	}
	public class DummySubField { }
	public class DummyProperty
	{
		[Inject(optional: true)]
		public DummySubProperty SubProperty { get; set; }
	}
	public class DummySubProperty { }
}
