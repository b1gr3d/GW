using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameSystem : MonoBehaviour {
	public static GameSystem instance = null;

	public enum GAMEPHASE { START, END, PLACEMENT, MOVEMENT, ATTACK, PATK, AI };
	[SerializeField] static GAMEPHASE gamePhase;
	[SerializeField] int movement, maxMovement = 10;
	[SerializeField] Text systemText = null, subText = null;
	[SerializeField] Button btn;
	[SerializeField] Slider movementBar = null;
	[SerializeField] public int count;

	[SerializeField] public Camera cam;
	public int size;

	GameObject[,] Tile;
	GameObject activeShip = null;
	public List<GameObject> fleet, aiFleet;

	void Awake() {
		instance = this;
		gamePhase = GAMEPHASE.START;

		if (size > 2) {
			size = 2;
		}
	}

	// Use this for initialization
	void Start () {
		GridManager.instance.CreateTile();
		Tile = GridManager.instance.getTile();

		movement = maxMovement;
		DisplayTitle ();
		FadeTitle (6.0f);
		fleet = new List<GameObject>();
		movementBar.maxValue = maxMovement;
		movementBar.value = movement;
		int i = Random.Range (1, 5);
		for (int n = 0; n < i; n++) {
			GetComponent<AiController> ().placeAiShip ();
		}

		cam.gameObject.SetActive(true);
		cam.GetComponent<CC_CrossStitch> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void setGamePhase(GAMEPHASE phase) {
		gamePhase = phase;
	}

	public GAMEPHASE getGamePhase() {
		return gamePhase;
	}

	public int getMovement() {
		return movement;
	}

	public int getMaxMovement() {
		return maxMovement;
	}

	public void decreaseMovement(short value) {
		if ((movement - value) >= 0)
			movement -= value;
		else {
			movement = 0;
		}
		movementBar.value = movement;

	}

	public void increaseMovement(int val) {
		movement += val;
		movementBar.value = movement;
	}

	public void setMovement(short value) {
		movement = value;
		movementBar.value = movement;
	}

	public GameObject[,] getTile() {
		return Tile;
	}

	void DisplayTitle() {
		systemText.enabled = true;
		//EventHandlerSubText.enabled = true;
		systemText.text = "GALATIC WARS - MURICA";
		subText.text = "---------------";
		//EventHandlerSubText.text = "\tby BinaryArc Studios";
	}
	
	void FadeTitle(float x) {
		systemText.CrossFadeAlpha (0, x, true);
		subText.CrossFadeAlpha (0, x - 1, true);
	}

	void FadeInText(Text text, string s, float x) {
		text.text = s;
		text.CrossFadeAlpha (1, x, true);
	}

	void FadeOutText(Text text, float x) {
		text.CrossFadeAlpha (0, x, true);
	}

	public void onGetNextPhase() {
		if (gamePhase == GAMEPHASE.START) {
			StartCoroutine ("onNextPhase");
		}
		else if (fleet.Count > 0)
			StartCoroutine ("onNextPhase");
		else 
			Debug.Log ("No Player Ships");
	}

	public IEnumerator onNextPhase() {
		if (gamePhase == GAMEPHASE.START) {
			gamePhase = GAMEPHASE.PLACEMENT;
			cam.GetComponent<CC_CrossStitch>().enabled = false;
		}
		else if (gamePhase == GAMEPHASE.PLACEMENT) {
			FadeInText(systemText, "MOVEMENT PHASE", 3f);
			FadeInText(subText, "Engage!", 2f);
			gamePhase = GAMEPHASE.MOVEMENT;
			ResetTile();
			yield return new WaitForSeconds(3);
			FadeOutText(systemText, 3f);
			FadeOutText(subText, 2f);
			yield return new WaitForSeconds(3);
		} else if (gamePhase == GAMEPHASE.MOVEMENT) {
			FadeInText(systemText, "ASSAULT PHASE", 3f);
			FadeInText(subText, "Plan your attacks!", 2f);
			gamePhase = GAMEPHASE.ATTACK;
			yield return new WaitForSeconds(3);
			FadeOutText(systemText, 3f);
			FadeOutText(subText, 2f);
			yield return new WaitForSeconds(3);
		} else if (gamePhase == GAMEPHASE.ATTACK) {
			Reset();
			gamePhase = GAMEPHASE.PATK;
			FadeInText(systemText, "ATTACKING", 3f);
			FadeInText(subText, "Engaging the enemy!", 2f);
			yield return new WaitForSeconds(3);
			FadeOutText(systemText, 3f);
			FadeOutText(subText, 2f);
		} else if (gamePhase == GAMEPHASE.PATK) {
			FadeInText(systemText, "OPPONENT PHASE", 3f);
			FadeInText(subText, "----------", 2f);
			gamePhase = GAMEPHASE.AI;
			btn.gameObject.SetActive(false);
			yield return new WaitForSeconds(3);
			FadeOutText(systemText, 3f);
			FadeOutText(subText, 2f);
			GetComponent<AiController>().callAi();
			yield return new WaitForSeconds(3);
		} else {
			FadeInText(systemText, "PLACEMENT PHASE", 3f);
			FadeInText(subText, "Place your units!", 2f);
			gamePhase = GAMEPHASE.PLACEMENT;
			SetTileZone();
			movement = maxMovement;
			movementBar.value = movement;
			btn.gameObject.SetActive(true);
			yield return new WaitForSeconds(3);
			FadeOutText(systemText, 3f);
			FadeOutText(subText, 2f);
			yield return new WaitForSeconds(3);
		}
	}

	public void setActiveShip(GameObject ship) {
		if(activeShip != null) 
			activeShip.GetComponent<InputMoveHandler> ().setTargetUI (false);
		activeShip = ship;
		activeShip.GetComponent<InputMoveHandler> ().setTargetUI (true);
	}

	public GameObject getActiveShip() {
		return activeShip;
	}

	public void Reset() {
		//movement = maxMovement;
		for (int i = 0; i < fleet.Count; i++) {
			fleet[i].GetComponent<PlayerShips>().Reset();
		}
		GetComponent<AiController> ().Reset ();
	}

	public void AddShip(GameObject ship) {
		fleet.Add (ship);
	}

	public void ResetTile() {
		int x = GridManager.instance.getX ();
		int y = GridManager.instance.getY ();

		for (int n = 0; n < y; n++) {
			for(int m = 0; m < x; m++) {
				if(Tile[n,m].GetComponent<TileState>().getTileState() != TileState.TileSTATE.OCCUPIED) {
					Tile[n,m].GetComponent<TileState>().SetDefaultZone();
				}
			}
		}
	}

	public void SetTileZone() {
		int x = GridManager.instance.getX ();
		int y = GridManager.instance.getY ();
		
		for (int n = 0; n < y; n++) {
			for(int m = 0; m < x; m++) {
				if(Tile[n,m].GetComponent<TileState>().getTileState() != TileState.TileSTATE.OCCUPIED) {
					if(Tile[n,m].GetComponent<TileState>().getDeploymentZone() == TileState.TileZONE.PLAYER) {
						Tile[n,m].GetComponent<TileState>().SetPlayerZone();
					}
					else if(Tile[n,m].GetComponent<TileState>().getDeploymentZone() == TileState.TileZONE.AI) {
						Tile[n,m].GetComponent<TileState>().SetAiZone();
					}
				}
			}
		}
	}

	public void RemoveShip(GameObject ship) {
		fleet.Remove (ship);
	}
}
