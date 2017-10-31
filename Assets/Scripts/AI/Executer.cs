using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Executer : UtilityParser {
	GameObject gameSystem = null;
	public GameObject TileObject = null;

	Vector3 nextPos;
	bool isNextPos = false;
	[SerializeField] int speed = 10;

	bool isActive = false;
	public bool moved = false;

	int movement = 0;

	Queue<string> actionList;
	// Use this for initialization
	void Start () {
		gameSystem = GameObject.Find ("GameSystem");
		nextPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if ((transform.position - nextPos).magnitude > 0.04 && gameSystem.GetComponent<GameSystem> ().getGamePhase () == GameSystem.GAMEPHASE.AI) {
			if (isNextPos) {
				Vector3 a = nextPos - transform.position;
				transform.Translate (a * Time.deltaTime * speed);
			}
		}
		else if ((transform.position - nextPos).magnitude <= 0.04
		         && gameSystem.GetComponent<GameSystem> ().getGamePhase () == GameSystem.GAMEPHASE.AI) {
			if(isNextPos) {
				transform.position = nextPos;
				checkTile();
				if(TileObject.GetComponent<TileState>().getTileState() == TileState.TileSTATE.EMPTY)
					TileObject.GetComponent<TileState> ().SetOccupied ();
			}
			isNextPos = false;
			if(isActive)
				Execute();
		}
	}

	public void Execute() {
		if (actionList != null)
			Debug.Log ("ActionList.Count: " + actionList.Count);
		if (actionList != null && actionList.Count > 0) {
			string[] tokens = parse (actionList.Dequeue ());
			Perform (tokens);
		} else {
			isActive = false;
			AiController.instance.callAi();
		}
	}

	public void Target() {
		GameObject targetShip = GetComponent<Planner> ().getTarget ();
		GetComponent<EnemyShips> ().setTargetShip (targetShip);
	}

	public void MoveToTarget() {
		Target ();
		getLocation();
		setIsActive(true);
		Execute();
	}

	public void Perform(string[] actionTokens) {
		string action = actionTokens [0];
		switch (action) {
			case "MOV":
				Debug.Log ("MOVING: " + actionTokens[1] + "," + actionTokens[2]);
				Debug.Log ("Movement: " + movement);
				if (AiController.instance.getMovement () > 0 && movement > 0) {
					checkTile ();
					TileObject.GetComponent<TileState>().SetDefault();
					AiController.instance.decreaseMovement (1);
					movement--;
					Move (actionTokens, out nextPos);
				}
				break;
			case "ATK":
				//Check Attack Type
				Debug.Log ("Attacking: " + actionTokens[1]);
				if(actionTokens[1] == "basic") {
					GameObject target = GetComponent<EnemyShips>().getTargetShip();
					int damage = GetComponent<EnemyShips>().Attack(target);
					target.GetComponentInParent<ScoutShip>().decreaseHealth(damage);
				}
				break;
			default:
				Debug.Log ("OTHER");
				break;
		}
	}

	public void getLocation() {
		GetComponent<Planner> ().getLocation ();
		actionList = GetComponent<Planner> ().getActionList ();
	}

	public void Attack() {
		GetComponent<Planner> ().getAttackType ();
		actionList = GetComponent<Planner> ().getActionList ();
		Execute ();
	}

	public void Move(string[] actionTokens, out Vector3 pos) {
		int x, y; 
		int.TryParse(actionTokens[1], out x);
		int.TryParse(actionTokens[2], out y);
		Vector2 currentPos = new Vector2 (this.transform.position.x, this.transform.position.z);
		Vector2 destination = new Vector2 ((float)x, (float)y);
		Vector2 pathVector = destination - currentPos;
		pos = transform.position + new Vector3(pathVector.x, 0, pathVector.y);
		isNextPos = true;
	}

	public void checkTile() {
		RaycastHit hit;
		Ray ray = new Ray (transform.position, Vector3.down);
		
		if (Physics.Raycast (ray, out hit, 10f)) {
			if (hit.collider.tag == "Ground") {
				TileObject = hit.collider.gameObject;
			}
		}
	}

	public void setIsActive(bool status) {
		isActive = status;
	}

	public void setMovement(int val) {
		movement = val;
	}
}
