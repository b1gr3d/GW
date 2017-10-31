using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyShips : Ship {

	// Use this for initialization
	void Start () {
		health = 10;
		armor = 1;
		shield = health * 2;
		energy = 3;
		range = 4;
		HealthBar.maxValue = health;
		HealthBar.value = health;

		ShieldBar.maxValue = shield;
		ShieldBar.value = shield;
	}
	
	// Update is called once per frame
	void Update () {
		HealthBar.value = health;
	}

	public override void Reset() {

	}

	public override int Attack (GameObject target) {
		if (target == null) {
			return 0;
		}
		float distance = Mathf.Abs (target.transform.position.magnitude - transform.position.magnitude);
		if (distance <= range) {
			if (target.GetComponentInParent<PlayerShips> ().getShield () > 0) {
				target.GetComponentInParent<PlayerShips> ().decreaseShield (Damage ());
				return 0;
			} else {
				float damageReduction = Damage () * target.GetComponentInParent<PlayerShips> ().Armor ();
				float trueDamage = Damage () - damageReduction;
				
				return (int)trueDamage;
			}
		} else {
			Debug.Log ("Ai out of range");
			return 0;
		}
	}

	public override float Armor () {
		float reduction = (armor * 0.05f) / ( 1 + (0.061f * armor));
		return reduction;
	}

	public override	int Damage () {
		return energy;
	}

	public void setTargetShip(GameObject target) {
		targetShip = target;
	}

	public GameObject getTargetShip() {
		return targetShip;
	}

	public bool shipStatus(){
		if (health <= 0) {
			return true;
		} else {
			return false;
		}
	}
}
