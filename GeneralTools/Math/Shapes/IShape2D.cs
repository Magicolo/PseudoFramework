using UnityEngine;

namespace Pseudo
{
	public interface IShape2D
	{
		float XMin { get; }
		float XMax { get; }
		float YMin { get; }
		float YMax { get; }
		Vector2 Top { get; }
		Vector2 Bottom { get; }
		Vector2 Left { get; }
		Vector2 Right { get; }
		Rect Bounds { get; }
		Vector2 Position { get; set; }

		bool Contains(Vector2 point);
		bool Contains(Rect rect);
		bool Contains(Circle circle);
		bool Contains(Disk disk);
		bool IsContained(Rect rect);
		bool IsContained(Circle circle);
		bool IsContained(Disk disk);
		bool Overlaps(Rect rect);
		bool Overlaps(Circle circle);
		bool Overlaps(Disk disk);
		Vector2 GetRandomPoint();
	}
}