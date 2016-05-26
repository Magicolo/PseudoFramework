using UnityEngine;
using System;
using NUnit.Framework;
using NSubstitute;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Architect.Tests
{
	public class ArchitectLayerDataTests
	{

		[SetUp]
		public void Setup()
		{
		}

		[TearDown]
		public void TearDown()
		{
		}

		[Test]
		public void CanAddTileTypeWithoutPrefab()
		{
			LayerData ld = new LayerData(null, "SuperLayer", 1, 1);
			TileType type = new TileType(1);
			Point2 position = new Point2(0, 0);

			bool result = ld.AddTile(position, type);

			Assert.IsTrue(result);
			Assert.IsNotNull(ld[position]);
		}

		[Test]
		public void CanAddTileTypeWithPrefab()
		{
			LayerData ld = new LayerData(null, "SuperLayer", 1, 1);
			GameObject go = new GameObject("TestGo");
			TileType type = new TileType(1,go);
			Point2 position = new Point2(0, 0);

			bool result = ld.AddTile(position, type);

			Assert.IsTrue(result);
			Assert.IsNotNull(ld[position]);
			Assert.IsNotNull(ld[position].GameObject);

		}
	}
}
