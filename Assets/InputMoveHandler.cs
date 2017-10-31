using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputMoveHandler : MonoBehaviour {
	private Vector3 currentPosition, nextPosition;
	GameObject TileObject = null;

	List<GameObject> TileList = null;
	LineRenderer targetLine = null;
	
	private int tempMovement = 0;

	// Use this for initialization
	void Start () {
		currentPosition = transform.position;
		nextPosition = new Vector3 (0, 0, 0);
		TileList = new List<GameObject> ();
		targetLine = GetComponentInChildren<LineRenderer> ();
		targetLine.enabled = false;
		targetLine.SetPosition (0, new Vector3 (transform.position.x, 0.03f, transform.position.z));
		targetLine.SetPosition (1, new Vector3 (transform.position.x, 0.03f, transform.position.z));
	}

	void OnMouseDown() {
		currentPosition = transform.position;
		tempMovement = GameSystem.instance.getMovement ();
		//targetLine.SetPosition (0, new Vector3(transform.position.x, 0.03f, transform.position.z));
		
		if (GameSystem.instance.getGamePhase () == GameSystem.GAMEPHASE.MOVEMENT) {
			if(tempMovement >= 0) {
				transform.position = new Vector3( transform.position.x, transform.position.y + 0.1f, transform.position.z );
				RaycastHit hit;
				Ray ray = new Ray (transform.position, Vector3.down);
				
				if (Physics.Raycast (ray, out hit, 10f)) {
					if (hit.collider.tag == "Ground") {
						TileObject = hit.collider.gameObject;
						TileObject.GetComponent<TileState> ().SetDefault ();
						TileObject.GetComponent<TileBehaviour>().tile.Passable = true;
					}
				}
			}
		} 
	}

	void OnMouseDrag()
	{
		if (GameSystem.instance.getGamePhase () == GameSystem.GAMEPHASE.MOVEMENT) {
			if(tempMovement >= 0) {
				float distance_to_screen = Camera.main.WorldToScreenPoint (gameObject.transform.position).z;
				Vector3 pos_move = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
				transform.position = new Vector3 (Mathf.Round (pos_move.x/GameSystem.instance.size) * GameSystem.instance.size, transform.position.y, Mathf.Round (pos_move.z/GameSystem.instance.size) * GameSystem.instance.size);

				targetLine.SetPosition (0, new Vector3 (transform.position.x, 0.03f, transform.position.z));
				targetLine.SetPosition (1, new Vector3 (transform.position.x, 0.03f, (float)(transform.position.z - 3.5)));
				
				RaycastHit hit;
				Ray ray = new Ray (transform.position, Vector3.down);
				if (Physics.Raycast (ray, out hit, 10f)) {
					if (hit.collider.tag == "Ground") {
						if (hit.collider.GetComponent<TileState> ().getTileState () == TileState.TileSTATE.EMPTY
						    || hit.collider.GetComponent<TileState> ().getTileState () == TileState.TileSTATE.HIGHLIGHT) {
							if (hit.collider.GetComponent<TileState> ().getTileState () == TileState.TileSTATE.EMPTY) {
								TileList.Add (hit.collider.gameObject);
								hit.collider.gameObject.GetComponent<TileState> ().setHightLight ();
								tempMovement--;
							}
							nextPosition = new Vector3 (Mathf.Round (pos_move.x/GameSystem.instance.size) * GameSystem.instance.size, transform.position.y, Mathf.Round (pos_move.z/GameSystem.instance.size) * GameSystem.instance.size);
						} else if (hit.collider.GetComponent<TileState> ().getTileState () == TileState.TileSTATE.OCCUPIED) {
							transform.position = nextPosition;
						}
					}
				} else {
					transform.position = nextPosition;
				}
			}
		}
	}

	void OnMouseUp() {
		// Unhighlight Tiles
		for (int i = 0; i < TileList.Count; i++) {
			TileList[i].GetComponent<TileState>().SetDefault();
		}
		if (TileList.Count >= 1) {
			GameSystem.instance.decreaseMovement ((short)(TileList.Count-1));
		}
		TileList.Clear ();
		
		if (GameSystem.instance.getGamePhase () == GameSystem.GAMEPHASE.MOVEMENT) {
			if (GameSystem.instance.getMovement () >= 0) {
				RaycastHit hit;
				Ray ray = new Ray (transform.position, Vector3.down);
				
				if (Physics.Raycast (ray, out hit, 10f)) {
					if (hit.collider.tag == "Ground") {
						if (hit.collider.GetComponent<TileState> ().getTileState () == TileState.TileSTATE.EMPTY 
						    || hit.collider.GetComponent<TileState> ().getTileState () == TileState.TileSTATE.HIGHLIGHT) {
							transform.position = nextPosition;
							hit.collider.GetComponent<TileState> ().SetOccupied ();
							hit.collider.GetComponent<TileBehaviour>().tile.Passable = false;
						}
					}
				} else {
					transform.position = currentPosition;
					TileObject.GetComponent<TileState> ().SetOccupied ();
					TileObject.GetComponent<TileBehaviour>().tile.Passable = false;
				}
				transform.position = new Vector3 (Mathf.Round (transform.position.x), transform.position.y - 0.1f, Mathf.Round (transform.position.z));
			}
		} else if (GameSystem.instance.getGamePhase () == GameSystem.GAMEPHASE.ATTACK) {
			RaycastHit hit;
			Ray ray = new Ray (transform.position, Vector3.down);
			
			if (Physics.Raycast (ray, out hit, 10f)) {
				if (hit.collider.tag == "Ground") {
					TileObject = hit.collider.gameObject;
				}
			}
			
			GameSystem.instance.setActiveShip(this.gameObject);
		}
		targetLine.enabled = false;
	}

	void OnMouseOver() {
		if (Input.GetMouseButtonDown(2)) {
			Camera.main.GetComponent<CameraScript> ().resetLookAt();
			Camera.main.GetComponent<CameraScript> ().setLookAt (this.gameObject.transform.position);
			Camera.main.transform.LookAt (transform.position);
		}
	}
	
	public void setTarget(Vector3 pos, bool selected) {
		targetLine.enabled = selected;
		targetLine.SetPosition(1, new Vector3 (pos.x, 0.03f, pos.z));
	}
	
	public void setTargetUI(bool status) {
		if (TileObject != null) {
			TileObject.GetComponent<TileUI> ().SetActiveUI (status);
		}
	}
	
	public void setFormationUI(Vector3 pos) {
		targetLine.enabled = true;
		targetLine.SetPosition (1, new Vector3 (pos.x, 0.03f, (float)(pos.z)));
	}
	
	public GameObject getTileObject() {
		GameObject Tile = null;
		RaycastHit hit;
		Ray ray = new Ray (transform.position, Vector3.down);
		if (Physics.Raycast (ray, out hit, 10f)) {
			if (hit.collider.tag == "Ground") {
				Tile = hit.collider.gameObject;
			}
		}
		return Tile;
	}


	public void checkPosition() {
		RaycastHit hit;
		Ray ray = new Ray (transform.position, Vector3.down);
		
		if (Physics.Raycast (ray, out hit, 10f)) {
			if (hit.collider.tag == "Ground") {
				if (hit.collider.GetComponent<TileState> ().getTileState () == TileState.TileSTATE.EMPTY) {
					hit.collider.GetComponent<TileState> ().SetOccupied ();
					hit.collider.GetComponent<TileBehaviour>().tile.Passable = false;
				}
			}
		}
	}

	public void resetMarker() {
		targetLine.SetPosition (0, new Vector3 (transform.position.x, 0.03f, transform.position.z));
		targetLine.SetPosition (1, new Vector3 (transform.position.x, 0.03f, transform.position.z));
	}
}
