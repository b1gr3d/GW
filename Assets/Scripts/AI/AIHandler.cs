using UnityEngine;
using System.Collections;

public class AIHandler : MonoBehaviour {
	GameObject gameSystem = null;
	GameObject TileObject = null;

	//Vector3 nextPos;
	bool isNextPos = false;//, isReady = false;
	[SerializeField] bool isInRange = false;

	void Start () {
		gameSystem = GameObject.Find ("GameSystem");
		StartCoroutine ("UpdateMove");
		//nextPos = transform.position;
	}

	void Update () {

	}

	// Update is called once per frame
	public IEnumerator UpdateMove () {
		if (!isInRange) {
			GameSystem.GAMEPHASE phase = gameSystem.GetComponent<GameSystem> ().getGamePhase ();
			if (phase == GameSystem.GAMEPHASE.AI || phase == GameSystem.GAMEPHASE.PLACEMENT) {
				if(phase == GameSystem.GAMEPHASE.AI) {
					yield return new WaitForSeconds(1);
					//nextPos = getNextPosition((int)transform.position.x, (int)transform.position.z);
					//isReady = true;
					isNextPos = true;
					TileObject.GetComponent<TileState> ().SetDefault ();
					//Debug.Log("Moving: " + transform.position.ToString() + " ==> " + nextPos.ToString());
				}
				
				if(!isNextPos) {
					checkTile();
				}
			}
		}
	}

	public void getPosition() {
		StartCoroutine ("UpdateMove");
	}
	

	bool checkBounds(int x, int y) {
		bool isBounds = false;
		if (x <= GridManager.instance.TileX/2
		    && x >= -GridManager.instance.TileX/2) {
			if (y <= GridManager.instance.TileY/2
			    && y >= -GridManager.instance.TileY/2) {
				isBounds = true;
			}
		}
		return isBounds;
	}

	public bool getIsNextPos() {
		return isNextPos;
	}

	public void checkTile() {
		RaycastHit hit;
		Ray ray = new Ray (transform.position, Vector3.down);
		
		if (Physics.Raycast (ray, out hit, 10f)) {
			if (hit.collider.tag == "Ground") {
				TileObject = hit.collider.gameObject;
				TileObject.GetComponent<TileState> ().SetOccupied ();
			}
		}
	}

	public bool getIsRanged() {
		return isInRange;
	}

	public void setIsRanged(bool status) {
		isInRange = status;
	}
}
