using UnityEngine;
using System;
using NUnit.Framework;
using NSubstitute;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Tests.ArchitectTests
{
	public class NewMap
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
		public void _01_OnNewMaptheMapIsEmpty()
		{
			architect.CreateNewMap(null, "Test1", 5, 5);
			Assert.IsEmpty(architect.MapData.Layers);
		}

		[Test]
		public void _02_Adding1Layer_NewMapContains1Layer()
		{
			architect.CreateNewMap(null, "Test1", 5, 5);
			architect.AddLayerData("bob");
			Assert.IsNotEmpty(architect.MapData.Layers);
			Assert.AreEqual(1, architect.MapData.Layers.Count);
		}

		[Test]
		public void _03_Adding1Layer_NewMapRootTransformContains1Layer()
		{
			GameObject parent = new GameObject("Test");
			architect.CreateNewMap(parent.transform, "Test1", 5, 5);
			architect.AddLayerData("bob");
			Assert.AreEqual(1, parent.transform.childCount, "There should be only 1 gameobject.");
		}

		[Test]
		public void _04_NewMapRemoveOldLayers()
		{
			architect.CreateNewMap(null, "Test1", 5, 5);
			architect.AddLayerData("bob");

			architect.CreateNewMap(null, "Test2", 5, 5);
			Assert.IsEmpty(architect.MapData.Layers);
		}

		[Test]
		public void _05_NewMapRemoveOldMapGameObjects()
		{
			GameObject parent = new GameObject("Test");
			architect.CreateNewMap(parent.transform, "Test1", 5, 5);
			architect.AddLayerData("bob");

			GameObject parent2 = new GameObject("Test2");
			architect.CreateNewMap(parent2.transform, "Test2", 5, 5);
			Assert.IsTrue(null == parent);
		}

		[Test]
		public void _10_Removing1Layer_LayerIsRemoved()
		{
			GameObject parent = new GameObject("Test");
			architect.CreateNewMap(parent.transform, "TestRemove", 5, 5);
			
			LayerData removeMe = architect.AddLayerData("remove");
			architect.RemoveLayerData(removeMe);
			
			Assert.AreEqual(0, parent.transform.childCount, "There should be 0 gameobject after the remove.");
		}

		[Test]
		public void _11_RemovingNullLayer()
		{
			GameObject parent = new GameObject("Test");
			architect.CreateNewMap(parent.transform, "Test1", 5, 5);
			
			architect.RemoveLayerData(null);
		}

		
	}
}
