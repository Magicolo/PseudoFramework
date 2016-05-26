using UnityEngine;
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Architect.Tests
{
	[Serializable]
	public class ArchitectBrushTest
	{

		[SetUp]
		public void Setup()
		{
			//architect = new ArchitectControler();
		}

		[TearDown]
		public void TearDown()
		{
			//architect = null;
		}

		[Test]
		public void bob()
		{
			LayerData layer = new LayerData(null, "Layer", 2, 2);
			ArchitectTilePositionGetter tilePosition = new ArchitectTilePositionGetter(Vector3.zero, layer);
			TileType type = new TileType(5);
			ArchitectRotationFlip rotation = new ArchitectRotationFlip(90, false, false);
			BrushCommand brush = new BrushCommand(tilePosition, type, rotation);
			bool done = brush.Do();
			Debug.Log(done);
			Assert.IsTrue(done);
		}
	}
}
