using UnityEngine;
using System;
using NUnit.Framework;
using NSubstitute;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Tests	
{
	public class ArchitectTest
	{
		Architect architect;

		[SetUp]
		public void Setup()
		{
			architect = new Architect();
		}

		[TearDown]
		public void TearDown()
		{
			architect = null;
		}

		[Test]
		public void OnNewMaptheMapIsEmpty()
		{
			architect.CreateNewMap(null, "Test1", 5, 5);
			Assert.IsEmpty(architect.MapData.Layers);
		}

		[Test]
		public void WithNewMapAdding1Layer_MapContains1Layer()
		{
			architect.CreateNewMap(null, "Test1", 5, 5);
			architect.AddLayerData("bob");
			Assert.IsNotEmpty(architect.MapData.Layers);
			Assert.IsTrue(architect.MapData.Layers.Count == 1);
		}

		[Test]
		public void WithNewMapAdding1Layer_MapRootTransformContains1Layer()
		{
			GameObject parent = new GameObject("Test");
			architect.CreateNewMap(parent.transform, "Test1", 5, 5);
			architect.AddLayerData("bob");
			Assert.IsTrue(parent.transform.childCount == 1);
		}

		[Test]
		public void NewMapRemoveOldLayers()
		{
			architect.CreateNewMap(null, "Test1", 5, 5);
			architect.AddLayerData("bob");

			architect.CreateNewMap(null, "Test2", 5, 5);
			Assert.IsEmpty(architect.MapData.Layers);
		}

		[Test]
		public void NewMapRemoveOldMapGameObjects()
		{
			GameObject parent = new GameObject("Test");
			architect.CreateNewMap(parent.transform, "Test1", 5, 5);
			architect.AddLayerData("bob");

			architect.CreateNewMap(parent.transform, "Test2", 5, 5);
			Assert.IsTrue(parent.transform.childCount == 0);
		}
	}
}
