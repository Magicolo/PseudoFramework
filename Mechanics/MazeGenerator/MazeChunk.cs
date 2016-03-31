using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Mechanics.Internal
{
	[Serializable]
	public class MazeChunk : PMonoBehaviour
	{
		public Point2 Position { get; set; }
		public MazeGenerator.Orientations Orientation { get; set; }
	}
}