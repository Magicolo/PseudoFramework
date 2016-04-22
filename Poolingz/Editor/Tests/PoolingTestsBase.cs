using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using NUnit.Framework;
using Pseudo.Pooling;
using Pseudo.Pooling.Internal;
using NSubstitute;

namespace Pseudo.Pooling.Tests
{
	public abstract class PoolingTestsBase
	{

	}

	[Serializable]
	public class DummyPoolable : IPoolFieldsInitializable, IPoolInitializable
	{
		public float Value1;
		public string Value2;
		[DoNotInitialize]
		public bool Value3;
		public ScriptableObject Value4;
		[InitializeContent]
		public DummyContent Value5;
		public int[] Value6;
		public List<char> Value7;
		[InitializeContent]
		public DummyContent[] Value8;
		[InitializeContent]
		public List<DummyContent> Value9;
		[InitializeContent]
		public object Value10;

		public void OnPrePoolInitialize() { }

		public void OnPostPoolInitialize() { }

		public void OnPrePoolFieldsInitialize(IFieldInitializer initializer) { }

		public void OnPostPoolFieldsInitialize(IFieldInitializer initializer) { }
	}

	public class DummyScriptable : ScriptableObject { }

	public class DummyContent : IEquatable<DummyContent>
	{
		public float Value1;
		public string Value2;

		public bool Equals(DummyContent other)
		{
			return Value1 == other.Value1 && Value2 == other.Value2;
		}
	}

	public class DummySub : DummyBase
	{
		public string ValueSub
		{
			get { return value; }
			set { this.value = value; }
		}

		string value;
	}

	public class DummyBase
	{
		public string ValueBase
		{
			get { return value; }
			set { this.value = value; }
		}

		string value;
	}
}
