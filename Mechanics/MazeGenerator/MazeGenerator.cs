using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Mechanics.Internal
{
	public class MazeGenerator : PMonoBehaviour
	{
		public enum Modes
		{
			First,
			Last,
			Random
		}

		public enum Orientations
		{
			Right,
			Down,
			Left,
			Up
		}

		[SerializeField, Button]
		protected bool generate;

		public Point2 Size = Point2.One;
		public Modes Mode;
		public MazeChunk[] Chunks;
		public GameObject Wall;

		const int tryCount = 1000;
		static Orientations[] orientations =
		{
			Orientations.Right,
			Orientations.Down,
			Orientations.Left,
			Orientations.Up
		};
		GameObject maze;

		void Generate()
		{
			StopAllCoroutines();

			if (maze != null)
				maze.Destroy();

			StartCoroutine(GenerateRoutine());
		}

		IEnumerator GenerateRoutine()
		{
			maze = gameObject.AddChild(string.Format("Maze {0}", CachedTransform.childCount));
			var map = new MazeChunk[Size.X, Size.Y];
			var chunks = new List<MazeChunk>(Size.X * Size.Y);
			var initialPosition = GetInitialPosition(map);

			chunks.Add(CreateChunk(initialPosition, GetRandomValidOrientation(initialPosition, map), maze, map));

			while (chunks.Count > 0)
			{
				MazeChunk chunk;

				switch (Mode)
				{
					default:
						chunk = chunks.First();
						break;
					case Modes.Last:
						chunk = chunks.Last();
						break;
					case Modes.Random:
						chunk = chunks.GetRandom();
						break;
				}

				if (HasValidOrientation(chunk.Position, map))
				{
					chunks.Add(CreateChunk(chunk.Position, GetRandomValidOrientation(chunk.Position, map), maze, map));
					yield return null;
				}
				else
					chunks.Remove(chunk);
			}
		}

		public MazeChunk CreateChunk(Point2 position, Orientations orientation, GameObject maze, MazeChunk[,] map)
		{
			position += ToDirection(orientation);

			var chunk = Instantiate(Chunks.GetRandom());
			chunk.name = string.Format("{0}_{1}", position.X, position.Y);
			chunk.Position = position;
			chunk.Orientation = orientation;
			chunk.CachedTransform.parent = maze.transform;
			chunk.CachedTransform.localPosition = position;
			map[position.X, position.Y] = chunk;

			CreateWalls(chunk, map);

			return chunk;
		}

		public void CreateWalls(MazeChunk chunk, MazeChunk[,] map)
		{
			for (int i = 0; i < orientations.Length; i++)
			{
				var orientation = orientations[i];
				var adjustedPoint = chunk.Position + ToDirection(orientation);

				if (orientation == ToOpposite(chunk.Orientation))
					continue;

				if (!map.ContainsPoint(adjustedPoint) || map.Get(adjustedPoint) != null)
					CreateWall(chunk, orientation);
			}
		}

		public GameObject CreateWall(MazeChunk chunk, Orientations orientation)
		{
			var wall = Instantiate(Wall);
			wall.transform.parent = chunk.CachedTransform;
			wall.transform.localPosition = Vector3.zero;
			wall.transform.localRotation = ToRotation(orientation);

			return wall;
		}

		public bool HasValidOrientation(Point2 position, MazeChunk[,] map)
		{
			for (int i = 0; i < orientations.Length; i++)
			{
				var orientation = orientations[i];
				var adjustedPoint = position + ToDirection(orientation);

				if (map.ContainsPoint(adjustedPoint) && map.Get(adjustedPoint) == null)
					return true;
			}

			return false;
		}

		public Orientations GetRandomValidOrientation(Point2 position, MazeChunk[,] map)
		{
			orientations.Shuffle();

			for (int i = 0; i < orientations.Length; i++)
			{
				var orientation = orientations[i];
				var adjustedPoint = position + ToDirection(orientation);

				if (map.ContainsPoint(adjustedPoint) && map.Get(adjustedPoint) == null)
					return orientation;
			}

			return Orientations.Right;
		}

		public Point2 GetInitialPosition(MazeChunk[,] map)
		{
			return new Point2(PRandom.Range(0, map.GetUpperBound(0) - 1), PRandom.Range(0, map.GetUpperBound(1)));
		}

		public static Orientations ToOpposite(Orientations orientation)
		{
			switch (orientation)
			{
				default:
					return Orientations.Left;
				case Orientations.Down:
					return Orientations.Up;
				case Orientations.Left:
					return Orientations.Right;
				case Orientations.Up:
					return Orientations.Down;
			}
		}

		public static Point2 ToDirection(Orientations orientation)
		{
			switch (orientation)
			{
				default:
					return Point2.Right;
				case Orientations.Down:
					return Point2.Down;
				case Orientations.Left:
					return Point2.Left;
				case Orientations.Up:
					return Point2.Up;
			}
		}

		public static Quaternion ToRotation(Orientations orientation)
		{
			switch (orientation)
			{
				default:
					return Quaternion.identity;
				case Orientations.Down:
					return Quaternion.Euler(0f, 0f, 270f);
				case Orientations.Left:
					return Quaternion.Euler(0f, 0f, 180f);
				case Orientations.Up:
					return Quaternion.Euler(0f, 0f, 90f);
			}
		}
	}
}