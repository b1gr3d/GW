using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planner : MonoBehaviour {
	int[] dx = {-2, -1, 0, 0, 2, 1, -2, -1, 0, 0, 2, 1};
	int[] dy = {-2, -1, -2, -1, -2, -1, 2, 1, 2, 1, 2, 1};

	private Queue<string> actionList;

	// Use this for initialization
	void Start () {
		actionList = new Queue<string> ();
	}

	public string getTarget(int target) {
		string t = null;
		if (target >= 0 && target <= 60) {
			t = "low";
		} else if (target > 60 && target <= 75) {
			t = "heal";
		} else if (target > 75 && target <= 85) {
			t = "highest";
		} else if (target > 85 && target <= 95) {
			t = "close";
		} else {
			t = "random";
		}
		return t;
	}

	public GameObject getTarget() {
		GameObject target = null;
		int i = Random.Range (0, 100);
		if (GetComponent<Analyzer> ().getAiTargetCount () == 0) {
			if (i >= 0 && i <= 50) {
				target = GetComponent<Analyzer>().getRandom();
			} else {
				target = GetComponent<Analyzer>().getClosest();
			}
		} else {
			string c = getTarget(i); 
			switch(c) {
				case "low":
					Debug.Log ("Lowest health targeted");
					GameObject lh = GetComponent<Analyzer>().getLowestHealth();
					GameObject ls = GetComponent<Analyzer>().getLowestShield();
					int hp = lh.GetComponentInParent<PlayerShips>().getHealth();
					int sp = ls.GetComponentInParent<PlayerShips>().getShield();
					if(hp <= sp) {
						target = lh;
					}
					else {
						target = ls;
					}
					break;
				case "heal":
				Debug.Log ("Healer targeted");
					List<GameObject> healers = GetComponent<Analyzer>().getHealers();
					if(healers.Count > 0) 
						target = healers[Random.Range(0, healers.Count)];
					else 
						target = GetComponent<Analyzer>().getClosest();
					break;
				case "highest":
				Debug.Log ("Highest Damage targeted");
					target = GetComponent<Analyzer>().getHighestDamage();
					break;
				case "close":
				Debug.Log ("Closest targeted");
				    target = GetComponent<Analyzer>().getClosest();
				    break;
				default:
				Debug.Log ("Random targeted");
				    target = GetComponent<Analyzer>().getRandom();
					break;
			}
		}
		return target;
	}

	public void getLocation() {
		//Clear ();
		GameObject ship = GetComponentInParent<EnemyShips> ().getTargetShip ();
		GameObject s1 = getNextLocation (ship.transform.position);
		TileBehaviour t1 = s1.GetComponent<TileBehaviour> ();
		GetComponent<Executer> ().checkTile ();
		TileBehaviour t2 = GetComponent<Executer> ().TileObject.GetComponent<TileBehaviour> ();
		GridManager.instance.setStartTile (t1);
		GridManager.instance.setDestinationTile (t2);
		GridManager.instance.generatePath(actionList);
	}
	
	public GameObject getNextLocation(Vector3 p1) {
		GameObject tile = null;
		do {
			int index = Random.Range (0, 12);
			int x = dx [index];
			int y = dy [index];
			Vector2 tilePos = new Vector2 (p1.x + x, p1.z + y);
			if(GridManager.gridMap.Contains(tilePos.x + "," + tilePos.y)) {
				tile = (GameObject) GridManager.gridMap[tilePos.x + "," + tilePos.y];
			}
		} while(tile == null || tile.GetComponent<TileState>().getTileState() == TileState.TileSTATE.OCCUPIED);

		return tile;
	}

	public void getAttackType() {
		//Random Attack
		addAtk ("basic");
	}

	public void addMov(string loc) {
		actionList.Enqueue("MOV " + loc);
	}

	public void addAtk(string action) {
		actionList.Enqueue ("ATK " + action);
	}

	public void removeAction() {
		actionList.Dequeue ();
	}

	public Queue<string> getActionList() {
		return actionList;
	}

	public void Clear() {
		actionList.Clear ();
	}
}
