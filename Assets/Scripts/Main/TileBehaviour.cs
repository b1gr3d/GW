using UnityEngine;
using System.Collections;
 
public class TileBehaviour: MonoBehaviour {
    public Tile tile;
	GameObject gameSystem;
	GameObject[] ShipFleet;
	GameObject ship = null;
	
	[SerializeField] float max = 0.4f, min = 0.1f;
	
	[SerializeField]
	GameObject fxObject;
	
	// Use this for initialization
	void Start () {
		gameSystem = GameObject.Find ("GameSystem");
		ShipFleet = GameObject.Find ("ShipFleet").GetComponent<Fleet>().getFleet();
		
	}
	
	void OnMouseEnter() {
		if (isPlacementPhase()) {
			if(GetComponent<TileState>().getDeploymentZone() == TileState.TileZONE.PLAYER 
			   && GetComponent<TileState>().getTileState() == TileState.TileSTATE.EMPTY) {
				fxObject.SetActive(true);
			}
		}
	}
	
	void OnMouseExit() {
		if (fxObject.activeSelf) {
			fxObject.SetActive(false);
		}
	}
	
	public void OnMouseUp() {
		if (isPlacementPhase()) {
			if(GetComponent<TileState>().getDeploymentZone() == TileState.TileZONE.PLAYER 
			   && GetComponent<TileState>().getTileState() == TileState.TileSTATE.EMPTY) {
				Vector3 pos = this.transform.position;
				Spawn (new Vector3(pos.x, Random.Range(min, max), pos.z));
				if(ship != null) {
					ship.GetComponent<MeshCollider>().enabled = true;
					ship.GetComponentInChildren<InputMoveHandler>().checkPosition();
				}
			}
		}
	}
	
	public void OnMouseOver() {
		if (isPlacementPhase()) {
			
		}
	}
	
	void Spawn(Vector3 pos) {
		if(GameSystem.instance.count > 0) {
			GameSystem.instance.count--;
			ship = (GameObject) Instantiate (ShipFleet [0], pos, Quaternion.identity);
			gameSystem.GetComponent<GameSystem> ().AddShip (ship);
			fxObject.SetActive(false);
		}
		
	}
	
	public bool isPlacementPhase() {
		return (GameSystem.instance.getGamePhase () == GameSystem.GAMEPHASE.PLACEMENT && GameSystem.instance.count > 0);
	}

	public void setStartTile() {
		GridManager.instance.selectedTile = tile;
	}

	public void setDestinationTile() {

	}
}