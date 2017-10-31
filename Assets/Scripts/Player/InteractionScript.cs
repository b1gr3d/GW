using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractionScript : MonoBehaviour {
	GameObject gameSystem = null;
	bool targeted = false;
	// Use this for initialization
	void Start () {
		gameSystem = GameObject.Find ("GameSystem");
		GetComponentInChildren<RawImage> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver() {
		if (Input.GetMouseButtonDown(2)) {
			Camera.main.GetComponent<CameraScript> ().resetLookAt();
			Camera.main.GetComponent<CameraScript> ().setLookAt(this.gameObject.transform.position);
			Camera.main.transform.LookAt (transform.position);
		}

		if (gameSystem.GetComponent<GameSystem> ().getGamePhase () == GameSystem.GAMEPHASE.ATTACK) {
			if(gameSystem.GetComponent<GameSystem>().getActiveShip() != null) {
				GameObject playerShip = gameSystem.GetComponent<GameSystem>().getActiveShip();
				float distance = Vector3.Distance(playerShip.transform.position, transform.position);
				distance = Mathf.Abs(distance);
				if(distance <= playerShip.GetComponentInParent<PlayerShips>().getRange()) {
					GetComponentInChildren<RawImage>().enabled = true;
				}
			}
		}
	}

	void OnMouseUp() {
		if (gameSystem.GetComponent<GameSystem> ().getGamePhase () == GameSystem.GAMEPHASE.ATTACK) {
			if(gameSystem.GetComponent<GameSystem>().getActiveShip() != null) {
				GameObject playerShip = gameSystem.GetComponent<GameSystem>().getActiveShip();
				float distance = Vector3.Distance(playerShip.transform.position, transform.position);
				distance = Mathf.Abs(distance);
				if(distance <= playerShip.GetComponentInParent<PlayerShips>().getRange() * GameSystem.instance.size) {
					playerShip.GetComponentInParent<PlayerShips>().setTargetShip(this.gameObject);
					targeted = !targeted;
					GetComponentInChildren<RawImage> ().enabled = true;
					playerShip.GetComponent<InputMoveHandler>().setTarget(transform.position, targeted);
					playerShip.GetComponent<InputMoveHandler>().setTargetUI(false);
				}
			}
		}
	}

	void OnMouseExit() {
		if(!targeted)
			GetComponentInChildren<RawImage> ().enabled = false;
	}
}
