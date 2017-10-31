using System;
using UnityEngine;

public abstract class GridObject : MonoBehaviour {
	public Point Location;
	public int X { get { return (int) Location.x; }}
	public int Y { get { return (int) Location.y; }}

	public GridObject(Point location) {
		Location = location;
	}

	public GridObject(int x, int y): this(new Point(x,y)) {}
}