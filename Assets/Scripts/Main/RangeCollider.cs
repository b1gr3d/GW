using UnityEngine;
using System.Collections;

public class RangeCollider : MonoBehaviour {
	void OnTriggerEnter() {
		GameObject playerShip = GameObject.FindGameObjectWithTag("Player");
		if (playerShip.tag == "Player") {
			GetComponentInParent<EnemyShips> ().setTargetShip(playerShip);
		}
	}

	void OnTriggerExit() {
		GameObject playerShip = GameObject.FindGameObjectWithTag("Player");
		if (playerShip.tag == "Player") {
			GetComponentInParent<EnemyShips> ().setTargetShip(null);
		}
	}
}
