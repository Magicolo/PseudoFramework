using UnityEngine;
using System;
using NUnit.Framework;
using NSubstitute;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Architect;

namespace Pseudo.Architect.Tests
{
	public class NewMap
	{
		ArchitectControler architect;

		[SetUp]
		public void Setup()
		{
			architect = new ArchitectControler();
		}

		[TearDown]
		public void TearDown()
		{
			architect = null;
		}

		[Test]
		public void A1_OnNewMaptheMapIsEmpty()
		{
			architect.CreateNewMap(null, "Test1", 5, 5);
			Assert.IsEmpty(architect.MapData.Layers);
		}

		[Test]
		public void A2_Adding1Layer_NewMapContains1Layer()
		{
			architect.CreateNewMap(null, "Test1", 5, 5);
			architect.AddLayerData("bob");
			Assert.IsNotEmpty(architect.MapData.Layers);
			Assert.AreEqual(1, architect.MapData.Layers.Count);
		}

		[Test]
		public void A3_Adding1Layer_NewMapRootTransformContains1Layer()
		{
			GameObject parent = new GameObject("Test");
			architect.CreateNewMap(parent.transform, "Test1", 5, 5);
			architect.AddLayerData("bob");
			Assert.AreEqual(1, parent.transform.childCount, "There should be only 1 gameobject.");
		}

		[Test]
		public void A4_NewMapRemoveOldLayers()
		{
			architect.CreateNewMap(null, "Test1", 5, 5);
			architect.AddLayerData("bob");

			architect.CreateNewMap(null, "Test2", 5, 5);
			Assert.IsEmpty(architect.MapData.Layers);
		}

		[Test]
		public void A5_NewMapRemoveOldMapGameObjects()
		{
			GameObject parent = new GameObject("Test");
			architect.CreateNewMap(parent.transform, "Test1", 5, 5);
			architect.AddLayerData("bob");

			GameObject parent2 = new GameObject("Test2");
			architect.CreateNewMap(parent2.transform, "Test2", 5, 5);
			Assert.IsTrue(null == parent);
		}

		[Test]
		public void B1_Removing1Layer_LayerIsRemoved()
		{
			GameObject parent = new GameObject("Test");
			architect.CreateNewMap(parent.transform, "TestRemove", 5, 5);
			
			LayerData removeMe = architect.AddLayerData("remove");
			architect.RemoveLayerData(removeMe);
			
			Assert.AreEqual(0, parent.transform.childCount, "There should be 0 gameobject after the remove.");
		}

		[Test]
		public void B2_RemovingNullLayer()
		{
			GameObject parent = new GameObject("Test");
			architect.CreateNewMap(parent.transform, "Test1", 5, 5);
			
			architect.RemoveLayerData(null);
		}

		
	}
}
