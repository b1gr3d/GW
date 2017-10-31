using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Analyzer : MonoBehaviour {
	GameObject[,] Tile;
	GameObject gameSystem = null;

	// Use this for initialization
	void Start () {
		gameSystem = GameObject.Find ("GameSystem");
		Tile = gameSystem.GetComponent<GameSystem> ().getTile ();
	}
	
	public GameObject getHighestDamage() {
		GameObject highestDamageShip = null;
		List<GameObject> pFleet = getPFleet ();

		highestDamageShip = pFleet [0];
		foreach (GameObject pShip in pFleet) {
			if(pShip.GetComponentInParent<PlayerShips>().totalDamage > highestDamageShip.GetComponentInParent<PlayerShips>().totalDamage) {
				highestDamageShip = pShip;
			}
		}
		return highestDamageShip;
	}

	public List<GameObject> getHealers() {
		List<GameObject> healers = new List<GameObject> ();
		List<GameObject> pFleet = getPFleet ();
		foreach (GameObject pShip in pFleet) {
			if(pShip.GetComponentInParent<PlayerShips>().getShipStatus() == PlayerShips.ACTION.HEALING) {
				healers.Add(pShip);
			}
		}
		return healers;
	}

	public GameObject getLowestHealth() {
		GameObject lowestHealth = null;
		List<GameObject> pFleet = getPFleet ();
		
		lowestHealth = pFleet [0];
		foreach (GameObject pShip in pFleet) {
			if(pShip.GetComponentInParent<PlayerShips>().getHealth() < lowestHealth.GetComponentInParent<PlayerShips>().getHealth()) {
				lowestHealth = pShip;
			}
		}
		return lowestHealth;
	}

	public GameObject getLowestShield() {
		GameObject lowest = null;
		List<GameObject> pFleet = getPFleet ();
		
		lowest = pFleet [0];
		foreach (GameObject pShip in pFleet) {
			if(pShip.GetComponentInParent<PlayerShips>().getShield() < lowest.GetComponentInParent<PlayerShips>().getShield()) {
				lowest = pShip;
			}
		}
		return lowest;
	}

	public GameObject getRandom() {
		GameObject random = null;
		List<GameObject> pFleet = getPFleet ();
		random = pFleet [Random.Range (0, pFleet.Count)];
		return random;
	}

	public GameObject getClosest() {
		GameObject closest = null;
		List<GameObject> pFleet = getPFleet ();

		float distance = int.MaxValue;
		foreach (GameObject ship in pFleet) {
			float d = Vector3.Distance(transform.position, ship.transform.position);
			if(distance > d) {
				distance = d;
				closest = ship;
			}
		}
		return closest;
	}

	public GameObject getFurthest() {
		GameObject furthest = null;
		List<GameObject> pFleet = getPFleet ();
		
		float distance = 0;
		foreach (GameObject ship in pFleet) {
			float d = Vector3.Distance(transform.position, ship.transform.position);
			if(distance < d) {
				distance = d;
				furthest = ship;
			}
		}
		return furthest;
	}

	public List<GameObject> getPFleet() {
		List<GameObject> pFleet = new List<GameObject> ();
		if (Tile == null) {
			Tile = gameSystem.GetComponent<GameSystem> ().getTile ();
		}

		int TileX = GridManager.instance.getX ();
		int TileY = GridManager.instance.getY ();

		for (int y = 0; y < TileY; y++) {
			for(int x = 0; x < TileX; x++) {
				if(Tile[x,y].GetComponent<TileState>().checkShip("Player")) {
					pFleet.Add(Tile[x,y].GetComponent<TileState>().getShip("Player"));
				}
			}
		}
		return pFleet;
	}

	public List<GameObject> getAiFleet() {
		List<GameObject> aiFleet = new List<GameObject> ();
		if (Tile == null) {
			Tile = gameSystem.GetComponent<GameSystem> ().getTile ();
		}
		
		int TileX = GridManager.instance.getX ();
		int TileY = GridManager.instance.getY ();
		
		for (int y = 0; y < TileY; y++) {
			for(int x = 0; x < TileX; x++) {
				if(Tile[x,y].GetComponent<TileState>().checkShip("Enemy")) {
					aiFleet.Add(Tile[x,y].GetComponent<TileState>().getShip("Enemy"));
				}
			}
		}
		return aiFleet;
	}

	public List<GameObject> getAiTargets() {
		List<GameObject> aiFleet = getAiFleet ();
		List<GameObject> targets = new List<GameObject> ();

		foreach(GameObject ship in aiFleet) {
			GameObject targetShip = ship.GetComponentInParent<EnemyShips>().getTargetShip();
			if(targetShip != null) {
				targets.Add(targetShip);
			}
		}
		return targets;
	}

	public int getAiTargetCount() {
		return getAiTargets ().Count;
	}
}
