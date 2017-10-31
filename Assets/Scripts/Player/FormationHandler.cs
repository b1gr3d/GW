using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormationHandler : MonoBehaviour {
	GameObject[,] Tile;
	GameObject gameSystem = null;
	// Use this for initialization
	void Start () {
		gameSystem = GameObject.Find ("GameSystem");
		Tile = gameSystem.GetComponent<GameSystem> ().getTile ();
	}

	void OnMouseUp() {
		if (gameSystem.GetComponent<GameSystem> ().getGamePhase () == GameSystem.GAMEPHASE.MOVEMENT) {
			List<GameObject> fleet = GameSystem.instance.fleet;
			foreach(GameObject ship in fleet) {
				ship.GetComponentInChildren<InputMoveHandler>().resetMarker();
			}
			checkFormation();
		} 
	}

	void checkFormation() {
		if(Tile == null) {
			Tile = gameSystem.GetComponent<GameSystem> ().getTile ();
		}
		int x = (int) transform.position.x/GameSystem.instance.size;
		int y = (int) transform.position.z/GameSystem.instance.size;
		int TileX = GridManager.instance.getX ();
		int TileY = GridManager.instance.getY ();

		int dX = (x + (TileX / 2));
		int dY = (y + (TileY / 2)) ;

		for(int i = 0; i < TileX; i++) {
			if(Tile[i,dY].GetComponent<TileState>().getTileState() == TileState.TileSTATE.OCCUPIED) {
				if (Tile[i, dY].GetComponent<TileState>().checkShip("Player")) {
					GameObject playerShip = Tile[i, dY].GetComponent<TileState>().getShip("Player");
					if(playerShip != null) {
						playerShip.GetComponentInChildren<InputMoveHandler>().setFormationUI(transform.position);
					}
				}
			}
		}

		for(int i = 0; i < TileY; i++) {
			if(Tile[dX,i].GetComponent<TileState>().getTileState() == TileState.TileSTATE.OCCUPIED) {
				if (Tile[dX, i].GetComponent<TileState>().checkShip("Player")) {
					GameObject playerShip = Tile[dX, i].GetComponent<TileState>().getShip("Player");
					if(playerShip != null) {
						playerShip.GetComponentInChildren<InputMoveHandler>().setFormationUI(transform.position);
					}
				}
			}
		}
	}
}
