using UnityEngine;
using System.Collections;

public class IconScript : MonoBehaviour {
	private Vector3 currentPosition, nextPosition;
	[SerializeField] GameObject[] obj;
	[SerializeField] float max = 0.4f, min = 0.1f;
	GameObject gameSystem = null;
	// Use this for initialization
	void Start () {
		gameSystem = GameObject.Find ("GameSystem");
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnDrag() { 
		transform.position = Input.mousePosition;
		nextPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition.normalized);
		//Debug.Log ("DEBUG: nextPosition: " + nextPosition.ToString ());
		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.down, out hit)) {
			if(hit.collider.tag == "Ground") {
				//Debug.Log ("Detected Ground");
				//Debug.Log ("DEBUG: Node hit: " + hit.collider.transform.position.ToString());
			}
		}
	}

	public void OnMouseDown() {
		//Debug.Log ("OnMouseDown()");
		currentPosition = transform.position;
		//Debug.Log ("Debug: currentPosition : (" + Mathf.Round(currentPosition.x) + "," + Mathf.Round(currentPosition.y) + ")");
	}

	public void OnMouseUp() {
		transform.position = currentPosition;
		//Debug.Log ("DEBUG: nextPosition: " + nextPosition.ToString ());
		RaycastHit hit;
		if (Physics.Raycast (nextPosition, Vector3.down, out hit)) {
			if(hit.collider.tag == "Ground") {
				if(hit.collider.GetComponent<TileState>().getTileState() == TileState.TileSTATE.EMPTY) {
					if(gameSystem.GetComponent<GameSystem>().getGamePhase() == GameSystem.GAMEPHASE.PLACEMENT) {
						Vector3 pos = hit.collider.transform.position;
						Spawn (new Vector3(pos.x, Random.Range(min, max), pos.z));
						hit.collider.gameObject.GetComponent<TileState>().SetOccupied();
					}
				}
			}
		}
	}

	void Spawn(Vector3 pos) {
		GameObject ship = (GameObject) Instantiate (obj [0], pos, Quaternion.identity);
		gameSystem.GetComponent<GameSystem> ().AddShip (ship);
	}
}
