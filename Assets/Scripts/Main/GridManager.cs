using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class GridManager: MonoBehaviour {
	[SerializeField]
	public int TileX, TileY;
	public float width = 1f;
	public float height = 1f;
	
	[SerializeField]
	GameObject TileNode;
	public GameObject[,] Tile;
	List<GameObject> AiZone;

    //selectedTile stores the tile mouse cursor is hovering on
    public Tile selectedTile = null;
    //TB of the tile which is the start of the path
    public TileBehaviour originTileTB = null;
    //TB of the tile which is the end of the path
    public TileBehaviour destTileTB = null;
 
    public static GridManager instance = null;
    public Dictionary<Point, TileBehaviour> Board;
	List<GameObject> path;
	public static Hashtable gridMap;

	void Awake() {
        instance = this;

		Tile = new GameObject[TileX,TileY];
		AiZone = new List<GameObject> ();
		gridMap = new Hashtable ();
    }
    //some of the omitted code from the original goes here
    public void CreateTile() {
		float range = (TileX / 2) * GameSystem.instance.size;
        Vector2 gridSize = new Vector2(-range, range);
        Board = new Dictionary<Point, TileBehaviour>();
		for (int n = 0; n < TileY; n++) {
			for (int m = 0; m < TileX; m++) {
				float x = (m - (TileX/2)) * GameSystem.instance.size;
				float y = (n - (TileY/2)) * GameSystem.instance.size;
				GameObject TileObject = (GameObject) Instantiate(TileNode, new Vector3(x, 0, y), new Quaternion(1f, 0, 0, 1f));
				TileObject.transform.localScale = new Vector3(GameSystem.instance.size, GameSystem.instance.size, GameSystem.instance.size);
				//Debug.Log ("Created Node at: (" + TileObject.transform.position.x + "," + TileObject.transform.position.z + ")");
				if(n <= 2) {
					TileObject.GetComponent<TileState>().SetAiZone();
					AiZone.Add(TileObject);
				}
				else if(n >= TileY - 3) {
					TileObject.GetComponent<TileState>().SetPlayerZone();
				}
				Tile[m,n] = TileObject;

				// A* algorithm
                var tb = (TileBehaviour)TileObject.GetComponent("TileBehaviour");
				if(tb.tile == null) {
					Debug.Log ("null");
				}
				tb.tile.gameObject.AddComponent<Tile>();
				tb.tile.gameObject.GetComponent<Tile>().Location.x = (int)x;
				tb.tile.gameObject.GetComponent<Tile>().Location.y = (int)y;
				//Debug.Log ("tb.tile: " + tb.tile.Location.x + "," + tb.tile.Location.y);
				Board.Add(tb.tile.Location, tb);

				//Adding to Hashtable for quick look up for AI computation.
				string tilePos = tb.tile.Location.x + "," + tb.tile.Location.y;
				gridMap[tilePos] = TileObject;
			}
		}
		foreach(TileBehaviour tb in Board.Values)
			tb.tile.FindNeighbours(Board, gridSize, true);
    }

    public void generatePath(Queue<String> actionList)
    {
        //Don't do anything if origin or destination is not defined yet
        if (originTileTB == null || destTileTB == null)
        {
			getPath(new List<Tile>(), actionList);
            return;
        }
        //We assume that the distance between any two adjacent tiles is 1
        //If you want to have some mountains, rivers, dirt roads or something else which might slow down the player you should replace the function with something that suits better your needs
        //Func<Tile, Tile, double> distance = (node1, node2) => 1;
        var path = PathFinder.FindPath(originTileTB.tile, destTileTB.tile);
		getPath(path, actionList);
    }

    private void getPath(IEnumerable<Tile> path, Queue<String> actionList)
    {
		if (path == null) {
			return;
		}
		foreach (Tile tile in path)
        {
            //calcWorldCoord method uses squiggly axis coordinates so we add y / 2 to convert x coordinate from straight axis coordinate system
            Vector2 gridPos = new Vector2(tile.Location.x, tile.Location.y);
			actionList.Enqueue("MOV " + gridPos.x + " " + gridPos.y);
        }
    }

	public GameObject[,] getTile() {
		return Tile;
	}
	
	public List<GameObject> getAiZone() {
		return AiZone;
	}
	
	public int getY() {
		return TileY;
	}
	
	public int getX() {
		return TileX;
	}

	public void setStartTile(TileBehaviour tb) {
		GridManager.instance.originTileTB = tb;
	}
	
	public void setDestinationTile(TileBehaviour tb) {
		GridManager.instance.destTileTB = tb;
	}
}