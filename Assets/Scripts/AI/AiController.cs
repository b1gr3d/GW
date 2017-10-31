using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiController : MonoBehaviour {
	public static AiController instance = null;
	List<GameObject> AiZone;
	[SerializeField] GameObject AiShip;
	List<GameObject> AiFleet = new List<GameObject> ();
	[SerializeField]
	int movement, maxMovement = 0;

	// Use this for initialization
	void Start () {
		instance = this;
		movement = GameSystem.instance.getMovement ();
		maxMovement = GameSystem.instance.getMovement ();
	}

	public void placeAiShip() {
		AiZone = GridManager.instance.getAiZone ();
		Vector3 pos = getTilePos ();
		GameObject ship = (GameObject) Instantiate (AiShip, new Vector3 (pos.x, 0.3f, pos.z), Quaternion.identity); 
		ship.GetComponent<AIHandler> ().checkTile ();
		AiFleet.Add (ship);
	}

	public Vector3 getTilePos() {
		Vector3 pos = new Vector3();
		int size = AiZone.Count;
		GameObject Tile = AiZone [Random.Range (0, size)];
		AiZone = GridManager.instance.getAiZone ();
		if (Tile.GetComponent<TileState> ().getTileState () == TileState.TileSTATE.EMPTY) {
			pos = Tile.transform.position;
		} else {
			pos = getTilePos();
		}

		return pos;
	}

	public List<GameObject> getAiFleet() {
		return AiFleet;
	}

	public int getMovement() {
		return movement;
	}
	
	public void decreaseMovement(short value) {
		if ((movement - value) >= 0)
			movement -= value;
		else {
			movement = 0;
		}
	}
	
	public void setMovement(short value) {
		movement = value;
	}

	public void Reset() {
		movement = maxMovement;
		foreach (GameObject ai in AiFleet) {
			ai.GetComponent<Executer>().moved = false;
		}
	}

	public void callAi() {
		StartCoroutine("AiAction");
	}
	
	private IEnumerator AiAction () {
		int movement = GetComponent<AiController> ().getMovement ();
		if (movement > 0 && AiFleet.Count > 0 && CheckAiMoved()) {
			yield return new WaitForSeconds(0);
			int index = Random.Range(0, AiFleet.Count);
			GameObject AiShip = AiFleet[index];
			if(!AiShip.GetComponent<Executer>().moved) {
				AiShip.GetComponent<Executer>().moved = true;
				AiShip.GetComponent<Executer>().setMovement((int)maxMovement/AiFleet.Count);
				AiShip.GetComponent<Executer>().MoveToTarget();
			}
			else {
				StartCoroutine("AiAction");
			}
		} else {
			StartCoroutine("AiAttack");
		}
	}
	
	private IEnumerator AiAttack() {
		yield return new WaitForSeconds(1);
		foreach (GameObject AiShip in AiFleet) {
			if(AiShip.GetComponent<EnemyShips>().getTargetShip() != null)
				AiShip.GetComponent<Executer>().Attack();
		}
		GetComponent<GameSystem>().onGetNextPhase();
	}

	public void RemoveAiShip(GameObject ship) {
		AiFleet.Remove(ship);
	}

	public bool CheckAiMoved() {
		bool moved = false;

		foreach (GameObject ai in AiFleet) {
			if(ai.GetComponent<Executer>().moved == false) {
				moved = true;
			}
		}

		return moved;
	}
}
