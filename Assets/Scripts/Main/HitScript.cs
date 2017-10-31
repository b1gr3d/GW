using UnityEngine;
using System.Collections;

public class HitScript : MonoBehaviour {
	public GameObject explosion;

	// Update is called once per frame
	void Update () {
		if (GetComponent<Ship> ().getHealth () <= 0) {
			Instantiate(explosion, transform.position, transform.rotation);
			if(this.tag == "Player") {
				GameSystem.instance.RemoveShip(this.gameObject);
				GameObject TileObject = GetComponentInChildren<InputMoveHandler>().getTileObject();
				if(TileObject != null) {
					TileObject.GetComponent<TileState>().SetDefault();
				}
			}
			else {
				AiController.instance.RemoveAiShip(this.gameObject);
				GetComponent<Executer>().checkTile();
				GameObject TileObject = GetComponent<Executer>().TileObject;
				TileObject.GetComponent<TileState>().SetDefault();
			}
			Destroy(this.gameObject);
		}
	}
}
