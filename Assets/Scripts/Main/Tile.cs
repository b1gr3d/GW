using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class Tile: GridObject, Neighbour<Tile> {
    public bool Passable;
	public static int indexer = GameSystem.instance.size;

	public Tile(int x, int y)
		: base(x, y)
	{
		Passable = true;
	}
	
	public IEnumerable<Tile> AllNeighbours { get; set; }
	public IEnumerable<Tile> Neighbours
	{
		get { return AllNeighbours; }
	}
	
	public void FindNeighbours(Dictionary<Point, TileBehaviour> Board,
        Vector2 BoardSize, bool EqualLineLengths)
    {
        List<Tile> n = new List<Tile>();

        foreach (Point point in NeighbourShift)
        {
            int neighbourX = X + point.x;
            int neighbourY = Y + point.y;

            if (neighbourX >= BoardSize.x && neighbourX < BoardSize.y &&
                neighbourY >= BoardSize.x && neighbourY < BoardSize.y) {
                n.Add(Board[new Point(neighbourX, neighbourY)].tile);
            }
        }

        AllNeighbours = n;
    }

    public static List<Point> NeighbourShift {
        get {
            return new List<Point> {
				new Point(0, indexer),
				new Point(indexer, 0),
				new Point(0, -indexer),
				new Point(-indexer, 0),
            };
        }
    }
}