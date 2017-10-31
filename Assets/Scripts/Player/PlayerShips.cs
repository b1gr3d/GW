using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerShips : Ship {
	public bool attacked = false;

	// level up stuff
	public int totalDamage = 0;
	public int currentExp = 0;
	public int maxExp = 20;
	private bool levelUp = true;

	// for playing levelup sound
	AudioClip levelUpSound;

	public enum ACTION { NONE, ATTACKING, HEALING, DEFENDING };
	public ACTION action;

	//Pilot
	[SerializeField] protected GameObject pilot;
	protected Pilot pilotClass;

	public void Awake() {
		action = ACTION.NONE;
		
		//Random Pilot Generator
		pilot = new GameObject ();
		pilot.AddComponent<Pilot> ();
		pilotClass = pilot.GetComponent<Pilot> ();
		range += pilotClass.focus;
	}

	void Start() {
		GameSystem.instance.increaseMovement (pilotClass.flight);
	}
	
	// Update is called once per frame
	public void Update () {
		if (GameSystem.instance.getGamePhase () == GameSystem.GAMEPHASE.PATK) {
			GainExp ();
		}

		if (currentExp >= maxExp) {
			LevelUp();
		}

		if (GameSystem.instance.getGamePhase() == GameSystem.GAMEPHASE.PATK) {
			if(targetShip != null) {
				if (!attacked)
				{
					int damage = Attack (targetShip);
					targetShip.GetComponentInParent<EnemyShips>().decreaseHealth(damage);
					attacked = true;
					GetComponentInChildren<LineRenderer>().enabled = false;
					targetShip.GetComponentInChildren<RawImage>().enabled = false;
					//GetComponentInChildren<InputMoveHandler>().setTargetUI(false);
				}
			}
		}
	}

	public override int Attack (GameObject target) {
		if (target == null)
			return 0;
		float distance = Mathf.Abs (target.transform.position.magnitude - transform.position.magnitude);
		turret.transform.LookAt (target.transform);
		//GetComponentInChildren<WeaponController>().TurretSocket[Get.curSocket] = turret.transform;
		StartCoroutine ("Fire");
		if (distance <= range) {
			if (target.GetComponentInParent<EnemyShips> ().getShield () > 0) {
				target.GetComponentInParent<EnemyShips> ().decreaseShield (Damage ());
				return 0;
			} else {
				int trueDamage = base.Attack (target);
				totalDamage += (int)trueDamage;
				return trueDamage;
			}
		} else
			return 0;
	}

	public override float Armor () {
		float reduction = (armor * 0.05f) / (1 + (0.061f * armor)); 
		return reduction;
	}

	public override int Damage () {
		return energy + pilotClass.combat;
	}

	public override void decreaseHealth(int val) {
		base.decreaseHealth(val);
	}

	public override void increaseHealth(int val) {
		base.increaseHealth(val);
	}
	
	public void setTargetShip(GameObject enemy) {
		targetShip = enemy;
	}

	public GameObject getTargetShip() {
		return targetShip;
	}

	public override void Reset() {
		attacked = false;
		GetComponentInChildren<InputMoveHandler> ().setTargetUI (false);
	}

	public void setAction(ACTION a) {
		action = a;
	}

	public ACTION getShipStatus() {
		return action;
	}

	public void LevelUp(){
		currentExp = 0;
		maxExp = maxExp + 30;
		level++;
		levelUp = true;

		// update health, armor and some other values
		health += 5;
		armor++;
	}

	// getting exp
	public void GainExp(){
		if(targetShip != null && targetShip.GetComponentInParent<EnemyShips>().shipStatus() == true) {
			currentExp += 20;
		}
	}

	public IEnumerator Fire() {
		int timer = Random.Range (2, 4);
		GetComponentInChildren<WeaponController>().Fire ();
		yield return new WaitForSeconds(timer);
		GetComponentInChildren<WeaponController>().Stop ();
		StopCoroutine ("Fire");
	}
}
