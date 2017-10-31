using UnityEngine;
using System.Collections;

public class TileState : MonoBehaviour {
	[SerializeField] Material redNode = null, defaultNode = null, greenNode = null;

	public enum TileSTATE { EMPTY, OCCUPIED, HIGHLIGHT } ;
	public enum TileZONE { NONE, PLAYER, AI };
	enum TileSTATUS { NONE, EMP, GAS, ASTEROID, ELECTRIC } ;
	// Use this for initialization
	[SerializeField] TileSTATE NodeState;
	private TileSTATUS NodeStatus;
	public TileZONE deploymentZone = TileZONE.NONE;

	void Start () {
		NodeState = TileSTATE.EMPTY;
		NodeStatus = TileSTATUS.NONE;
	}

	// Update is called once per frame
	void Update () {
		if ( NodeStatus == TileSTATUS.EMP ) {
			
		}
		else if ( NodeStatus == TileSTATUS.GAS ) {

		}
		else if ( NodeStatus == TileSTATUS.ASTEROID ) {

		}
		else if ( NodeStatus == TileSTATUS.ELECTRIC ) {

		}
	}

	public void SetOccupied() {
		transform.gameObject.GetComponent<Renderer> ().material = redNode;
		NodeState = TileSTATE.OCCUPIED;
	}

	public void SetDefault() {
		transform.gameObject.GetComponent<Renderer> ().material = defaultNode;
		NodeState = TileSTATE.EMPTY;
	}

	public void setHightLight() {
		transform.gameObject.GetComponent<Renderer> ().material = greenNode;
		NodeState = TileSTATE.HIGHLIGHT;
	}

	public TileSTATE getTileState() {
		return NodeState;
	}

	public void SetPlayerZone() {
		deploymentZone = TileZONE.PLAYER;
		transform.gameObject.GetComponent<Renderer> ().material = greenNode;
	}

	public void SetAiZone() {
		deploymentZone = TileZONE.AI;
		transform.gameObject.GetComponent<Renderer> ().material = redNode;
	}

	public void SetDefaultZone() {
		transform.gameObject.GetComponent<Renderer> ().material = defaultNode;
	}

	public TileZONE getDeploymentZone() {
		return deploymentZone;
	}

	public bool checkShip(string type) {
		RaycastHit hit;
		Ray ray = new Ray (transform.position, Vector3.up);
		bool isType = false;
		if (Physics.Raycast (ray, out hit, 10f)) {
			if (hit.collider.tag == type) {
				isType = true;
			}
		} 
		return isType;
	}

	public GameObject getShip(string type) {
		RaycastHit hit;
		Ray ray = new Ray (transform.position, Vector3.up);
		GameObject ship = null;
		if (Physics.Raycast (ray, out hit, 10f)) {
			if (hit.collider.tag == type) {
				ship = hit.collider.gameObject;
			}
		} 
		return ship;
	}
}
