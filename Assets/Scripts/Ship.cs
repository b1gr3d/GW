using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Ship : MonoBehaviour {
	[SerializeField] protected Slider HealthBar, ShieldBar;
	[SerializeField] protected int level;
	[SerializeField] protected int health;
	[SerializeField] protected int armor;
	[SerializeField] protected int shield;
	[SerializeField] protected int energy;
	[SerializeField] protected int range;

	[SerializeField] protected GameObject targetShip = null;
	public Transform turret;

	public abstract void Reset ();

	public virtual int Attack (GameObject target) {
		float damageReduction = Damage () * target.GetComponentInParent<EnemyShips>().Armor();
		float trueDamage = Damage () - damageReduction;

		return (int)trueDamage;
	}

	public virtual float Armor () {
		float reduction = (armor * 0.05f) / (1 + (0.061f * armor)); 
		return reduction;
	}

	public virtual int Damage () {
		return energy;
	}

	public void setLevel(int value){
		level = value;
	}

	public int getLevel(){
		return level;
	}

	public void setHealth(int value) {
		health = value;
	}

	public int getHealth() {
		return health;
	}

	public virtual void decreaseHealth(int value) {
		if ((health - value) > 0) {
			health -= value;
		} else {
			health = 0;
		}
		HealthBar.value = health;
	}

	public virtual void increaseHealth(int value) {
		health += value;
		HealthBar.value = health;
	}

	public void updateHealth () {
		HealthBar.value = health;
	}

	public void increaseShield(int value) {
		shield += value;
		ShieldBar.value = shield;
	}

	public void decreaseShield(int value) {
		if ((shield - value) > 0) {
			shield -= value;
		} else {
			shield = 0;
		}
		ShieldBar.value = shield;
	}

	public int getShield() {
		return shield;
	}

	public void setRange(int value) {
		range = value;
	}

	public int getRange() {
		return range;
	}
}
